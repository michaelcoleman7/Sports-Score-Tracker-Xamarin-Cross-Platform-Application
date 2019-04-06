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
	public partial class IceHockey : ContentPage
	{
		public IceHockey ()
		{
			InitializeComponent ();
            SetupDefaults();
        }
        //variables used throughout page
        private ISimpleAudioPlayer audioPlayer;
        //bool variable needed for deciding to mute sound or not - on by default
        bool soundOn = true;
        List<MatchClass> hockeyList = new List<MatchClass>();
        List<MatchClass> existingList = new List<MatchClass>();

        //Method to setup default values needed for page setup
        public void SetupDefaults()
        {
            //Turn off navigation bar
            NavigationPage.SetHasNavigationBar(this, false);

            var assembly = typeof(IceHockey);
            //Set default image source for sound icon (default is on)
            string soundOption = "ScoreTracker.Assets.Images.soundondark.png";
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
                    string androidBackground = "ScoreTracker.Assets.Images.icehockeyandroid.jpg";
                    imgBackground.Source = ImageSource.FromResource(androidBackground, assembly);
                    break;
                case Device.UWP:
                    //setup background image for UWP
                    string uwpBackground = "ScoreTracker.Assets.Images.icehockeyuwp.jpg";
                    imgBackground.Source = ImageSource.FromResource(uwpBackground, assembly);
                    break;
                default:
                    break;
            }
        }

        //Method to add a goal to the home score
        private void AddHome_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 1, then set to text property
            string score = homeScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 1;
            homeScore.Text = addscore.ToString();
        }

        //Method to add a goal to the away score
        private void AddAway_Clicked(object sender, EventArgs e)
        {
            //Change awayscore text to string then convert to an integer - add 1, then set to text property
            string score = awayScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 1;
            awayScore.Text = addscore.ToString();
        }

        //Method to save game and ensure requiremnets are met in order to save
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
                    hockeyList = MatchClass.ReadList();

                    //loop through each item in existing list and see if match name exists already
                    foreach (var mc in existingList)
                    {
                        //if match name is found
                        if (mc.MatchName == matchName.Text.Trim())
                        {
                            matchExists = true;
                        }
                    }

                    //if name already exists display alert - referenced - https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/navigation/pop-ups
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
            //create new match class and add to hockeyList
            MatchClass mc = new MatchClass(gameType.Text, homeTeam.Text, homeScore.Text, awayTeam.Text, awayScore.Text, matchName.Text.Trim());
            hockeyList.Add(mc);
            MatchClass.SaveMatchDataToFile(hockeyList);

            //if the sound option is left/turned on via double clicking sound icon
            if (soundOn)
            {
                //Add audio to application when game is saved - referenced from https://forums.xamarin.com/discussion/145050/beep-in-xamarin
                audioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                Stream audioStream = GetType().Assembly.GetManifestResourceStream("ScoreTracker.AudioFiles.HockeyHorn.mp3");
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
                var assembly = typeof(IceHockey);
                string soundOption = "ScoreTracker.Assets.Images.mutedark.png";
                imgSound.Source = ImageSource.FromResource(soundOption, assembly);
                //set sound option equal to false
                soundOn = false;
            }
            //if sound option is turned off, swap image back to sound on image
            else
            {
                //set image source to sound on icon
                var assembly = typeof(IceHockey);
                string soundOption = "ScoreTracker.Assets.Images.soundondark.png";
                imgSound.Source = ImageSource.FromResource(soundOption, assembly);
                //set sound option equal to true
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