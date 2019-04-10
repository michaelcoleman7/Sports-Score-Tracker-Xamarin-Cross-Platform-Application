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
	public partial class Basketball : ContentPage
	{
		public Basketball ()
		{
			InitializeComponent ();
            SetupDefaults();
        }
        //variables needed throughout page
        private ISimpleAudioPlayer audioPlayer;
        //bool variable needed for deciding to mute sound or not - on by default
        bool soundOn = true;
        List<MatchClass> basketballList = new List<MatchClass>();
        List<MatchClass> existingList = new List<MatchClass>();

        //Method to setup default values needed for page setup
        public void SetupDefaults()
        {
            //Turn off navigation bar
            NavigationPage.SetHasNavigationBar(this, false);

            var assembly = typeof(Basketball);
            //Set default image source for sound icon (default is on)
            string soundOption = "ScoreTracker.Assets.Images.soundonlight.png";
            imgSound.Source = ImageSource.FromResource(soundOption, assembly);

            //Set reset image source
            string ResetImage = "ScoreTracker.Assets.Images.reset.png";
            imgReset.Source = ImageSource.FromResource(ResetImage, assembly);

            // Choose between platform/build options for each device
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    //setup background image for android
                    string androidBackground = "ScoreTracker.Assets.Images.basketballcourt.jpg";
                    imgBackground.Source = ImageSource.FromResource(androidBackground, assembly);
                    break;
                case Device.UWP:
                    //setup background image for UWP
                    string uwpBackground = "ScoreTracker.Assets.Images.Basketballuwp.jpg";
                    imgBackground.Source = ImageSource.FromResource(uwpBackground, assembly);
                    //setup text colors for page elements for UWP
                    gameType.TextColor = Color.White;
                    homeScore.TextColor = Color.White;
                    awayScore.TextColor = Color.White;
                    break;
                default:
                    break;
            }
        }

        //Method to add a 1 point to the home team score
        private void AddHome1Point_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 2, then set to text property
            string score = homeScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 1;
            homeScore.Text = addscore.ToString();
        }

        //Method to add a 2 pointer to the home team score
        private void AddHome2Points_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 2, then set to text property
            string score = homeScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 2;
            homeScore.Text = addscore.ToString();
        }

        //Method to add a 3 pointer to the home team score
        private void AddHome3Points_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 3, then set to text property
            string score = homeScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 3;
            homeScore.Text = addscore.ToString();
        }

        //Method to add a 1 point to the away team score
        private void AddAway1Point_Clicked(object sender, EventArgs e)
        {
            //Change awayscore text to string then convert to an integer - add 2, then set to text property
            string score = awayScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 1;
            awayScore.Text = addscore.ToString();
        }

        //Method to add a 2 pointer to the away team score
        private void AddAway2Points_Clicked(object sender, EventArgs e)
        {
            //Change awayscore text to string then convert to an integer - add 2, then set to text property
            string score = awayScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 2;
            awayScore.Text = addscore.ToString();
        }

        //Method to add a 3 pointer to the home team score
        private void AddAway3Points_Clicked(object sender, EventArgs e)
        {
            //Change awayscore text to string then convert to an integer - add 3, then set to text property
            string score = awayScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 3;
            awayScore.Text = addscore.ToString();
        }

        //Method to save game and ensure requirements are met in order to save
        private async void SaveGame_Clicked(object sender, EventArgs e)
        {
            //If match name is left empty by user
            if (matchName.Text == null || matchName.Text.Trim() == "")
            {
                //Alert user they must enter a match name - referenced - https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/navigation/pop-ups
                await DisplayAlert("Save Requirement", "Match Name cannot be empty", "OK");
            }
            else
            {
                //Boolean to determine if match name already exists
                Boolean matchExists = false;
                //read in all existing matches into existingList
                existingList = MatchClass.ReadList();

                //if no matches exist in existingList
                if (existingList == null)
                {
                    //Save match to file and return to main menu
                    SaveandReturn();
                }
                //If matches are loaded into existingList
                else
                {
                    //games exist, therefore populate list with games for adding and saving later
                    basketballList = MatchClass.ReadList();
                    //Loop through each item in existing list and see if match name exists already
                    foreach (var mc in existingList)
                    {
                        //If match name is found
                        if (mc.MatchName == matchName.Text.Trim())
                        {
                            matchExists = true;
                        }
                    }

                    //If name already exists display alert
                    if (matchExists)
                    {
                        await DisplayAlert("Duplication Error", "Match Name already exists, please enter another", "OK");
                    }
                    //If name doesn't exist already then save match
                    else
                    {
                        //Save match to file and return to main menu
                        SaveandReturn();
                    }
                }               
            }
        }

        //Method used to save match to file, play sound effect and return to main menu
        private async void SaveandReturn()
        {
            //Create new match class and add to basketballList
            MatchClass s = new MatchClass(gameType.Text, homeTeam.Text, homeScore.Text, awayTeam.Text, awayScore.Text, matchName.Text.Trim());
            basketballList.Add(s);
            MatchClass.SaveMatchDataToFile(basketballList);

            //If the sound option is left/turned on via double clicking sound icon
            if (soundOn)
            {
                //Add audio to application when game is saved - referenced from https://forums.xamarin.com/discussion/145050/beep-in-xamarin
                audioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                Stream audioStream = GetType().Assembly.GetManifestResourceStream("ScoreTracker.AudioFiles.BasketballBuzzer.mp3");
                bool isSuccess = audioPlayer.Load(audioStream);
                audioPlayer.Play();
            }

            //Return to Mainpage
            await Navigation.PushAsync(new MainPage());
        }

        //Method to determine if sound should be played and which icon should be displayed - dblclick needed on image to change
        private void ImgSound_Tapped(object sender, EventArgs e)
        {
            //If sound option is on, swap image to mute image
            if (soundOn)
            {
                //Set image source to mute
                var assembly = typeof(Basketball);
                string soundOption = "ScoreTracker.Assets.Images.mutelight.png";
                imgSound.Source = ImageSource.FromResource(soundOption, assembly);
                //Set sound option equal to false
                soundOn = false;
            }
            //If sound option is turned off, swap image back to sound on image
            else
            {
                //Set image source to sound on icon
                var assembly = typeof(Basketball);
                string soundOption = "ScoreTracker.Assets.Images.soundonlight.png";
                imgSound.Source = ImageSource.FromResource(soundOption, assembly);
                //Set sound option equal to true
                soundOn = true;
            }
        }

        //Method to reset scores
        private void ImgReset_Tapped(object sender, EventArgs e)
        {
            homeScore.Text = "0";
            awayScore.Text = "0";
        }
    }
}