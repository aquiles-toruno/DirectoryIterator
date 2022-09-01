using DirectoryIterator.Models;
using System.Collections;

namespace DirectoryIterator.Contracts
{
    public interface IInaccessiblePathCollection : IList<InaccessiblePath>, IEnumerable<InaccessiblePath>, IEnumerable
    {
    }
}
