using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScoreTracker.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace ScoreTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Soccer : ContentPage
    {
        private ISimpleAudioPlayer audioPlayer;
        List<MatchClass> soccerList = new List<MatchClass>();
        List<MatchClass> existingList = new List<MatchClass>();

        public Soccer()
        {
            InitializeComponent();
            setupDefaults();
        }
        bool soundOn = true;

        public void setupDefaults()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            var assembly = typeof(Soccer);
            string soundOption = "ScoreTracker.Assets.Images.soundondark.png";
            imgSound.Source = ImageSource.FromResource(soundOption, assembly);


            // Choose between platform/build options for each device
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    //setup background image
                    string androidBackground = "ScoreTracker.Assets.Images.soccerpitch.jpg";
                    imgBackground.Source = ImageSource.FromResource(androidBackground, assembly);

                    //setup text colors for page elements
                    gameType.TextColor = Color.White;
                    homeTeam.TextColor = Color.White;
                    awayTeam.TextColor = Color.White;
                    dash.TextColor = Color.White;
                    matchName.TextColor = Color.White;
                    matchName.PlaceholderColor = Color.White;
                    break;
                case Device.UWP:
                    //setup background image
                    string uwpBackground = "ScoreTracker.Assets.Images.socceruwp.jpg";
                    imgBackground.Source = ImageSource.FromResource(uwpBackground, assembly);
                    break;
                default:
                    break;
            }
        }

        private void AddHome_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 1, then set to text property
            string score = homeScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 1;
            homeScore.Text = addscore.ToString();
        }

        private void AddAway_Clicked(object sender, EventArgs e)
        {
            //Change awayscore text to string then convert to an integer - add 1, then set to text property
            string score = awayScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 1;
            awayScore.Text = addscore.ToString();
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
                    soccerList = MatchClass.ReadList();

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
            //create new match class and add to soccerList
            MatchClass s = new MatchClass(gameType.Text, homeTeam.Text, homeScore.Text, awayTeam.Text, awayScore.Text, matchName.Text.Trim());
            soccerList.Add(s);
            MatchClass.SaveMatchDataToFile(soccerList);

            //if the sound option is left/turned on via double clicking sound icon
            if (soundOn)
            {
                //Add audio to application when game is saved - referenced from https://forums.xamarin.com/discussion/145050/beep-in-xamarin
                audioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                Stream audioStream = GetType().Assembly.GetManifestResourceStream("ScoreTracker.AudioFiles.fulltime.mp3");
                bool isSuccess = audioPlayer.Load(audioStream);
                audioPlayer.Play();
            }


            //return to Mainpage
            await Navigation.PushAsync(new MainPage());
        }

        private void imgSound_Tapped(object sender, EventArgs e)
        {
            //if sound option is on, swap image to mute image
            if (soundOn)
            {
                //set image source to mute
                var assembly = typeof(Soccer);
                string soundOption = "ScoreTracker.Assets.Images.mutedark.png";
                imgSound.Source = ImageSource.FromResource(soundOption, assembly);
                //set sound option equal to false
                soundOn = false;
            }
            //if sound option is turned off, swap image back to sound on image
            else
            {
                //set image source to sound on icon
                var assembly = typeof(Soccer);
                string soundOption = "ScoreTracker.Assets.Images.soundondark.png";
                imgSound.Source = ImageSource.FromResource(soundOption, assembly);
                //set sound option equal to true
                soundOn = true;
            }
        }
    }
}