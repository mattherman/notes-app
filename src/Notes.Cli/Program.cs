using System.CommandLine;

var rootCommand = new RootCommand("Notes CLI tool");

var convertCommand = new Command("convert", "Convert notes into a different format.");

var fileArgument = new Argument<FileInfo>(
    name: "file",
    description: "The file to convert");

var formatOption = new Option<DestinationFormat>(
    name: "--format",
    description: "The format to convert the file to. Supports \"html\", \"json\", and \"markdown\"")
    { IsRequired = true };
formatOption.AddAlias("-f");

var outputOption = new Option<FileInfo?>(
    name: "--output",
    description: "The output file");
outputOption.AddAlias("-o");

convertCommand.AddArgument(fileArgument);
convertCommand.AddOption(formatOption);
convertCommand.AddOption(outputOption);

convertCommand.SetHandler((sourceFile, format, destinationFile) =>
{
    Console.WriteLine($"Converting file to {format} format and outputing to {destinationFile}...");
    ReadFile(sourceFile, destinationFile);
}, fileArgument, formatOption, outputOption);

rootCommand.AddCommand(convertCommand);

return await rootCommand.InvokeAsync(args);

static void ReadFile(FileInfo source, FileInfo? destination)
{
    var lines = File.ReadLines(source.FullName).ToList();
    if (destination is not null)
    {
        File.WriteAllLines(destination.FullName, lines);
    }
    else
    {
        lines.ForEach(line => Console.WriteLine(line));
    }
}

enum DestinationFormat
{
    Html = 0,
    Json = 1,
    Markdown = 2
}
