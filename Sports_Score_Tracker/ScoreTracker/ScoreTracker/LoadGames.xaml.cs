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

        //Method to set up default settings needed for page
        private void SetupDefaults()
        {
            if (matchList == null) matchList = new List<MatchClass>();

            //call readlist function in order to populate matchList
            matchList = ReadList();

            // Set data context for the list view
            MatchesListView.ItemsSource = matchList;

        }

        private static List<MatchClass> ReadList()
        {
            List<MatchClass> myList = new List<MatchClass>();
            string jsonText;

            // Read localApplicationFolder
            try
            {
                string path = Environment.GetFolderPath(
                                Environment.SpecialFolder.LocalApplicationData);
                string filename = Path.Combine(path, "SavedGames.txt");
                using (var reader = new StreamReader(filename))
                {
                    //read text file contents into jsontext
                    jsonText = reader.ReadToEnd();
                }
            }
            // if unable to read localApplicationFolder, read the default file
            catch
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(
                                                typeof(MainPage)).Assembly;
                // Create stream
                Stream stream = assembly.GetManifestResourceStream(
                                    "ScoreTracker.DataFiles.SavedGames.txt");
                try
                {
                    using (var reader = new StreamReader(stream))
                    {
                        //read text file contents into jsontext
                        jsonText = reader.ReadToEnd();
                    }
                }
                //catch when trying to read file if it doesn't exist
                catch (Exception)
                {
                    //set jsontext to empty string so serializing is not being carried out on null string when file doesn't exist
                    jsonText = "";
                }
            }
            //deserialize json text into myList and return myList
            myList = JsonConvert.DeserializeObject<List<MatchClass>>(jsonText);
            return myList;
        }

        private void MatchesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // set the binding context for each stacklayout to be the selected item on the listview
            ListItemGameType.BindingContext = (MatchClass)e.SelectedItem;
            ListItemHomeTeam.BindingContext = (MatchClass)e.SelectedItem;
            ListItemAwayTeam.BindingContext = (MatchClass)e.SelectedItem;
            ListItemHomeScore.BindingContext = (MatchClass)e.SelectedItem;
            ListItemAwayScore.BindingContext = (MatchClass)e.SelectedItem;
        }
    }
}