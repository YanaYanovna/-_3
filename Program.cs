using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;

namespace Dictionary
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    // ++++++++++11+++++++++
    [Serializable]
    public class BinaryTreeNode<TKey, TValue> where TKey : IComparable
    {
        public KeyValuePair<TKey, TValue> KVP;
        public BinaryTreeNode<TKey, TValue> Left;
        public BinaryTreeNode<TKey, TValue> Right;
        public BinaryTreeNode<TKey, TValue> Parent;
    }

    // +++++++++12+++++++++
    [Serializable]
    public class BinaryTree<TKey, TValue> where TKey : IComparable
    {
        private BinaryTreeNode<TKey, TValue> _root;

        public (TValue, bool) Find(TKey element)
        {
            var current = _root;

            while (current != null)
            {
                var compareResult = element.CompareTo(current.KVP.Key);
                if (compareResult < 0)
                {
                    current = current.Left;
                }
                else if (compareResult > 0)
                {
                    current = current.Right;
                }
                else
                {
                    return (current.KVP.Value, true);
                }
            }

            return (default, false);
        }

        //public bool FindForContainsKey(TKey element)
        //{
        //    var current = _root;

        //    while (current != null)
        //    {
        //        var compareResult = element.CompareTo(current.KVP.Key);
        //        if (compareResult < 0)
        //        {
        //            current = current.Left;
        //        }
        //        else if (compareResult > 0)
        //        {
        //            current = current.Right;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        // +++++++++13+++++++++
        public bool Insert(KeyValuePair<TKey, TValue> element)
        {
            if (_root == null)
            {
                _root = new BinaryTreeNode<TKey, TValue>()
                {
                    KVP = element
                };
                return true;
            };

            var current = _root;

            while (current != null)
            {
                var compareResult = element.Key.CompareTo(current.KVP.Key);
                if (compareResult < 0)
                {
                    if (current.Left == null)
                    {
                        current.Left = new BinaryTreeNode<TKey, TValue>()
                        {
                            KVP = element,
                            Parent = current,
                        };
                        return true;
                    }

                    current = current.Left;
                }
                else if (compareResult > 0)
                {

                    if (current.Right == null)
                    {
                        current.Right = new BinaryTreeNode<TKey, TValue>()
                        {
                            KVP = element,
                            Parent = current,
                        };
                        return true;
                    }

                    current = current.Right;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        // ++++++++++14++++++++++
        public bool Remove(TKey element)
        {
            var current = _root;

            while (current != null)
            {
                var compareResult = element.CompareTo(current.KVP.Key);
                if (compareResult < 0)
                {
                    current = current.Left;
                }
                else if (compareResult > 0)
                {
                    current = current.Right;
                }
                else
                {
                    if (current.Left == null && current.Right == null)
                    {
                        if (current.Parent == null)
                        {
                            _root = null;
                        }
                        else if (current == current.Parent.Left)
                        {
                            current.Parent.Left = null;
                        }
                        else
                        {
                            current.Parent.Right = null;
                        }
                    }
                    else if (current.Left == null)
                    {
                        current.Right.Parent = current.Parent;
                        if (current.Parent == null)
                        {
                            _root = current.Right;
                        }
                        else if (current == current.Parent.Left)
                        {
                            current.Parent.Left = current.Right;
                        }
                        else
                        {
                            current.Parent.Right = current.Right;
                        }
                    }
                    else if (current.Right == null)
                    {
                        current.Left.Parent = current.Parent;
                        if (current.Parent == null)
                        {
                            _root = current.Left;
                        }
                        else if (current == current.Parent.Left)
                        {
                            current.Parent.Left = current.Left;
                        }
                        else
                        {
                            current.Parent.Right = current.Left;
                        }
                    }
                    else
                    {
                        if (current.Parent == null)
                        {
                            _root = current.Right;
                        }
                        else if (current == current.Parent.Left)
                        {
                            current.Parent.Left = current.Right;
                        }
                        else
                        {
                            current.Parent.Right = current.Right;
                        }

                        var newParentRight = current.Right;
                        while (newParentRight.Left != null)
                            newParentRight = newParentRight.Left;

                        newParentRight.Left = current.Left;
                        current.Left.Parent = newParentRight;
                    }

                    return true;
                }
            }

            return false;
        }

        // ++++++++++15++++++++++
        public IEnumerable<KeyValuePair<TKey, TValue>> Traverse()
        {
            return TraverseNode(_root);
        }

        private IEnumerable<KeyValuePair<TKey, TValue>> TraverseNode(BinaryTreeNode<TKey, TValue> node)
        {
            if (node != null)
            {
                foreach (var element in TraverseNode(node.Left))
                    yield return element;

                yield return node.KVP;

                foreach (var element in TraverseNode(node.Right))
                    yield return element;
            }
        }
    }

    // ----------2----------
    [Serializable]
    public class BinaryTreeDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable
    {
        private BinaryTree<TKey, TValue> _tree = new BinaryTree<TKey, TValue>();

        //Свойства

        //Count++++++++++
        public int Count => _tree.Traverse().Count();

        //IsReadOnly++++++++++
        public bool IsReadOnly => false;

        //Item[TKey]----------
        public TValue this[TKey key]
        {
            get => _tree.Find(key).Item1;
            set => Add(key, value);
        }

        //Keys++++++++++
        public ICollection<TKey> Keys => _tree.Traverse().Select(keyValuePair => keyValuePair.Key).ToList();
        //Values++++++++++
        public ICollection<TValue> Values => _tree.Traverse().Select(keyValuePair => keyValuePair.Value).ToList();

        //Методы

        //Add(T)++++++++++
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _tree.Insert(item);
        }

        //Add(TKey, TValue)++++++++++
        public void Add(TKey key, TValue value)
        {
            _tree.Insert(new KeyValuePair<TKey, TValue>(key, value));
        }

        //Clear()++++++++++
        public void Clear()
        {
            _tree = new BinaryTree<TKey, TValue>();
        }

        ////Contains(T)++++++++++
        //public bool Contains(KeyValuePair<TKey, TValue> item)
        //{
        //    return _tree.Traverse().ToList().Contains(item);
        //}

        ////ContainsKey(TKey)++++++++++
        //public bool ContainsKey(TKey key)
        //{
        //    return _tree.Traverse().Select(keyValuePair => keyValuePair.Key).Contains(key);
        //}

        //Contains(T)++++++++++
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _tree.Find(item.Key).Item1.Equals(item.Value);
        }

        ////ContainsKey(TKey)++++++++++
        //public bool ContainsKey(TKey key)
        //{
        //    return _tree.FindForContainsKey(key);
        //}

        //ContainsKey(TKey)++++++++++
        public bool ContainsKey(TKey key)
        {
            return _tree.Find(key).Item2;
        }

        //CopyTo(T[], Int32)++++++++++
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (var item in _tree.Traverse())
            {
                array[arrayIndex++] = item;
            }
        }

        //GetEnumerator()++++++++++
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _tree.Traverse().GetEnumerator();
        }

        //Remove(TKey)++++++++++
        public bool Remove(TKey key)
        {
            return _tree.Remove(key);
        }

        //TryGetValue(TKey key, out TValue value)++++++++++
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = _tree.Find(key).Item1;
            return _tree.Find(key).Item2;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _tree.Remove(item.Key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tree.Traverse().GetEnumerator();
        }

    }

    // ++++++++++
    public interface IFileService
    {
        Stream GetReadStream(string name);
        Stream GetWriteStream(string name);
    }

    public class FileService : IFileService
    {
        public Stream GetReadStream(string name)
        {
            return new FileStream(name, FileMode.Open);
        }

        public Stream GetWriteStream(string name)
        {
            return new FileStream(name, FileMode.OpenOrCreate);
        }
    }

    public class BinaryTreeDictSerializer
    {
        private IFileService fileService;

        public BinaryTreeDictSerializer(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public void Serialize<TKey, TValue>(BinaryTreeDictionary<TKey, TValue> dict, string filename, bool closeStream = true) where TKey : IComparable
        {
            var formatter = new DataContractJsonSerializer(typeof(BinaryTreeDictionary<TKey, TValue>));

            Stream fs = fileService.GetWriteStream(filename);
            formatter.WriteObject(fs, dict);

            if (closeStream)
                fs.Close();
        }

        public BinaryTreeDictionary<TKey, TValue> Deserialize<TKey, TValue>(string filename, bool closeStream = true) where TKey : IComparable
        {
            var formatter = new DataContractJsonSerializer(typeof(BinaryTreeDictionary<TKey, TValue>));
            Stream fs = fileService.GetReadStream(filename);

            var result = (BinaryTreeDictionary<TKey, TValue>)formatter.ReadObject(fs);

            if (closeStream)
                fs.Close();

            return result;
        }
    }
}
