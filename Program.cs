using System;
using System.CommandLine;
using System.Runtime.InteropServices;

internal static class Extensions
{
    public static void Each<T>(this IEnumerable<T> ie, Action<T, int> action)
    {
        var i = 0;
        foreach (var e in ie) action(e, i++);
    }
}

class Application
{

    public static Task<int> Main(string[] args)
    {
        var inputFileArgument = new Argument<FileInfo>("input-file", "The file to search");
        var patternArgument = new Argument<string>("pattern", "The pattern to search for");
        var beforeOption = new Option<int>("--before", () => 0, "The number of lines to print before the match");
        beforeOption.AddAlias("-B");
        var afterOption = new Option<int>("--after", () => 0, "The number of lines to print after the match");
        afterOption.AddAlias("-A");

        var rootCommand = new RootCommand("A simple grep-like utility")
        {
            inputFileArgument,
            patternArgument,
            beforeOption,
            afterOption
        };

        rootCommand.SetHandler((System.IO.FileInfo inputFile, string pattern, int before, int after) =>
        {
            Grep(inputFile, pattern, before, after);
        }, inputFileArgument, patternArgument, beforeOption, afterOption);

        return rootCommand.InvokeAsync(args);
    }

    static void Grep(FileInfo inputFile, string pattern, int before, int after)
    {

#if DEBUG
        Console.WriteLine("====== DEBUG INFORMATION ====== ");
        Console.WriteLine(" Input file: " + inputFile);
        Console.WriteLine(" Pattern: " + pattern);
        Console.WriteLine("====== DEBUG INFORMATION ====== ");
        Console.WriteLine("");
#endif
        using (System.IO.StreamReader file = new System.IO.StreamReader(inputFile.ToString()))
        {
            string line;
            int lineNumber = 0;
            int maxBufferSize = before;

            int afterCounter = 0;

            Queue<string> buffer = new Queue<string>();

            while ((line = file.ReadLine()) != null)
            {
                lineNumber++;

                if (line.Contains(pattern))
                {

                    // Print the Buffer of proceeding lines
                    buffer.Each((l, i) =>
                    {
                        Console.WriteLine(l);
                    });
                    // Print the matching line
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{lineNumber}: {line}");
                    Console.ResetColor();

                    // Set the afterCounter so we print the next lines
                    afterCounter = after;
                }
                else
                {

                    // Add the line to the buffer and limit the size of the buffer
                    buffer.Enqueue($"{lineNumber}: {line}");
                    if (buffer.Count > maxBufferSize)
                    {
                        buffer.Dequeue();
                    }
                    // Check if we need to print any lines after the match
                    if (afterCounter > 0)
                    {
                        Console.WriteLine($"{lineNumber}: {line}");
                        afterCounter--;
                        if (afterCounter == 0)
                        {
                            Console.WriteLine();
                        }
                    }
                }
            }
        }
    }
}