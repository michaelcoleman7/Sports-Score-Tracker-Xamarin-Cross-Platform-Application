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

        public Soccer()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void AddHome_Clicked(object sender, EventArgs e)
        {
            string score = homeScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 1;
            homeScore.Text = addscore.ToString();
        }

        private void AddAway_Clicked(object sender, EventArgs e)
        {
            string score = awayScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            addscore = addscore + 1;
            awayScore.Text = addscore.ToString();
        }

        private void SaveGame_Clicked(object sender, EventArgs e)
        {
            MatchClass s = new MatchClass(homeTeam.Text, homeScore.Text, awayTeam.Text, awayScore.Text);
            soccerList.Add(s);
            MatchClass.SaveSoccerDataToFile(soccerList);

            //Add audio to application when game is saved - referenced from https://forums.xamarin.com/discussion/145050/beep-in-xamarin
            audioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            Stream audioStream = GetType().Assembly.GetManifestResourceStream("ScoreTracker.AudioFiles.fulltime.mp3");
            bool isSuccess = audioPlayer.Load(audioStream);
            audioPlayer.Play();
        }
    }
}