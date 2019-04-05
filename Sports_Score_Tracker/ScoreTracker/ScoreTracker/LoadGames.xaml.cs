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

        }

        //Method to set up default settings needed for page
        private void SetupDefaults()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            var assembly = typeof(LoadGames);

            // Choose between platform/build options for each device
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    //setup background image
                    string androidBackground = "ScoreTracker.Assets.Images.loadgamesandroid.png";
                    imgBackground.Source = ImageSource.FromResource(androidBackground, assembly);
                    homeScorelbl.TextColor = Color.White;
                    awayScorelbl.TextColor = Color.White;
                    homeTeamlbl.TextColor = Color.White;
                    awayTeamlbl.TextColor = Color.White;
                    homeSclbl.TextColor = Color.White;
                    awaySclbl.TextColor = Color.White;
                    homeNamelbl.TextColor = Color.White;
                    awayNamelbl.TextColor = Color.White;
                    

                    //setup text colors for page elements
                    //lvGameType.TextColor = Color.White;
                    break;
                case Device.UWP:
                    //setup background image
                    string uwpBackground = "ScoreTracker.Assets.Images.loadgamesuwp.jpeg";
                    imgBackground.Source = ImageSource.FromResource(uwpBackground, assembly);
                    break;
                default:
                    break;
            }

            if (matchList == null) matchList = new List<MatchClass>();

            //call readlist function from matchclass in order to populate matchList
            matchList = MatchClass.ReadList();

            // Set data context for the list view
            MatchesListView.ItemsSource = matchList;

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

        private void SaveBtn_Clicked(object sender, EventArgs e)
        {
            foreach (var mc in matchList)
            {
                if (mc.MatchName == matchNamelbl.Text)
                {
                    mc.HomeTeam = homeTeamlbl.Text;
                    mc.HomeScore = homeScorelbl.Text;
                    mc.AwayTeam = awayTeamlbl.Text;
                    mc.AwayScore = awayScorelbl.Text;
                }
            }
            MatchesListView.ItemsSource = null;
            MatchesListView.ItemsSource = matchList;

            MatchClass.SaveMatchDataToFile(matchList);
        }

        private void DeleteBtn_Clicked(object sender, EventArgs e)
        {
            //loops through each item in list, needs to be converted toList(), in order to delete during loop
            foreach (var mc in matchList.ToList())
            {
                if (mc.MatchName == matchNamelbl.Text)
                {
                    matchList.Remove(mc);
                }
            }
            MatchesListView.ItemsSource = null;
            MatchesListView.ItemsSource = matchList;

            MatchClass.SaveMatchDataToFile(matchList);
        }
    }
}