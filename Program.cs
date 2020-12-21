using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLine;

namespace Tail
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var location = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Tail.ini"));
            var tailString = File.ReadAllLines(location);
            var tails = new Tails(tailString);
            
            Parser.Default.ParseArguments<CommandOptions>(args)
                .WithParsed(options =>
                {
                    if (options.Question)
                    {
                        Console.Out.WriteLine("If you could have a tail irl, what kind of tail would you get?");
                    }
            
                    if (options.AddingParameters != null)
                    {
                        var addingParameterList = options.AddingParameters.ToList();
                        for (var i = 0; i < addingParameterList.Count; i+=2)
                        {
                            if (i + 1 < addingParameterList.Count)
                            {
                                tails.AddTail(addingParameterList[i], addingParameterList[i+1]);
                            }
                            else
                            {
                                tails.AddTail(addingParameterList[i]);
                            }
                        }
                    }
                });
            
            File.WriteAllText(location, tails.ToString());
        }
    }
}