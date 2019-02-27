using SuffixTreeLibary;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var suffixTree = new SuffixTree("MISSISSIPPI");
            var a = suffixTree.Contains("ISS");
            var b = suffixTree.Contains("ISIPPI");
        }
    }
}
