﻿using Plugin.SimpleAudioPlayer;
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
    public partial class Rugby : ContentPage
    {
        public Rugby()
        {
            InitializeComponent();
            SetupDefaults();
        }
        //Variables needed throughout page
        private ISimpleAudioPlayer audioPlayer;
        //bool variable needed for deciding to mute sound or not - on by default
        bool soundOn = true;
        List<MatchClass> rugbyList = new List<MatchClass>();
        List<MatchClass> existingList = new List<MatchClass>();

        //Method to setup default values needed for page setup
        public void SetupDefaults()
        {
            //Turn off navigation bar
            NavigationPage.SetHasNavigationBar(this, false);

            var assembly = typeof(Rugby);
            //Set default image source for sound icon (default is on)
            string soundOption = "ScoreTracker.Assets.Images.soundondark.png";
            ImgSound.Source = ImageSource.FromResource(soundOption, assembly);

            //Set reset image source
            string ResetImage = "ScoreTracker.Assets.Images.reset.png";
            ImgReset.Source = ImageSource.FromResource(ResetImage, assembly);

            // Choose between platform/build options for each device
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    //setup background image for android
                    string androidBackground = "ScoreTracker.Assets.Images.rugbyandroid.jpg";
                    ImgBackground.Source = ImageSource.FromResource(androidBackground, assembly);
                    MatchNamelbl.TextColor = Color.White;
                    MatchNamelbl.PlaceholderColor = Color.White;
                    break;
                case Device.UWP:
                    //setup background image for UWP
                    string uwpBackground = "ScoreTracker.Assets.Images.Rugbyuwp.jpg";
                    ImgBackground.Source = ImageSource.FromResource(uwpBackground, assembly);
                    break;
                default:
                    break;
            }
        }

        //Method to add a try to the current home score
        private void AddHomeTry_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 5, then set to text property
            string score = HomeScorelbl.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 5;
            HomeScorelbl.Text = addscore.ToString();
        }

        //Method to add a conversion to the current home score
        private void AddHomeConversion_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 2, then set to text property
            string score = HomeScorelbl.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 2;
            HomeScorelbl.Text = addscore.ToString();
        }

        //Method to add a goal kick to the current home score
        private void AddHomeGoalKick_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 1, then set to text property
            string score = HomeScorelbl.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 3;
            HomeScorelbl.Text = addscore.ToString();
        }

        //Method to add a try to the current away score
        private void AddAwayTry_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 5, then set to text property
            string score = AwayScorelbl.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 5;
            AwayScorelbl.Text = addscore.ToString();
        }

        //Method to add a conversion to the current away score
        private void AddAwayConversion_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 2, then set to text property
            string score = AwayScorelbl.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 2;
            AwayScorelbl.Text = addscore.ToString();
        }

        //Method to add a goal kick to the current away score
        private void AddAwayGoalKick_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 1, then set to text property
            string score = AwayScorelbl.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 3;
            AwayScorelbl.Text = addscore.ToString();
        }

        //Method to save game and ensure requirements are met in order to save
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
                if (existingList == null)
                {
                    //save match to file and return to main menu
                    SaveandReturn();
                }
                //if matches are loaded into existingList
                else
                {
                    //games exist, therefore populate list with games for adding and saving later
                    rugbyList = MatchClass.ReadList();
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
            //create new match class and add to rugbyList
            MatchClass s = new MatchClass(GameTypelbl.Text, HomeTeamlbl.Text, HomeScorelbl.Text, AwayTeamlbl.Text, AwayScorelbl.Text, MatchNamelbl.Text.Trim());
            rugbyList.Add(s);
            MatchClass.SaveMatchDataToFile(rugbyList);

            //if the sound option is left/turned on via double clicking sound icon
            if (soundOn)
            {
                //Add audio to application when game is saved - referenced from https://forums.xamarin.com/discussion/145050/beep-in-xamarin
                audioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                Stream audioStream = GetType().Assembly.GetManifestResourceStream("ScoreTracker.AudioFiles.RugbyWhistle.mp3");
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
                var assembly = typeof(Rugby);
                string soundOption = "ScoreTracker.Assets.Images.mutedark.png";
                ImgSound.Source = ImageSource.FromResource(soundOption, assembly);
                //set sound option equal to false
                soundOn = false;
            }
            //if sound option is turned off, swap image back to sound on image
            else
            {
                //set image source to sound on icon
                var assembly = typeof(Rugby);
                string soundOption = "ScoreTracker.Assets.Images.soundondark.png";
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