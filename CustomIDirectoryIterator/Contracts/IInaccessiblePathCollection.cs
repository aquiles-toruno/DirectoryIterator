using CustomDirectoryIterator.Models;
using System.Collections;
using System.Collections.Generic;

namespace CustomDirectoryIterator.Contracts
{
    public interface IInaccessiblePathCollection : IList<InaccessiblePath>, IEnumerable<InaccessiblePath>, IEnumerable
    {
    }
}
