using System;
using System.Collections.Generic;
using System.Linq;

namespace SuffixTreeLibrary
{
    internal class Node
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
