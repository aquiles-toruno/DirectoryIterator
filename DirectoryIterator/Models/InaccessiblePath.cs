namespace DirectoryIterator.Models
{
    public record InaccessiblePath(string Path, Exception Exception)
    {
    }
}
