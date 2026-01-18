namespace ImgWarehouse.Core.Helpers;

internal static class PathResolver
{
    public static string ResolveOutputPath(string basePath, string relativeOrAbsolutePath, string extension)
    {
        var relativePath = Path.GetRelativePath(basePath, relativeOrAbsolutePath);
        var fileName = $"{string.Join('_', relativePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))}.{extension}";

        return Path.Combine(basePath, fileName);
    }
}
