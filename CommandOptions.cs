using System.Collections.Generic;
using CommandLine;

namespace Tail
{
    public class CommandOptions
    {
        [Option('q', "question",
            HelpText = "Returns the tail question to spam streamers easily.",
            Min = 1, Max = 2, Required = false)]
        public bool Question { get; set; }

        [Option('a', "add",
            HelpText = "Adds a new streamer and tail choice to the list.",
            Required = false)]
        public IEnumerable<string> AddingParameters { get; set; }

        [Option('s', "streamer", HelpText = "Returns what tail the given streamer chose.")]
        public string User { get; set; }

        [Option('c', "count",
            HelpText = "Returns the number of time this tail has been chosen.",
            Required = false)]
        public IEnumerable<string> List { get; set; }

        [Option('l', "list", HelpText = "Returns all the tail choices and their numbers")]
        public bool ListAll { get; set; }
    }
}