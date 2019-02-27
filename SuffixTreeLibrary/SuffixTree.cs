using System.Collections.Generic;
using System.Linq;

namespace SuffixTreeLibary
{
    public class SuffixTree
    {
        private readonly string text;

        private readonly Node rootNode = new Node(string.Empty);

        public SuffixTree(string text)
        {
            this.text = text + "$";

			BuildNLogN();
        }

        public bool Contains(string substring)
        {
            bool Contains(string s, Node node)
            {
                if (!node.FindChild(s[0], out var child))
                    return false;

                if (s.Length == 1)
                    return true;

                return Contains(s.Substring(1), child);
            }

            return Contains(substring, rootNode);
        }

        private void BuildNLogN()
        {
            void AddChildToNode(string addingSuffix, Node node)
			{
				if (node.FindChild(addingSuffix[0], out var child))
				{
				    int difIndex;
				    for (difIndex = 1; difIndex < child.Text.Length; difIndex++)
						if (addingSuffix[difIndex] != child.Text[difIndex])
							break;

				    var addingSuffixRest = addingSuffix.Substring(difIndex);
					if (difIndex == child.Text.Length)
					    AddChildToNode(addingSuffixRest, child);
					else
					{
					    child.Split(difIndex);
						child.AddChild(addingSuffixRest);
					}
				}
				else
				    node.AddChild(addingSuffix);
			}

            var currentSuffix = string.Empty;
            for (int i = text.Length - 1; i >= 0; i--)
            {
                var currentChar = text[i];
                currentSuffix = currentChar + currentSuffix;
				AddChildToNode(currentSuffix, rootNode);
            }
        }

        private class Node
        {
            private readonly Dictionary<char, Node> children = new Dictionary<char, Node>();

            public Node(string text)
            {
                Text = text;
            }

            public string Text { get; private set; }

            public bool FindChild(char c, out Node node)
            {
                return children.TryGetValue(c, out node);
            }

            public void AddChild(string text)
            {
				children.Add(text[0], new Node(text));
            }

            public void Split(int index)
            {
				if (index == 0 || index == Text.Length)
					return;

				AddChild(Text.Substring(index));
                Text = Text.Substring(0, index);
            }

            public override string ToString()
            {
                return Text + "|" + string.Join('-', children.Select(pair => pair.Value.Text).OrderBy(t => t));
            }
        }
    }
}
