using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SoccerClassNamespace;
using Plugin.SimpleAudioPlayer;
using System.IO;

namespace Sports_Score_Tracker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Soccer : ContentPage
	{
        private ISimpleAudioPlayer _simpleAudioPlayer;
        List<SoccerClass> soccerList = new List<SoccerClass>();

        public Soccer ()
		{
			InitializeComponent ();
            _simpleAudioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            Stream beepStream = GetType().Assembly.GetManifestResourceStream("Sports_Score_Tracker.AudioFiles.fulltime.mp3");
            bool isSuccess = _simpleAudioPlayer.Load(beepStream);
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
            SoccerClass s = new SoccerClass(homeTeam.Text, homeScore.Text, awayTeam.Text, awayScore.Text);
            soccerList.Add(s);
            SoccerClass.SaveSoccerDataToFile(soccerList);
            _simpleAudioPlayer.Play();
        }
    }
}