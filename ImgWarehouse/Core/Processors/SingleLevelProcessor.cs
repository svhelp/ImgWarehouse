using ImgWarehouse.Models;

namespace ImgWarehouse.Core.Processors;

internal class SingleLevelProcessor : DirectoryProcessor
{
    public SingleLevelProcessor(ProcessorConfig config) : base(config)
    {
    }

    internal override List<DirectoryData> GetDirectoryData(string directoryPath)
    {
        return new List<DirectoryData>()
        {
            GetSingleDirectoryData(directoryPath, true, true)
        };
    }
}
