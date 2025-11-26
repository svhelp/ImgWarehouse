using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace ImgWarehouse.Logging;

internal sealed class NoCategoryConsoleFormatter : ConsoleFormatter, IDisposable
{
    private readonly IDisposable? _optionsReloadToken;
    private NoCategoryConsoleFormatterOptions _formatterOptions;
    private bool ConsoleColorFormattingEnabled =>
        _formatterOptions.ColorBehavior == LoggerColorBehavior.Enabled ||
        _formatterOptions.ColorBehavior == LoggerColorBehavior.Default &&
        Console.IsOutputRedirected == false;

    public NoCategoryConsoleFormatter(IOptionsMonitor<NoCategoryConsoleFormatterOptions> options)
        : base("noCategoryConsoleFormatter") =>
        (_optionsReloadToken, _formatterOptions) =
            (options.OnChange(ReloadLoggerOptions), options.CurrentValue);

    private void ReloadLoggerOptions(NoCategoryConsoleFormatterOptions options) =>
        _formatterOptions = options;

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter)
    {
        string? message =
            logEntry.Formatter?.Invoke(
                logEntry.State, logEntry.Exception);

        if (message is null)
        {
            return;
        }

        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        string logContent = $"{logEntry.LogLevel}: {message}";

        textWriter.Write($"[{timestamp}] ");

        if (ConsoleColorFormattingEnabled)
        {
            var color = logEntry.LogLevel switch
            {
                LogLevel.Trace => ConsoleColor.Gray,
                LogLevel.Debug => ConsoleColor.Gray,
                LogLevel.Information => ConsoleColor.Green,
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Critical => ConsoleColor.Red,
                _ => Console.ForegroundColor
            };

            textWriter.WriteWithColor(
                logContent,
                ConsoleColor.Black,
                color);
        } else {
            textWriter.WriteLine(logContent);
        }
    }

    public void Dispose() => _optionsReloadToken?.Dispose();
}
