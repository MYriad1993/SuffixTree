using System.Collections.Generic;

namespace SuffixTreeLibrary
{
    internal class NBuilder : IBuilder
    {
        private readonly string text;

        public NBuilder(string text)
        {
            this.text = text;
        }

        public Node Build()
        {
            var rootNode = new Node(text.Length, 0, text);

            var currentNode = rootNode;
            var nodeStack = new Stack<Node>();
            for (int i = text.Length - 1; i >= 0; i--)
            {
                Node linkedNode;
                while (!currentNode.TryGetPrefixLink(text[i], out linkedNode) || currentNode.Parent != null)
                {
                    nodeStack.Push(currentNode);
                    currentNode = currentNode.Parent;
                }

                var currentNodeTotalLength = text.Length - currentNode.StartIndex;
                var firstCharToCompareIndex = i + currentNodeTotalLength + 1;
                Node addedNode;
                if (!linkedNode.TryGetChild(text[firstCharToCompareIndex], out var child))
                {
                    addedNode = linkedNode.AddChild(firstCharToCompareIndex);
                    continue;
                }

                for (int j = firstCharToCompareIndex + 1, k = child.StartIndex + 1; ; j++, k++)
                {
                    if (text[j] != text[k])
                    {
                        child.Split(k);
                        addedNode = child.AddChild(k);
                        break;
                    }
                }


            }

            return rootNode;
        }
    }
}
