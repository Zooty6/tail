using System;
using System.Collections.Generic;
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
            var (tails, location) = CreateTails();

            Parser.Default.ParseArguments<CommandOptions>(args)
                .WithParsed(options =>
                {
                    if (options.Question)
                    {
                        PrintQuestion();
                    }

                    if (options.AddingParameters != null && options.AddingParameters.Any())
                    {
                        AddTail(tails, options.AddingParameters);
                    }

                    if (!string.IsNullOrWhiteSpace(options.User))
                    {
                        SearchUser(tails, options.User);
                    }

                    if (options.List != null && options.List.Any()) Console.Out.WriteLine(tails.GetList(options.List));

                    if (options.ListAll) Console.Out.WriteLine(tails.GetList());
                });

            File.WriteAllText(location, tails.ToString());
        }

        private static (Tails, string) CreateTails()
        {
            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (baseDir == null)
                throw new ArgumentException("Can't find assembly directory location");
            var location = File.ReadAllText(Path.Combine(baseDir, "Tail.ini"));
            var tailString = File.ReadAllLines(location);
            return (new Tails(tailString), location);
        }

        private static void PrintQuestion()
        {
            const string question = "If you could have a tail irl, what kind of tail would you get?";
            Console.Out.WriteLine(question);
            Clipboard.Clipboard.Copy(question);
        }

        private static void AddTail(Tails tails, IEnumerable<string> addingParameters)
        {
            var addingParameterList = addingParameters.ToList();
            for (var i = 0; i < addingParameterList.Count; i += 2)
                if (i + 1 < addingParameterList.Count)
                {
                    tails.AddTail(addingParameterList[i], addingParameterList[i + 1]);
                    Console.Out.WriteLine(
                        $"{addingParameterList[i + 1]} tail has been added to {addingParameterList[i]}, raising the number to {tails.GetNumberOfTails(addingParameterList[i + 1])}!");
                    Clipboard.Clipboard.Copy(
                        $"You are the {PrintNumberWithOrdinal(tails.GetNumberOfTails(addingParameterList[i + 1]))} streamer picking a {addingParameterList[i + 1]} tail");
                }
                else
                {
                    tails.AddTail(addingParameterList[i]);
                    Console.Out.WriteLine($"{addingParameterList[i]} has been added to the boring list!");
                }
        }

        private static string PrintNumberWithOrdinal(int number)
        {
            return (number % 10) switch
            {
                1 => $"{number}st",
                2 => $"{number}nd",
                3 => $"{number}rd",
                _ => $"{number}th"
            };
        }

        private static void SearchUser(Tails tails, string user)
        {
            var tailOfUser = tails.GetTailOfUser(user);
            Console.Out.WriteLine(tailOfUser != null
                ? $"{user} has chosen the tail of a(n) {tailOfUser}."
                : $"{user} has no tail yet.");
        }
    }
}