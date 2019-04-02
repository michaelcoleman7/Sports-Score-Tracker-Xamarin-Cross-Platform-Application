using Plugin.SimpleAudioPlayer;
using ScoreTracker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScoreTracker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GAA : ContentPage
	{
		public GAA ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        private ISimpleAudioPlayer audioPlayer;
        List<MatchClass> gaaList = new List<MatchClass>();
        List<MatchClass> existingList = new List<MatchClass>();

        private void AddHome1Goal_Clicked(object sender, EventArgs e)
        {
            //Change homegoals text to string then convert to an integer - add 1, then set to text property
            string score = homeGoals.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 1;
            homeGoals.Text = addscore.ToString();
        }

        private void AddHome1Point_Clicked(object sender, EventArgs e)
        {
            //Change homepoints text to string then convert to an integer - add 1, then set to text property
            string score = homePoints.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 1;
            homePoints.Text = addscore.ToString();
        }

        private void AddAway1Goal_Clicked(object sender, EventArgs e)
        {
            //Change awaygoals text to string then convert to an integer - add 1, then set to text property
            string score = awayGoals.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 1;
            awayGoals.Text = addscore.ToString();
        }

        private void AddAway1Point_Clicked(object sender, EventArgs e)
        {
            //Change awaypoints text to string then convert to an integer - add 1, then set to text property
            string score = awayPoints.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 1;
            awayPoints.Text = addscore.ToString();
        }

        private async void SaveGame_Clicked(object sender, EventArgs e)
        {
            //if match name is left empty by user
            if (matchName.Text == null || matchName.Text.Trim() == "")
            {
                //alert user they must enter a match name
                await DisplayAlert("Save Requirement", "Match Name cannot be empty", "OK");
            }
            else
            {
                //boolean to determine if match name already exists
                Boolean matchExists = false;
                //read in all existing matches into existingList
                existingList = MatchClass.ReadList();

                //if no matches exist in existingList
                if (existingList == null)
                {
                    //save match to file and return to main menu
                    SaveandReturn();
                }
                //if matches are loaded into existingList
                else
                {
                    //loop through each item in existing list and see if match name exists already
                    foreach (var mc in existingList)
                    {
                        //if match name is found
                        if (mc.MatchName == matchName.Text.Trim())
                        {
                            matchExists = true;
                        }
                    }

                    //if name already exists display alert
                    if (matchExists)
                    {
                        await DisplayAlert("Duplication Error", "Match Name already exists, please enter another", "OK");
                    }
                    //if name doesn't exist already then save match
                    else
                    {
                        //save match to file and return to main menu
                        SaveandReturn();
                    }
                }
            }
        }

        //Method used to save match to file, play sound effect and return to main menu
        private async void SaveandReturn()
        {
            string homeScore = homeGoals.Text + "-" + homePoints.Text;
            string awayScore = awayGoals.Text + "-" + awayPoints.Text;
            //create new match class and add to gaaList
            MatchClass mc = new MatchClass(gameType.Text, homeTeam.Text, homeScore, awayTeam.Text, awayScore, matchName.Text.Trim());
            gaaList.Add(mc);
            MatchClass.SaveMatchDataToFile(gaaList);

            //Add audio to application when game is saved - referenced from https://forums.xamarin.com/discussion/145050/beep-in-xamarin
            audioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            Stream audioStream = GetType().Assembly.GetManifestResourceStream("ScoreTracker.AudioFiles.fulltime.mp3");
            bool isSuccess = audioPlayer.Load(audioStream);
            audioPlayer.Play();

            await Navigation.PushAsync(new MainPage());
        }
    }
}