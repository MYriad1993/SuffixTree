namespace SuffixTreeLibrary
{
    public class SuffixTree
    {
        private readonly string text;
        private readonly Node rootNode;

        public SuffixTree(string text)
        {
            this.text = text + "$";
            IBuilder builder = new NLogNBuilder(this.text);
            rootNode = builder.Build();
        }

        public bool Contains(string substring) => Contains(substring, rootNode);

        private bool Contains(string s, Node node)
        {
            if (!node.TryGetChild(s[0], out var child))
                return false;

            if (s.Length == 1)
                return true;

            return Contains(s.Substring(1), child);
        }
    }
}
