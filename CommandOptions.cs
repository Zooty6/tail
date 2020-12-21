using System.Collections.Generic;
using CommandLine;

namespace Tail
{
    public class CommandOptions
    {
        // [Option('t', "test")]
        // public string TestInput { get; set; }

        [Option('q', "question", 
            HelpText = "Returns the tail question to spam streamers easily", 
            Min = 1, Max = 2, Required = false)]
        public bool Question { get; set; }

        [Option('a', "add", 
            HelpText = "Adds a new streamer and tail choice to the list", 
            Required = false)]
        public IEnumerable<string> AddingParameters { get; set; }
    }
}