namespace SuffixTreeLibrary
{
    internal class NLogNBuilder : IBuilder
    {
        private readonly string text;

        public NLogNBuilder(string text)
        {
            this.text = text;
        }

        public Node Build()
        {
            var rootNode = new Node(text.Length, 0, text);
            for (int i = text.Length - 1; i >= 0; i--)
                AddChildToNode(i, rootNode);

            return rootNode;
        }

        private void AddChildToNode(int startIndex, Node node)
        {
            var addingSuffix = text.Substring(startIndex);
            if (node.TryGetChild(text[startIndex], out var child))
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
    }
}
