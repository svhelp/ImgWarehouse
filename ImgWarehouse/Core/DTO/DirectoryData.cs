namespace ImgWarehouse.Core.DTO;

internal class DirectoryData
{
    public string Path { get; set; }

    public List<string> ArchiveEntries { get; set; } = new List<string>();

    public List<ContactListData> ContactLists { get; set; } = new List<ContactListData>();
}
