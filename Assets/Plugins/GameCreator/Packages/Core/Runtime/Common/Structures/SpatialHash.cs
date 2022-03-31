using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    public class SpatialHash
    {
        private const int MAX_SIZE = 256; // Must be POT
        private const int MIN_SIZE = 4;   // Must be POT

        ///////////////////////////////////////////////////////////////////////////////////////////
        // SPATIAL TREE: --------------------------------------------------------------------------

        private abstract class SpatialTree
        {
            protected readonly int size;
            private readonly Dictionary<int, Vector3Int> hashCache;

            protected SpatialTree(int size)
            {
                this.size = size;
                this.hashCache = new Dictionary<int, Vector3Int>();
            }

            protected Vector3Int Hash(ISpatialHash element)
            {
                int uniqueCode = element.UniqueCode;
                if (!this.hashCache.TryGetValue(uniqueCode, out Vector3Int hash))
                {
                    hash = this.Hash(element.Position);
                    this.hashCache.Add(uniqueCode, hash);
                }

                return hash;
            }

            protected Vector3Int Hash(Vector3 point)
            {
                return new Vector3Int(
                    Mathf.FloorToInt(point.x / this.size),
                    Mathf.FloorToInt(point.y / this.size),
                    Mathf.FloorToInt(point.z / this.size)
                );
            }

            protected SpatialTree CreateSubTree(List<ISpatialHash> set, int size)
            {
                int nextSize = size >> 1;
                return nextSize > MIN_SIZE
                    ? (SpatialTree) new SpatialTreeNode(set, nextSize)
                    : (SpatialTree) new SpatialTreeLeaf(set, size);
            }

            public void UpdateHashCache(ISpatialHash element)
            {
                int uniqueCode = element.UniqueCode;
                this.hashCache[uniqueCode] = this.Hash(element.Position);
            }

            public abstract void Insert(Node node);
            public abstract bool Remove(Node node);
            public abstract bool Update(Node node);

            public abstract bool Remove(int uniqueCode, Vector3 point);
            public abstract HashSet<int> Query(Vector3[] bounds);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // SPATIAL TREE NODE: ---------------------------------------------------------------------

        private class SpatialTreeNode : SpatialTree
        {
            private readonly Dictionary<Vector3Int, SpatialTree> tree;

            public SpatialTreeNode(List<ISpatialHash> set, int size) : base(size)
            {
                this.tree = new Dictionary<Vector3Int, SpatialTree>();
                var group = new Dictionary<Vector3Int, List<ISpatialHash>>();

                int setCount = set.Count;
                for (int i = 0; i < setCount; ++i)
                {
                    Vector3Int hash = this.Hash(set[i]);

                    if (!group.TryGetValue(hash, out List<ISpatialHash> list))
                    {
                        list = new List<ISpatialHash>();
                        group.Add(hash, list);
                    }

                    list.Add(set[i]);
                }

                foreach (KeyValuePair<Vector3Int, List<ISpatialHash>> cluster in group)
                {
                    this.tree.Add(
                        cluster.Key,
                        this.CreateSubTree(cluster.Value, this.size)
                    );
                }
            }

            public override void Insert(Node node)
            {
                Vector3Int hash = this.Hash(node.Element);
                if (this.tree.TryGetValue(hash, out SpatialTree subTree))
                {
                    subTree.Insert(node);
                }
                else
                {
                    List<ISpatialHash> set = new List<ISpatialHash> { node.Element };
                    this.tree.Add(hash, this.CreateSubTree(set, this.size));
                }
            }

            public override bool Remove(Node node)
            {
                Vector3Int hash = this.Hash(node.Element);
                if (this.tree.TryGetValue(hash, out SpatialTree subTree))
                {
                    bool isEmpty = subTree.Remove(node);
                    if (isEmpty) this.tree.Remove(hash);
                }

                return this.tree.Count == 0;
            }

            public override bool Remove(int uniqueCode, Vector3 point)
            {
                Vector3Int hash = this.Hash(point);
                if (this.tree.TryGetValue(hash, out SpatialTree subTree))
                {
                    bool isEmpty = subTree.Remove(uniqueCode, point);
                    if (isEmpty) this.tree.Remove(hash);
                }

                return this.tree.Count == 0;
            }

            public override bool Update(Node node)
            {
                this.UpdateHashCache(node.Element);

                Vector3Int prevHash = this.Hash(node.Position);
                Vector3Int nextHash = this.Hash(node.Element);

                if (prevHash == nextHash)
                {
                    if (this.tree.TryGetValue(nextHash, out SpatialTree subTree))
                    {
                        return subTree.Update(node);
                    }

                    return false;
                }

                this.Insert(node);
                this.Remove(node.Element.UniqueCode, node.Position);
                return true;
            }

            public override HashSet<int> Query(Vector3[] bounds)
            {
                Vector3Int cellA = this.Hash(bounds[0]); // (-1,  1,  1)
                Vector3Int cellB = this.Hash(bounds[1]); // ( 1,  1,  1)
                Vector3Int cellD = this.Hash(bounds[2]); // (-1, -1,  1)
                Vector3Int cellG = this.Hash(bounds[6]); // (-1,  1, -1)

                HashSet<int> matches = new HashSet<int>();
                for (int x = cellA.x; x <= cellB.x; ++x)
                {
                    for (int y = cellD.y; y <= cellA.y; ++y)
                    {
                        for (int z = cellD.z; z <= cellG.z; ++z)
                        {
                            Vector3Int hash = new Vector3Int(x, y, z);
                            if (this.tree.TryGetValue(hash, out SpatialTree subTree))
                            {
                                matches.UnionWith(subTree.Query(bounds));
                            }
                        }
                    }
                }

                return matches;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // SPATIAL TREE LEAF: ---------------------------------------------------------------------

        private class SpatialTreeLeaf : SpatialTree
        {
            private readonly HashSet<int> data;

            public SpatialTreeLeaf(List<ISpatialHash> set, int size) : base(size)
            {
                this.data = new HashSet<int>();
                int setCount = set.Count;

                for (int i = 0; i < setCount; ++i)
                {
                    this.data.Add(set[i].UniqueCode);
                }
            }

            public override void Insert(Node node)
            {
                this.data.Add(node.Element.UniqueCode);
            }

            public override bool Remove(Node node)
            {
                this.data.Remove(node.Element.UniqueCode);
                return this.data.Count == 0;
            }

            public override bool Remove(int uniqueCode, Vector3 point)
            {
                this.data.Remove(uniqueCode);
                return this.data.Count == 0;
            }

            public override bool Update(Node node)
            {
                return false;
            }

            public override HashSet<int> Query(Vector3[] bounds)
            {
                return this.data;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // NODE: ----------------------------------------------------------------------------------

        private class Node
        {
            public ISpatialHash Element { get; }
            public Vector3 Position { get; set; }

            public Node(ISpatialHash element)
            {
                this.Element = element;
                this.Position = element.Position;
            }
        }

        // PROPERTIES: ----------------------------------------------------------------------------

        private readonly SpatialTree tree;
        private readonly Dictionary<int, Node> nodes;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public SpatialHash() : this(new List<ISpatialHash>())
        { }

        public SpatialHash(List<ISpatialHash> set)
        {
            this.tree = new SpatialTreeNode(set, MAX_SIZE);

            this.nodes = new Dictionary<int, Node>();
            int setCount = set.Count;
            for (int i = 0; i < setCount; ++i)
            {
                this.nodes.Add(set[i].UniqueCode, new Node(set[i]));
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Insert(ISpatialHash element)
        {
            if (this.nodes.ContainsKey(element.UniqueCode)) return;
            Node node = new Node(element);

            this.nodes.Add(element.UniqueCode, node);
            this.tree.Insert(node);
        }

        public void Remove(ISpatialHash element)
        {
            if (!this.nodes.ContainsKey(element.UniqueCode)) return;

            this.tree.Remove(this.nodes[element.UniqueCode]);
            this.nodes.Remove(element.UniqueCode);
        }

        public bool Update(ISpatialHash element)
        {
            if (this.nodes.TryGetValue(element.UniqueCode, out Node node))
            {
                bool isUpdate = this.tree.Update(node);
                node.Position = element.Position;

                return isUpdate;
            }

            return false;
        }

        public bool Contains(ISpatialHash element)
        {
            return this.nodes.ContainsKey(element.UniqueCode);
        }

        public List<ISpatialHash> Query(Vector3 point, float radius)
        {
            Vector3[] bounds =
            {
                new Vector3(point.x - radius, point.y + radius, point.z - radius),
                new Vector3(point.x + radius, point.y + radius, point.z - radius),
                new Vector3(point.x - radius, point.y - radius, point.z - radius),
                new Vector3(point.x + radius, point.y - radius, point.z - radius),
                new Vector3(point.x - radius, point.y + radius, point.z + radius),
                new Vector3(point.x + radius, point.y + radius, point.z + radius),
                new Vector3(point.x - radius, point.y - radius, point.z + radius),
                new Vector3(point.x + radius, point.y - radius, point.z + radius),
            };

            HashSet<int> matches = this.tree.Query(bounds);
            List<ISpatialHash> elements = new List<ISpatialHash>();

            foreach(int match in matches)
            {
                if (this.nodes.TryGetValue(match, out Node node))
                {
                    elements.Add(node.Element);
                }
            }

            return elements;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    // INTERFACE: ---------------------------------------------------------------------------------

    public interface ISpatialHash
    {
        Vector3 Position { get; }
        int UniqueCode { get; }
    }
}