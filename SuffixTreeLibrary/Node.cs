using System;
using System.Collections.Generic;
using System.Linq;

namespace SuffixTreeLibrary
{
    internal class Node
    {
        private readonly Dictionary<char, Node> children = new Dictionary<char, Node>();
        private readonly Dictionary<char, Node> prefixLinks = new Dictionary<char, Node>();
        private readonly string originalText; // just a pointer, does not take a lot of memory

        public Node(int startIndex, int length, string originalText, Node parent = null)
        {
            this.originalText = originalText;
            StartIndex = startIndex;
            Length = length;
            Parent = parent;
        }

        public int StartIndex { get; private set; }

        public int Length { get; private set; }

        public Node Parent { get; }

        public bool TryGetChild(char c, out Node node)
        {
            return children.TryGetValue(c, out node);
        }

        public bool TryGetPrefixLink(char c, out Node node)
        {
            return prefixLinks.TryGetValue(c, out node);
        }

        public Node AddChild(int startIndex)
        {
            var child = new Node(startIndex, originalText.Length - startIndex, originalText, this);
            children.Add(originalText[startIndex], child);
            return child;
        }

        public void Split(int index)
        {
			if (index == 0 || index == Length)
				return;

			AddChild(StartIndex + index);
            Length = index;
        }

        public int Compare(int originalTextStartIndex)
        {
            if (originalTextStartIndex > StartIndex)
                throw new Exception("Illegal method usage");

            for (int i = StartIndex, j = originalTextStartIndex; i - StartIndex < Length; i++, j++)
                if (originalText[i] != originalText[j])
                    return i - StartIndex;

            return Length;
        }

        public override string ToString()
        {
            return GetText() + "|" + string.Join('-', children.Select(pair => pair.Value.GetText()).OrderBy(t => t));
        }

        private string GetText() => originalText.Substring(StartIndex, Length);
    }
}
