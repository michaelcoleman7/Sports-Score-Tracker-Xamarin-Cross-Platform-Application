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
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private ISimpleAudioPlayer audioPlayer;
        List<MatchClass> tennisList = new List<MatchClass>();

        private void AddHome_Clicked(object sender, EventArgs e)
        {
            //Change homescore text to string then convert to an integer - add 1, then set to text property
            string score = homeScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            if (addscore == 0 || addscore == 15)
            {
                addscore = addscore + 15;
            }
            else if (addscore == 30)
            {
                addscore = addscore + 10;
            }
            else
            {
                DisplayAlert("Alert", "Scoring cannot go above 40", "OK");
            }
            homeScore.Text = addscore.ToString();
        }

        private void AddAway_Clicked(object sender, EventArgs e)
        {
            //Change awayscore text to string then convert to an integer - add 1, then set to text property
            string score = awayScore.Text.ToString();
            int addscore = Convert.ToInt32(score);
            if (addscore == 0 || addscore == 15)
            {
                addscore = addscore + 15;
            }
            else if (addscore == 30)
            {
                addscore = addscore + 10;
            }
            else
            {
                DisplayAlert("Alert", "Scoring cannot go above 40", "OK");
            }
            awayScore.Text = addscore.ToString();
        }

        private async void SaveGame_Clicked(object sender, EventArgs e)
        {
            //create new match class and add to hockeyList
            MatchClass mc = new MatchClass(gameType.Text, homeTeam.Text, homeScore.Text, awayTeam.Text, awayScore.Text);
            tennisList.Add(mc);
            MatchClass.SaveMatchDataToFile(tennisList);

            //Add audio to application when game is saved - referenced from https://forums.xamarin.com/discussion/145050/beep-in-xamarin
            audioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            Stream audioStream = GetType().Assembly.GetManifestResourceStream("ScoreTracker.AudioFiles.HockeyHorn.mp3");
            bool isSuccess = audioPlayer.Load(audioStream);
            audioPlayer.Play();

            await Navigation.PushAsync(new MainPage());
        }
    }
}