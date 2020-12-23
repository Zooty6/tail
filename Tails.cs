using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tail
{
    public class Tails
    {
        private const string TableRowPattern = @"^\|\s*(?<streamer>\w+)\s*\|\s*(?<tail>.+)\s*\|$";

        private const string Template = @"# Tails  
| twitch name      | tail     |
|:-----------------|:---------|
@@tails

# Summary
| animal        | count |
|:--------------|------:|
@@summary
";

        private readonly Dictionary<string, string> streamerTails = new Dictionary<string, string>();
        private readonly Dictionary<string, int> summary = new Dictionary<string, int>();
        private readonly string[] tailInitContent;

        public Tails(string[] tailInitContent)
        {
            this.tailInitContent = tailInitContent;
            var regex = new Regex(TableRowPattern);
            var inTable = false;
            for (var i = 0; i < tailInitContent.Length; i++)
            {
                var tailLine = tailInitContent[i];
                if (!inTable)
                {
                    if (tailLine.Trim() == "# Tails")
                    {
                        i += 2;
                        inTable = true;
                    }
                }
                else
                {
                    var match = regex.Match(tailLine);
                    if (!match.Success) break;

                    var streamer = match.Groups["streamer"].Value.Trim();
                    var tail = match.Groups["tail"].Value.Trim();

                    AddTail(streamer, tail);
                }
            }
        }

        public void AddTail(string streamer, string tail = "-")
        {
            if (tail.Trim() != "-" && !streamerTails.ContainsKey(streamer))
            {
                if (summary.ContainsKey(tail))
                    summary[tail]++;
                else
                    summary[tail] = 1;
            }

            streamerTails[streamer] = tail;
        }

        public override string ToString()
        {
            var streamerTailTable = "";
            var summaryTable = "";

            var streamerKeys = streamerTails.Keys.ToList();
            streamerKeys.Sort();

            foreach (var key in streamerKeys)
                streamerTailTable += $"| {key} | {streamerTails[key]} |{Environment.NewLine}";

            var summaryList = summary.ToList();
            summaryList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            foreach (var (key, value) in summaryList) summaryTable += $"| {key} | {value} |{Environment.NewLine}";

            return Template.Replace("@@tails", streamerTailTable).Replace("@@summary", summaryTable);
        }


        public string GetTailOfUser(string streamer)
        {
            return streamerTails.ContainsKey(streamer) ? streamerTails[streamer] : null;
        }
    }
}