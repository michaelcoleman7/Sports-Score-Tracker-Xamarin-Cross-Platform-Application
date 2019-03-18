using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace SoccerClassNamespace
{
    class SoccerClass
    {
        public string HomeTeam { get; set; }
        public string HomeScore { get; set; }
        public string AwayTeam { get; set; }
        public string AwayScore { get; set; }

        public SoccerClass() { }

        public SoccerClass(string ht, string hs, string awt, string aws)
        {
            HomeTeam = ht;
            HomeScore = hs;
            AwayTeam = awt;
            AwayScore = aws;
        }

        public static void SaveSoccerDataToFile(List<SoccerClass> list)
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
