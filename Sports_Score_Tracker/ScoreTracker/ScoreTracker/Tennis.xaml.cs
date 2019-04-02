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
	public partial class Tennis : ContentPage
	{
		public Tennis ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private ISimpleAudioPlayer audioPlayer;
        List<MatchClass> tennisList = new List<MatchClass>();
        List<MatchClass> existingList = new List<MatchClass>();

        private async void AddHome_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 1, then set to text property
            string score = homeScore.Text.ToString();
            int addscore = Convert.ToInt32(score);

            //Add 15 to score for 1st 2 scores
            if (addscore == 0 || addscore == 15)
            {
                //add to score and update homeScore text
                addscore = addscore + 15;
                homeScore.Text = addscore.ToString();
            }
            //add 10 to 3rd score
            else if (addscore == 30)
            {
                //add to score and update homeScore text
                addscore = addscore + 10;
                homeScore.Text = addscore.ToString();

                if (awayScore.Text == "40")
                {
                    //Display an alert which returns the user selected value
                    string winner = await DisplayActionSheet("Deuce: Select who wins Deuce?", "Cancel", null, homeTeam.Text, awayTeam.Text);

                    //Add (W) to the winner of the deuce's name, so that it's clear who won the match
                    if (winner == homeTeam.Text)
                    {
                        //add (W) to home team
                        homeTeam.Text = homeTeam.Text + " (W)";
                    }
                    else if (winner == awayTeam.Text)
                    {
                        //add (W) to away team name
                        awayTeam.Text = awayTeam.Text + " (W)";
                    }
                }
            }
            else
            {
                //display alert to let user know the score cannot exceed 40
                await DisplayAlert("Scoring Alert", "Scoring cannot go above 40", "OK");
            }
        }

        private async void AddAway_Clicked(object sender, EventArgs e)
        {
            //Change awayscore text to string then convert to an integer - add 1, then set to text property
            string score = awayScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            //Add 15 to score for 1st 2 scores
            if (addscore == 0 || addscore == 15)
            {
                //add to score and update awayScore text
                addscore = addscore + 15;
                awayScore.Text = addscore.ToString();
            }
            //add 10 to 3rd score
            else if (addscore == 30)
            {
                //add to score and update awayScore text
                addscore = addscore + 10;
                awayScore.Text = addscore.ToString();

                //Handle Deuce encounter and decide a winner
                if (homeScore.Text == "40")
                {
                    //Display an alert which returns the user selected value
                    string winner = await DisplayActionSheet("Deuce: Select who wins Deuce?", "Cancel", null, homeTeam.Text, awayTeam.Text);

                    //Add (W) to the winner of the deuce's name, so that it's clear who won the match
                    if (winner == homeTeam.Text)
                    {
                        //add (W) to home team
                        homeTeam.Text = homeTeam.Text + " (W)";
                    }
                    else if(winner == awayTeam.Text)
                    {
                        //add (W) to away team name
                        awayTeam.Text = awayTeam.Text + " (W)";
                    }    
                }
            }
            else
            {
                // Adapted from - https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/navigation/pop-ups
                await DisplayAlert("Alert", "Scoring cannot go above 40", "OK");
            }
        }

        private async void SaveGame_Clicked(object sender, EventArgs e)
        {
            //if match name is left empty by user
            if (matchName.Text == null)
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
                        if (mc.MatchName == matchName.Text)
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
            //create new match class and add to tennisList
            MatchClass s = new MatchClass(gameType.Text, homeTeam.Text, homeScore.Text, awayTeam.Text, awayScore.Text, matchName.Text);
            tennisList.Add(s);
            MatchClass.SaveMatchDataToFile(tennisList);

            //Add audio to application when game is saved - referenced from https://forums.xamarin.com/discussion/145050/beep-in-xamarin
            audioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            Stream audioStream = GetType().Assembly.GetManifestResourceStream("ScoreTracker.AudioFiles.TennisServe.mp3");
            bool isSuccess = audioPlayer.Load(audioStream);
            audioPlayer.Play();

            //return to Mainpage
            await Navigation.PushAsync(new MainPage());
        }
    }
}