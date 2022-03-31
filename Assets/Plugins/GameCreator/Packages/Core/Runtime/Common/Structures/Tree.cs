using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace GameCreator.Runtime.Common
{ 
	public class Tree<T> : IEnumerable<Tree<T>>
	{
        public readonly string id;
        public T Data { get; }
        
        public Tree<T> Parent { get; private set; }
        public Dictionary<string, Tree<T>> Children { get; }

        // INITIALIZE: ----------------------------------------------------------------------------

        private Tree()
        {
            this.id = string.Empty;
            this.Data = default;

            this.Children = new Dictionary<string, Tree<T>>();
        }

        public Tree(string id, T data)
		{
            this.id = id;
			this.Data = data;

            this.Children = new Dictionary<string, Tree<T>>();
        }

		// PUBLIC METHODS: ------------------------------------------------------------------------

		public Tree<T> AddChild(Tree<T> item)
		{
			item.Parent?.Children.Remove(item.id);
			item.Parent = this;

			if (this.Children.ContainsKey(item.id)) return null;
			
			this.Children.Add(item.id, item);
			return this.Children[item.id];
		}

        public static Tree<T> Create()
        {
            return new Tree<T>();
        }

		// STRING METHODS: ------------------------------------------------------------------------

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			BuildString(sb, this, 0);

			return sb.ToString();
		}

		public static string BuildString(Tree<T> tree)
		{
			StringBuilder sb = new StringBuilder();
			BuildString(sb, tree, 0);

			return sb.ToString();
		}

		private static void BuildString(StringBuilder sb, Tree<T> node, int depth)
		{
			sb.AppendLine(node.id.PadLeft(node.id.Length + depth));

			foreach (Tree<T> child in node)
			{
				BuildString(sb, child, depth + 1);
			}
		}

		// ENUMERATOR METHODS: --------------------------------------------------------------------

		public IEnumerator<Tree<T>> GetEnumerator()
		{
			return this.Children.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}