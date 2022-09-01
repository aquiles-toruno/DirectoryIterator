using System.Collections;

namespace DirectoryIterator.Contracts
{
    public interface ICustomDirectoryCollection : IEnumerable<string>, IEnumerable
    {
        IInaccessiblePathCollection InaccessiblePathCollection { get; }
    }
}
