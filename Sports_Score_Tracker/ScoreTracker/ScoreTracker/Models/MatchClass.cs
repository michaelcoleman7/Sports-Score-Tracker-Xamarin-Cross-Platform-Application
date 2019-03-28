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
            StringBuilder sb = new StringBuilder();

            if (File.Exists(filename))
            {
                using (var reader = new StreamReader(filename, false))
                {
                    sb.Append(reader.ReadToEnd());
                    sb.Append(',');
                }
                using (var writer = new StreamWriter(filename, false))
                {
                    string jsonText = JsonConvert.SerializeObject(list, Formatting.Indented);
                    //writer.WriteLine(jsonText);


                    sb.Append(jsonText);
                    sb.Replace('[', ' ');
                    sb.Replace(']', ' ');

                    sb.Insert(0, "[");
                    sb.Append(']');
                    writer.WriteLine(sb);
                }
            }
            else
            {
                using (var writer = new StreamWriter(filename, false))
                {
                    string jsonText = JsonConvert.SerializeObject(list, Formatting.Indented);
                    //writer.WriteLine(jsonText);

                    writer.WriteLine(jsonText);
                }

            }
        }
    }
}
