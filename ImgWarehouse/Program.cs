using CommandLine;
using ImgWarehouse.Core;
using ImgWarehouse.Core.Logging;
using Microsoft.Extensions.Logging;

class Options
{
    [Option(Default = 210)]
    public int ContactListSize { get; set; }

    [Option(Default = 30)]
    public int ChunkSize { get; set; }

    [Option(Default = false, HelpText = "Takes only one image from each directory recoursively.")]
    public bool OneImagePerLevel { get; set; }

    [Option(
      Default = false,
      HelpText = "Prints all messages to standard output.")]
    public bool Verbose { get; set; }
}

class Program
{
    static ILogger? Logger { get; set; }

    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
          .WithParsed(RunOptions)
          .WithNotParsed(HandleParseError);
    }

    static void RunOptions(Options opts)
    {
        var logLevel = opts.Verbose ? LogLevel.Debug : LogLevel.Information;

        using var lf = LoggerFactory.Create(builder => builder
            .AddConsole(options => options.FormatterName = "noCategoryConsoleFormatter")
            .AddConsoleFormatter<NoCategoryConsoleFormatter, NoCategoryConsoleFormatterOptions>()
            .SetMinimumLevel(logLevel)
        );

        LoggerServiceLocator.Init(lf);

        Logger = LoggerServiceLocator.CreateLogger<Program>();

        Logger.LogDebug("Logger initialized");
        Logger.LogDebug($"ChunkSize: {opts.ChunkSize}");
        Logger.LogDebug($"ContactListSize: {opts.ContactListSize}");
        Logger.LogDebug($"OneImagePerLevel: {opts.OneImagePerLevel}");
        Logger.LogInformation("Processing started");

        new MainProcessor().Process(opts.ContactListSize, opts.ChunkSize, opts.OneImagePerLevel);
    }

    static void HandleParseError(IEnumerable<Error> errs)
    {
        //handle errors
    }
}
