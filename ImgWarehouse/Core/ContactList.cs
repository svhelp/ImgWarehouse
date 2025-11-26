using ImageMagick;
using ImgWarehouse.Logging;
using Microsoft.Extensions.Logging;

namespace ImgWarehouse.Core;

internal class ContactList
{
    private static readonly int COLUMNS = 6;
    private static readonly int ROWS = 0;

    private static readonly uint WIDTH = 200;
    private static readonly uint HEIGHT = 200;

    private static readonly int SPACING = 5;

    private ILogger Logger { get; set; } = LoggerServiceLocator.CreateLogger<ContactList>();

    public void Create(List<string> imagePaths, string outputFile)
    {
        Logger.LogDebug("Creating Contact List");

        using (var images = new MagickImageCollection())
        {
            foreach (var path in imagePaths)
            {
                var img = new MagickImage(path)
                {
                    Label = Path.GetFileName(path)
                };
                img.Resize(WIDTH, HEIGHT);
                images.Add(img);
            }

            var settings = new MontageSettings
            {
                Geometry = new MagickGeometry($"{WIDTH}x{HEIGHT}+{SPACING}+{SPACING}"),
                TileGeometry = new MagickGeometry($"{COLUMNS}x{(ROWS > 0 ? ROWS.ToString() : "")}"),
                //Label = true
            };

            using (var montage = images.Montage(settings))
            {
                montage.Write(outputFile);
            }
        }

        Logger.LogInformation("Contact List created");
    }
}
