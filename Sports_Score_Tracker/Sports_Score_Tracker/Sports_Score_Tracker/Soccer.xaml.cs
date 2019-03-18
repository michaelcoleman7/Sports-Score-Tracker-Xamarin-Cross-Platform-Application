using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sports_Score_Tracker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Soccer : ContentPage
	{
		public Soccer ()
		{
			InitializeComponent ();
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
    }
}