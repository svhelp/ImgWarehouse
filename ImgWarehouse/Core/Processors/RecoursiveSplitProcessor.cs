using ImgWarehouse.Models;

namespace ImgWarehouse.Core.Processors;

internal class RecoursiveSplitProcessor : DirectoryProcessor
{
    public RecoursiveSplitProcessor(ProcessorConfig config) : base(config)
    {
    }

    internal override List<DirectoryData> GetDirectoryData(string directoryPath)
    {
        var result = new List<DirectoryData>();

        TraverseDirectories(directoryPath, (currentDirPath) =>
        {
            var currentDirData = GetSingleDirectoryData(currentDirPath, false, false);
            result.Add(currentDirData);
        });

        return result;
    }
}
