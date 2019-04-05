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
            setupDefaults();
        }
        private ISimpleAudioPlayer audioPlayer;
        List<MatchClass> rugbyList = new List<MatchClass>();
        List<MatchClass> existingList = new List<MatchClass>();

        public void setupDefaults()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            var assembly = typeof(Rugby);

            // Choose between platform/build options for each device
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    //setup background image
                    string androidBackground = "ScoreTracker.Assets.Images.rugbyandroid.jpg";
                    imgBackground.Source = ImageSource.FromResource(androidBackground, assembly);
                    matchName.TextColor = Color.White;
                    matchName.PlaceholderColor = Color.White;
                    break;
                case Device.UWP:
                    //setup background image
                    string uwpBackground = "ScoreTracker.Assets.Images.Rugbyuwp.jpg";
                    imgBackground.Source = ImageSource.FromResource(uwpBackground, assembly);
                    break;
                default:
                    break;
            }
        }

        //Add a try to the current home score
        private void AddHomeTry_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 5, then set to text property
            string score = homeScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 5;
            homeScore.Text = addscore.ToString();
        }

        //add a conversion to the current home score
        private void AddHomeConversion_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 2, then set to text property
            string score = homeScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 2;
            homeScore.Text = addscore.ToString();
        }

        //add a goal kick to the current home score
        private void AddHomeGoalKick_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 1, then set to text property
            string score = homeScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 3;
            homeScore.Text = addscore.ToString();
        }

        //Add a try to the current away score
        private void AddAwayTry_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 5, then set to text property
            string score = awayScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 5;
            awayScore.Text = addscore.ToString();
        }

        //add a conversion to the current away score
        private void AddAwayConversion_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 2, then set to text property
            string score = awayScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 2;
            awayScore.Text = addscore.ToString();
        }

        //add a goal kick to the current away score
        private void AddAwayGoalKick_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 1, then set to text property
            string score = awayScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 3;
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
                    rugbyList = MatchClass.ReadList();
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
            //create new match class and add to rugbyList
            MatchClass s = new MatchClass(gameType.Text, homeTeam.Text, homeScore.Text, awayTeam.Text, awayScore.Text, matchName.Text.Trim());
            rugbyList.Add(s);
            MatchClass.SaveMatchDataToFile(rugbyList);

            //Add audio to application when game is saved - referenced from https://forums.xamarin.com/discussion/145050/beep-in-xamarin
            audioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            Stream audioStream = GetType().Assembly.GetManifestResourceStream("ScoreTracker.AudioFiles.RugbyWhistle.mp3");
            bool isSuccess = audioPlayer.Load(audioStream);
            audioPlayer.Play();

            //return to Mainpage
            await Navigation.PushAsync(new MainPage());
        }
    }
}