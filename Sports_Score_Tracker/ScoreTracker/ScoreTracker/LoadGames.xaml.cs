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
            //turn off navigation bar
            NavigationPage.SetHasNavigationBar(this, false);

            var assembly = typeof(LoadGames);

            // Choose between platform/build options for each device
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    //setup background image for android
                    string androidBackground = "ScoreTracker.Assets.Images.loadgamesandroid.png";
                    imgBackground.Source = ImageSource.FromResource(androidBackground, assembly);

                    //setup text colors for page elements for android
                    homeScorelbl.TextColor = Color.White;
                    awayScorelbl.TextColor = Color.White;
                    homeTeamlbl.TextColor = Color.White;
                    awayTeamlbl.TextColor = Color.White;
                    homeSclbl.TextColor = Color.White;
                    awaySclbl.TextColor = Color.White;
                    homeNamelbl.TextColor = Color.White;
                    awayNamelbl.TextColor = Color.White;
                    break;
                case Device.UWP:
                    //setup background image for uwp
                    string uwpBackground = "ScoreTracker.Assets.Images.loadgamesuwp.jpeg";
                    imgBackground.Source = ImageSource.FromResource(uwpBackground, assembly);
                    break;
                default:
                    break;
            }
            //if null, create a new List of MatchClasses
            if (matchList == null) matchList = new List<MatchClass>();

            //call Readlist function from matchclass in order to populate matchList
            matchList = MatchClass.ReadList();

            // Set item source for the list view
            MatchesListView.ItemsSource = matchList;

        }

        //Method for binding data from list view to elements to edit data
        private void MatchesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // set the binding context for each stacklayout to be the selected item on the listview
            ListItemGameType.BindingContext = (MatchClass)e.SelectedItem;
            ListItemHomeTeam.BindingContext = (MatchClass)e.SelectedItem;
            ListItemAwayTeam.BindingContext = (MatchClass)e.SelectedItem;
            ListItemHomeScore.BindingContext = (MatchClass)e.SelectedItem;
            ListItemAwayScore.BindingContext = (MatchClass)e.SelectedItem;
        }

        //Method to update data based on the information entered by the user 
        private void UpdateBtn_Clicked(object sender, EventArgs e)
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
            //Refresh list view
            MatchesListView.ItemsSource = null;
            MatchesListView.ItemsSource = matchList;

            //Save updated data to file
            MatchClass.SaveMatchDataToFile(matchList);
        }

        //Method to delete the selected match from the list view and save deletion
        private void DeleteBtn_Clicked(object sender, EventArgs e)
        {
            //loops through each item in list, needs to be converted toList(), in order to delete during loop
            foreach (var mc in matchList.ToList())
            {
                //if MatchClass in list'd matchName is equal to matchNamelbl.Text (equal to user selected items name)
                if (mc.MatchName == matchNamelbl.Text)
                {
                    //remove MatchClass from list
                    matchList.Remove(mc);
                }
            }
            //Refresh list view
            MatchesListView.ItemsSource = null;
            MatchesListView.ItemsSource = matchList;

            //Save deletion to file
            MatchClass.SaveMatchDataToFile(matchList);
        }
    }
}