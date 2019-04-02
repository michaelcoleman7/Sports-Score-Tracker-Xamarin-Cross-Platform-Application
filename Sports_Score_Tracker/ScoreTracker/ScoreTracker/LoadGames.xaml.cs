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

            MatchClass.SaveUpdatedMatchDataToFile(matchList);
        }
    }
}