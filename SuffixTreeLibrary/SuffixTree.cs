using System;
using System.Collections.Generic;
using System.Linq;

namespace SuffixTreeLibary
{
    public class SuffixTree
    {
        private readonly string originalText;
        private readonly Node rootNode;

        public SuffixTree(string text)
        {
            originalText = text + "$";
            rootNode = new Node(originalText.Length, 0, originalText);

            BuildNLogN();
        }

        public bool Contains(string substring)
        {
            bool Contains(string s, Node node)
            {
                if (!node.TryGetChild(s[0], out var child))
                    return false;

                if (s.Length == 1)
                    return true;

                return Contains(s.Substring(1), child);
            }

            return Contains(substring, rootNode);
        }

        private void BuildNLogN()
        {
            void AddChildToNode(int startIndex, Node node)
			{
                var addingSuffix = originalText.Substring(startIndex);
				if (node.TryGetChild(originalText[startIndex], out var child))
				{
				    int difIndex = child.Compare(startIndex);
					if (difIndex == child.Length)
					    AddChildToNode(startIndex + child.Length, child);
					else
					{
					    child.Split(difIndex);
						child.AddChild(startIndex + difIndex);
					}
				}
				else
				    node.AddChild(startIndex);
			}

            for (int i = originalText.Length - 1; i >= 0; i--)
				AddChildToNode(i, rootNode);
        }

        private class Node
        {
            private readonly Dictionary<char, Node> children = new Dictionary<char, Node>();
            private readonly string originalText; // just a pointer, does not take a lot of memory
            private readonly int startIndex;

            public Node(int startIndex, int length, string originalText)
            {
                this.originalText = originalText;
                this.startIndex = startIndex;
                Length = length;
            }

            public int Length { get; private set; }

            public bool TryGetChild(char c, out Node node)
            {
                return children.TryGetValue(c, out node);
            }

            public void AddChild(int startIndex)
            {
				children.Add(originalText[startIndex], new Node(startIndex, originalText.Length - startIndex, originalText));
            }

            public void Split(int index)
            {
				if (index == 0 || index == Length)
					return;

				AddChild(startIndex + index);
                Length = index;
            }

            public int Compare(int originalTextStartIndex)
            {
                if (originalTextStartIndex > startIndex)
                    throw new Exception("Illegal method usage");

                for (int i = startIndex, j = originalTextStartIndex; i - startIndex < Length; i++, j++)
                    if (originalText[i] != originalText[j])
                        return i - startIndex;

                return Length;
            }

            public override string ToString()
            {
                return GetText() + "|" + string.Join('-', children.Select(pair => pair.Value.GetText()).OrderBy(t => t));
            }

            private string GetText() => originalText.Substring(startIndex, Length);
        }
    }
}
