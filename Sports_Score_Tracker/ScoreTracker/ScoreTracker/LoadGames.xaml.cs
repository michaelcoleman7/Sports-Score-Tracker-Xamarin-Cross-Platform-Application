using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ScoreTracker.Models;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScoreTracker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadGames : ContentPage
	{
        List<MatchClass> matchList;
        public LoadGames ()
		{
			InitializeComponent ();
            SetupDefaults();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        private void SetupDefaults()
        {
            if (matchList == null) matchList = new List<MatchClass>();

            matchList = ReadList();
            //MatchClass test = new MatchClass("1","1","1","1");
            //matchList.Add(test);
            // set the data context for the list view
            lvMatches.ItemsSource = matchList;
            

        }

        private static List<MatchClass> ReadList()
        {
            List<MatchClass> myList = new List<MatchClass>();
            string jsonText;

            try  // reading the localApplicationFolder first
            {
                string path = Environment.GetFolderPath(
                                Environment.SpecialFolder.LocalApplicationData);
                string filename = Path.Combine(path, "SavedGames.txt");
                using (var reader = new StreamReader(filename))
                {
                    jsonText = reader.ReadToEnd();
                    // need json library
                }
            }
            catch // fallback is to read the default file
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(
                                                typeof(MainPage)).Assembly;
                // create the stream
                Stream stream = assembly.GetManifestResourceStream(
                                    "ScoreTracker.DataFiles.SavedGames.txt");
                using (var reader = new StreamReader(stream))
                {
                    jsonText = reader.ReadToEnd();
                    // include JSON library now
                }
            }

            myList = JsonConvert.DeserializeObject<List<MatchClass>>(jsonText);

            return myList;
        }

        private void LvMatches_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}