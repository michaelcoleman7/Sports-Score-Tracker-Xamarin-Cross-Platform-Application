using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScoreTracker.Models
{
    class MatchClass
    {
        public string HomeTeam { get; set; }
        public string HomeScore { get; set; }
        public string AwayTeam { get; set; }
        public string AwayScore { get; set; }

        public MatchClass() { }

        public MatchClass(string ht, string hs, string awt, string aws)
        {
            HomeTeam = ht;
            HomeScore = hs;
            AwayTeam = awt;
            AwayScore = aws;
        }

        public static void SaveSoccerDataToFile(List<MatchClass> list)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filename = Path.Combine(path, "SavedGames.txt");

            using (var writer = new StreamWriter(filename, false))
            {
                string jsonText = JsonConvert.SerializeObject(list);
                writer.WriteLine(jsonText);
            }
        }
    }
}
