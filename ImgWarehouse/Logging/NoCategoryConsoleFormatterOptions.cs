using Microsoft.Extensions.Logging.Console;

namespace ImgWarehouse.Logging;

internal sealed class NoCategoryConsoleFormatterOptions : SimpleConsoleFormatterOptions
{
    public bool Verbose { get; set; } = false;
}
