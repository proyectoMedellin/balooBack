using System.Text.Json.Serialization;

namespace SiecaAPI.OneDrive.Models;

internal class ValueModel
{
    [JsonPropertyName("@microsoft.graph.downloadUrl")]
    public string DownloadUrl { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public string ETag { get; set; }

    public string Id { get; set; }

    public DateTime LastModifiedDateTime { get; set; }

    public string Name { get; set; }

    public string WebUrl { get; set; }

    public string CTag { get; set; }

    public int Size { get; set; }

    public CreatedByModel CreatedBy { get; set; }

    public CreatedByModel LastModifiedBy { get; set; }

    public ParentModel ParentReference { get; set; }

    public FileModel File { get; set; }

    public FileInfoModel FileSystemInfo { get; set; }

    public FolderModel Folder { get; set; }

    public SharedModel Shared { get; set; }
}
