using ImgWarehouse.Core.DTO;
using ImgWarehouse.Models;

namespace ImgWarehouse.Core.Processors;

internal class SingleLevelProcessor : DirectoryProcessor
{
    public SingleLevelProcessor(ProcessorConfig config) : base(config)
    {
    }

    internal override DirectoryData GetDirectoryData(string directoryPath)
    {
        var result = new DirectoryData
        {
            Path = directoryPath,
        };

        var contactList = GetSingleDirectoryData(directoryPath, true, true);

        if (contactList.Images.Any())
        {
            result.ArchiveEntries.Add(directoryPath);
            result.ContactLists.Add(contactList);
        }

        return result;
    }
}
