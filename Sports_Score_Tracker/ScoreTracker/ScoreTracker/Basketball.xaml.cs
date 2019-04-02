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
	public partial class Basketball : ContentPage
	{
		public Basketball ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        private ISimpleAudioPlayer audioPlayer;
        List<MatchClass> basketballList = new List<MatchClass>();
        List<MatchClass> testList = new List<MatchClass>();

        private void AddHome2Points_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 2, then set to text property
            string score = homeScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 2;
            homeScore.Text = addscore.ToString();
        }
        private void AddHome3Points_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 3, then set to text property
            string score = homeScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 3;
            homeScore.Text = addscore.ToString();
        }

        private void AddAway2Points_Clicked(object sender, EventArgs e)
        {
            //Change awayscore text to string then convert to an integer - add 2, then set to text property
            string score = awayScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 2;
            awayScore.Text = addscore.ToString();
        }
        private void AddAway3Points_Clicked(object sender, EventArgs e)
        {
            //Change awayscore text to string then convert to an integer - add 3, then set to text property
            string score = awayScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 3;
            awayScore.Text = addscore.ToString();
        }

        private async void SaveGame_Clicked(object sender, EventArgs e)
        {
            if (matchName.Text == null)
            {
                await DisplayAlert("Alert", "Match Name cannot be empty", "OK");
            }
            else
            {
                Boolean exists = false;
                testList = MatchClass.ReadList();
                if (testList == null)
                {
                    //create new match class and add to hockeyList
                    MatchClass s = new MatchClass(gameType.Text, homeTeam.Text, homeScore.Text, awayTeam.Text, awayScore.Text, matchName.Text);
                    basketballList.Add(s);
                    MatchClass.SaveMatchDataToFile(basketballList);

                    //Add audio to application when game is saved - referenced from https://forums.xamarin.com/discussion/145050/beep-in-xamarin
                    audioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                    Stream audioStream = GetType().Assembly.GetManifestResourceStream("ScoreTracker.AudioFiles.BasketballBuzzer.mp3");
                    bool isSuccess = audioPlayer.Load(audioStream);
                    audioPlayer.Play();

                    //return to Mainpage
                    await Navigation.PushAsync(new MainPage());
                }
                else
                {
                    foreach (var mc in testList)
                    {
                        if (mc.MatchName == matchName.Text)
                        {
                            exists = true;
                        }
                    }

                    if (exists)
                    {
                        await DisplayAlert("Alert", "Match Name already exists, please enter another", "OK");
                    }
                    else
                    {
                        //create new match class and add to hockeyList
                        MatchClass s = new MatchClass(gameType.Text, homeTeam.Text, homeScore.Text, awayTeam.Text, awayScore.Text, matchName.Text);
                        basketballList.Add(s);
                        MatchClass.SaveMatchDataToFile(basketballList);

                        //Add audio to application when game is saved - referenced from https://forums.xamarin.com/discussion/145050/beep-in-xamarin
                        audioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                        Stream audioStream = GetType().Assembly.GetManifestResourceStream("ScoreTracker.AudioFiles.BasketballBuzzer.mp3");
                        bool isSuccess = audioPlayer.Load(audioStream);
                        audioPlayer.Play();

                        //return to Mainpage
                        await Navigation.PushAsync(new MainPage());
                    }
                }               
            }

        }
    }
}