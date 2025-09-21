using Microsoft.Extensions.Logging;

namespace ImgWarehouse.Logging;

internal static class LoggerServiceLocator
{
    private static ILoggerFactory? _factory;
    public static void Init(ILoggerFactory factory) => _factory = factory;
    public static ILogger CreateLogger<T>() => _factory!.CreateLogger<T>();
}
