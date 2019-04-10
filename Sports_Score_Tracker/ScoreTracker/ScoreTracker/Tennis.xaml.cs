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
            ImgSound.Source = ImageSource.FromResource(soundOption, assembly);

            //Set reset image source
            string ResetImage = "ScoreTracker.Assets.Images.reset.png";
            ImgReset.Source = ImageSource.FromResource(ResetImage, assembly);

            // Choose between platform/build options for each device
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    //setup background image
                    string androidBackground = "ScoreTracker.Assets.Images.tennisandroid.jpg";
                    ImgBackground.Source = ImageSource.FromResource(androidBackground, assembly);
                    HomeScorelbl.TextColor = Color.Black;
                    AwayScorelbl.TextColor = Color.Black;
                    break;
                case Device.UWP:
                    //setup background image
                    string uwpBackground = "ScoreTracker.Assets.Images.tennisuwp.jpg";
                    ImgBackground.Source = ImageSource.FromResource(uwpBackground, assembly);
                    break;
                default:
                    break;
            }
        }

        //Method to add points to home team
        private async void AddHome_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 1, then set to text property
            string score = HomeScorelbl.Text.ToString();
            int addscore = Convert.ToInt32(score);

            //Add 15 to score for 1st 2 scores
            if (addscore == 0 || addscore == 15)
            {
                //add to score and update homeScore text
                addscore = addscore + 15;
                HomeScorelbl.Text = addscore.ToString();
            }
            //add 10 to 3rd score
            else if (addscore == 30)
            {
                //add to score and update homeScore text
                addscore = addscore + 10;
                HomeScorelbl.Text = addscore.ToString();

                if (AwayScorelbl.Text == "40")
                {
                    //Display an alert which returns the user selected value - referenced https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/navigation/pop-ups
                    string winner = await DisplayActionSheet("Deuce: Select who wins Deuce?", "Cancel", null, HomeTeamlbl.Text, AwayTeamlbl.Text);

                    //Add (W) to the winner of the deuce's name, so that it's clear who won the match
                    if (winner == HomeTeamlbl.Text)
                    {
                        //add (W) to home team
                        HomeTeamlbl.Text = HomeTeamlbl.Text + " (W)";
                    }
                    else if (winner == AwayTeamlbl.Text)
                    {
                        //add (W) to away team name
                        AwayTeamlbl.Text = AwayTeamlbl.Text + " (W)";
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
            string score = AwayScorelbl.Text.ToString();
            int addscore = Convert.ToInt32(score);
            //Add 15 to score for 1st 2 scores
            if (addscore == 0 || addscore == 15)
            {
                //add to score and update awayScore text
                addscore = addscore + 15;
                AwayScorelbl.Text = addscore.ToString();
            }
            //add 10 to 3rd score
            else if (addscore == 30)
            {
                //add to score and update awayScore text
                addscore = addscore + 10;
                AwayScorelbl.Text = addscore.ToString();

                //Handle Deuce encounter and decide a winner
                if (HomeScorelbl.Text == "40")
                {
                    //Display an alert which returns the user selected value
                    string winner = await DisplayActionSheet("Deuce: Select who wins Deuce?", "Cancel", null, HomeTeamlbl.Text, AwayTeamlbl.Text);

                    //Add (W) to the winner of the deuce's name, so that it's clear who won the match
                    if (winner == HomeTeamlbl.Text)
                    {
                        //add (W) to home team name
                        HomeTeamlbl.Text = HomeTeamlbl.Text + " (W)";
                    }
                    else if(winner == AwayTeamlbl.Text)
                    {
                        //add (W) to away team name
                        AwayTeamlbl.Text = AwayTeamlbl.Text + " (W)";
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
            if (MatchNamelbl.Text == null || MatchNamelbl.Text.Trim() == "")
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
                        if (mc.MatchName == MatchNamelbl.Text.Trim())
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
            MatchClass s = new MatchClass(GameTypelbl.Text, HomeTeamlbl.Text, HomeScorelbl.Text, AwayTeamlbl.Text, AwayScorelbl.Text, MatchNamelbl.Text.Trim());
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
                ImgSound.Source = ImageSource.FromResource(soundOption, assembly);
                //set sound option equal to false
                soundOn = false;
            }
            //if sound option is turned off, swap image back to sound on image
            else
            {
                //set image source to sound on icon
                var assembly = typeof(Tennis);
                string soundOption = "ScoreTracker.Assets.Images.soundonlight.png";
                ImgSound.Source = ImageSource.FromResource(soundOption, assembly);
                //set sound option equal to true
                soundOn = true;
            }
        }

        //Method to reset scores
        private void ImgReset_Tapped(object sender, EventArgs e)
        {
            HomeScorelbl.Text = "0";
            AwayScorelbl.Text = "0";
        }
    }
}