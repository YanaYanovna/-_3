using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;

namespace DictionaryTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAddTThenContainsT()
        {
            var tree = new BinaryTreeDictionary<int, int>();
            tree.Add(0, 0);
            
            Assert.IsTrue(tree.Contains(new KeyValuePair<int, int>(0, 0)));
        }

        [TestMethod]
        public void TestAddTAnotherThenDontContainsT()
        {
            var tree = new BinaryTreeDictionary<int, int>();
            tree.Add(0, 0);

            Assert.IsFalse(tree.Contains(new KeyValuePair<int, int>(2, 2)));
        }

        [TestMethod]
        public void TestAddTKeyTValueMultipleThenCount()
        {
            var tree = new BinaryTreeDictionary<int, int>();
            tree.Add(0, 0);
            tree.Add(1, 1);
            tree.Add(2, 2);
            tree.Add(3, 3);
            tree.Add(4, 4);
            tree.Add(5, 5);

            Assert.AreEqual(6, tree.Count);
        }

        [TestMethod]
        public void TestAddTKeyTValueMultipleThenCountWrong()
        {
            var tree = new BinaryTreeDictionary<int, int>();
            tree.Add(0, 0);
            tree.Add(1, 1);
            tree.Add(2, 2);
            tree.Add(3, 3);
            tree.Add(4, 4);
            tree.Add(5, 5);

            Assert.IsFalse(tree.Count==2);
        }

        [TestMethod]
        public void TestAddTKeyTValueThenClear()
        {
            var tree = new BinaryTreeDictionary<int, int>();
            tree.Add(0, 0);
            tree.Clear();

            Assert.AreEqual(0, tree.Count);
        }

        [TestMethod]
        public void TestAddTKeyTValueMultipleThenRemoveTKey()
        {
            var tree = new BinaryTreeDictionary<int, int>();
            tree.Add(0, 0);
            tree.Add(1, 1);
            tree.Remove(0);

            Assert.AreEqual(1, tree.Count);
        }

        [TestMethod]
        public void TestAddTThenContainsKey()
        {
            var tree = new BinaryTreeDictionary<int, int>();
            tree.Add(new KeyValuePair<int, int>(0, 0));

            Assert.IsTrue(tree.ContainsKey(0));
        }

        [TestMethod]
        public void TestAddTThenTryGetValue()
        {
            var tree = new BinaryTreeDictionary<int, int>();
            tree.Add(new KeyValuePair<int, int>(0, 0));
            int value = 5;

            Assert.IsTrue(tree.TryGetValue(0, out value));
            Assert.AreEqual(0, value);
        }

        [TestMethod]
        public void TestAddTKeyTValueMultipleThenRemoveTKeyTValue()
        {
            var tree = new BinaryTreeDictionary<int, int>();
            tree.Add(0, 0);
            tree.Add(1, 1);
            tree.Add(2, 2);
            tree.Remove(new KeyValuePair<int, int>(2, 2));

            Assert.AreEqual(2, tree.Count);
        }

        [TestClass]
        public class BinaryTreeSetSerializerTest
        {
            public class MemoryStreamService : IFileService
            {
                public MemoryStream stream = new MemoryStream();

                public Stream GetReadStream(string name)
                {
                    stream.Position = 0;
                    return stream;
                }

                public Stream GetWriteStream(string name)
                {
                    return stream;
                }
            }

            [TestMethod]
            public void TestSerializeAndDeserialize()
            {
                var tree = new BinaryTreeDictionary<int, int>();
                tree.Add(0, 0);
                tree.Add(1, 1);
                tree.Add(2, 2);

                var serializer = new BinaryTreeDictSerializer(new MemoryStreamService());
                serializer.Serialize(tree, "123", closeStream: false);
                var newTree = serializer.Deserialize<int, int>("123", closeStream: false);

                CollectionAssert.AreEqual(tree.ToList(), newTree.ToList());
            }
        }

    }
}
