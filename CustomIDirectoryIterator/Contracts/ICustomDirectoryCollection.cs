using System.Collections;
using System.Collections.Generic;

namespace CustomDirectoryIterator.Contracts
{
    public interface ICustomDirectoryCollection : IEnumerable<string>, IEnumerable
    {
        IInaccessiblePathCollection InaccessiblePathCollection { get; }
    }
}
