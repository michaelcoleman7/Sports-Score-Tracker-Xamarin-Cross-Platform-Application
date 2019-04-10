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
                    ImgBackground.Source = ImageSource.FromResource(androidBackground, assembly);
                    //Setup android colour scheme
                    SetAndroidColours();
                    break;
                case Device.UWP:
                    //setup background image for uwp
                    string uwpBackground = "ScoreTracker.Assets.Images.loadgamesuwp.jpeg";
                    ImgBackground.Source = ImageSource.FromResource(uwpBackground, assembly);
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
                if (mc.MatchName == MatchNamelbl.Text)
                {
                    mc.HomeTeam = HomeTeamlbl.Text;
                    mc.HomeScore = HomeScorelbl.Text;
                    mc.AwayTeam = AwayTeamlbl.Text;
                    mc.AwayScore = AwayScorelbl.Text;
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
                if (mc.MatchName == MatchNamelbl.Text)
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

        //Method to set up colours for UWP elements
        private void SetAndroidColours()
        {
            //setup text colors for page elements for android
            HomeScorelbl.TextColor = Color.White;
            AwayScorelbl.TextColor = Color.White;
            HomeTeamlbl.TextColor = Color.White;
            AwayTeamlbl.TextColor = Color.White;
            HomeSclbl.TextColor = Color.White;
            AwaySclbl.TextColor = Color.White;
            HomeNamelbl.TextColor = Color.White;
            AwayNamelbl.TextColor = Color.White;
        }
    }
}