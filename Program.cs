﻿using System;
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
            var location =
                File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Tail.ini"));
            var tailString = File.ReadAllLines(location);
            var tails = new Tails(tailString);

            Parser.Default.ParseArguments<CommandOptions>(args)
                .WithParsed(options =>
                {
                    if (options.Question)
                    {
                        const string question = "If you could have a tail irl, what kind of tail would you get?";
                        Console.Out.WriteLine(question);
                        Clipboard.Clipboard.Copy(question);
                    }

                    if (options.AddingParameters != null)
                    {
                        var addingParameterList = options.AddingParameters.ToList();
                        for (var i = 0; i < addingParameterList.Count; i += 2)
                            if (i + 1 < addingParameterList.Count)
                            {
                                tails.AddTail(addingParameterList[i], addingParameterList[i + 1]);
                                Console.Out.WriteLine(
                                    $"{addingParameterList[i + 1]} tail has been added to {addingParameterList[i]}, raising the number to {tails.GetNumberOfTails(addingParameterList[i + 1])}!");
                            }
                            else
                            {
                                tails.AddTail(addingParameterList[i]);
                                Console.Out.WriteLine($"{addingParameterList[i]} has been added to the boring list!");
                            }
                    }

                    if (!string.IsNullOrWhiteSpace(options.User))
                    {
                        var tailOfUser = tails.GetTailOfUser(options.User);
                        Console.Out.WriteLine(tailOfUser != null
                            ? $"{options.User} has chosen the tail of a(n) {tailOfUser}."
                            : $"{options.User} has no tail yet.");
                    }

                    if (options.List != null) Console.Out.WriteLine(tails.GetList(options.List));

                    if (options.ListAll) Console.Out.WriteLine(tails.GetList());
                });

            File.WriteAllText(location, tails.ToString());
        }
    }
}