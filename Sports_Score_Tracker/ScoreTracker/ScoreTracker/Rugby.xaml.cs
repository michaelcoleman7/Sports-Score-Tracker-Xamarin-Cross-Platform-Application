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
    public partial class Rugby : ContentPage
    {
        public Rugby()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        private ISimpleAudioPlayer audioPlayer;
        List<MatchClass> rugbyList = new List<MatchClass>();

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
            //create new match class and add to rugbyList
            MatchClass mc = new MatchClass(gameType.Text, homeTeam.Text, homeScore.Text, awayTeam.Text, awayScore.Text);
            rugbyList.Add(mc);
            MatchClass.SaveMatchDataToFile(rugbyList);

            //Add audio to application when game is saved - referenced from https://forums.xamarin.com/discussion/145050/beep-in-xamarin
            audioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            Stream audioStream = GetType().Assembly.GetManifestResourceStream("ScoreTracker.AudioFiles.fulltime.mp3");
            bool isSuccess = audioPlayer.Load(audioStream);
            audioPlayer.Play();

            await Navigation.PushAsync(new MainPage());
        }
    }
}