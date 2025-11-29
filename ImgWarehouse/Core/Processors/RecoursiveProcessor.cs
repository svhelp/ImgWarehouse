using ImgWarehouse.Models;

namespace ImgWarehouse.Core.Processors;

internal class RecoursiveProcessor : DirectoryProcessor
{
    public RecoursiveProcessor(ProcessorConfig config) : base(config)
    {
    }

    internal override List<DirectoryData> GetDirectoryData(string directoryPath)
    {
        var mergedDirectoryData = new DirectoryData
        {
            Path = directoryPath,
            Images = new List<string>()
        };

        TraverseDirectories(directoryPath, (currentDirPath) =>
        {
            var currentDirData = GetSingleDirectoryData(currentDirPath, false, false);
            mergedDirectoryData.Images.AddRange(currentDirData.Images);
        });

        return new List<DirectoryData>
        {
            mergedDirectoryData,
        };
    }
}
