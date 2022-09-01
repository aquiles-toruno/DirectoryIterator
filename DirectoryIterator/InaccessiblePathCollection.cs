using DirectoryIterator.Contracts;
using DirectoryIterator.Models;
using System.Collections;

namespace DirectoryIterator
{
    internal class InaccessiblePathCollection : IInaccessiblePathCollection
    {
        private IList _list;

        public InaccessiblePathCollection()
        {
            _list = new List<InaccessiblePath>();
        }

        public InaccessiblePath this[int index] { get => _list[index] as InaccessiblePath; set => _list[index] = value; }

        public int Count => _list.Count;

        public bool IsReadOnly => _list.IsReadOnly;

        public void Add(InaccessiblePath item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(InaccessiblePath item) => _list.Contains(item);

        public void CopyTo(InaccessiblePath[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<InaccessiblePath> GetEnumerator()
        {
            foreach (var item in _list)
            {
                yield return (InaccessiblePath)item;
            }
        }

        public int IndexOf(InaccessiblePath item) => _list.IndexOf(item);

        public void Insert(int index, InaccessiblePath item)
        {
            if (index <= _list.Count && item != null)
            {
                _list.Insert(index, item);
            }
        }

        public bool Remove(InaccessiblePath item)
        {
            _list.Remove(item);

            return true;
        }

        public void RemoveAt(int index)
        {
            if (index <= _list.Count)
            {
                _list.RemoveAt(index);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
