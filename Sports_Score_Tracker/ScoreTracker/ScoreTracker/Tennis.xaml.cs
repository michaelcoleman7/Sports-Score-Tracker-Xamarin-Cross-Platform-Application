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
            SetupDefaults();
        }
        //Variables used throughout page
        private ISimpleAudioPlayer audioPlayer;
        //bool variable needed for deciding to mute sound or not - on by default
        bool soundOn = true;
        List<MatchClass> tennisList = new List<MatchClass>();
        List<MatchClass> existingList = new List<MatchClass>();

        //Method to setup default values needed for page setup
        public void SetupDefaults()
        {
            //Turn off Navgation bar
            NavigationPage.SetHasNavigationBar(this, false);

            var assembly = typeof(Tennis);
            //Set default image source for sound icon (default is on)
            string soundOption = "ScoreTracker.Assets.Images.soundonlight.png";
            imgSound.Source = ImageSource.FromResource(soundOption, assembly);

            // Choose between platform/build options for each device
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    //setup background image
                    string androidBackground = "ScoreTracker.Assets.Images.tennisandroid.jpg";
                    imgBackground.Source = ImageSource.FromResource(androidBackground, assembly);
                    homeScore.TextColor = Color.Black;
                    awayScore.TextColor = Color.Black;
                    break;
                case Device.UWP:
                    //setup background image
                    string uwpBackground = "ScoreTracker.Assets.Images.tennisuwp.jpg";
                    imgBackground.Source = ImageSource.FromResource(uwpBackground, assembly);
                    break;
                default:
                    break;
            }
        }

        //Method to add points to home team
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
                        //add (W) to home team name
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

        //Method used to save game and ensure save requirments are met
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
                if (existingList == null )
                {
                    //save match to file and return to main menu
                    SaveandReturn();
                }
                //if matches are loaded into existingList
                else
                {
                    //games exist, therefore populate list with games for adding and saving later
                    tennisList = MatchClass.ReadList();

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
            //create new match class and add to tennisList
            MatchClass s = new MatchClass(gameType.Text, homeTeam.Text, homeScore.Text, awayTeam.Text, awayScore.Text, matchName.Text.Trim());
            tennisList.Add(s);
            MatchClass.SaveMatchDataToFile(tennisList);

            //if the sound option is left/turned on via double clicking sound icon
            if (soundOn)
            {
                //Add audio to application when game is saved - referenced from https://forums.xamarin.com/discussion/145050/beep-in-xamarin
                audioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                Stream audioStream = GetType().Assembly.GetManifestResourceStream("ScoreTracker.AudioFiles.TennisServe.mp3");
                bool isSuccess = audioPlayer.Load(audioStream);
                audioPlayer.Play();
            }

            //return to Mainpage
            await Navigation.PushAsync(new MainPage());
        }

        //Method to determine if sound should be played and which icon should be displayed - dblclick needed on image to change
        private void ImgSound_Tapped(object sender, EventArgs e)
        {
            //if sound option is on, swap image to mute image
            if (soundOn)
            {
                //set image source to mute
                var assembly = typeof(Tennis);
                string soundOption = "ScoreTracker.Assets.Images.mutelight.png";
                imgSound.Source = ImageSource.FromResource(soundOption, assembly);
                //set sound option equal to false
                soundOn = false;
            }
            //if sound option is turned off, swap image back to sound on image
            else
            {
                //set image source to sound on icon
                var assembly = typeof(Tennis);
                string soundOption = "ScoreTracker.Assets.Images.soundonlight.png";
                imgSound.Source = ImageSource.FromResource(soundOption, assembly);
                //set sound option equal to true
                soundOn = true;
            }
        }
    }
}