using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JBLib
{
    public class Attachments
    {
        readonly IDictionary<string, IList<(string filename, bool linked)>> dict = new Dictionary<string,IList<(string, bool)>>();

        public Attachments(IEnumerable<(string filepath, bool linked)> attachments)
        {
            foreach(var (filepath, linked) in attachments)
            {
                string ext = Path.GetExtension(filepath);

                string key = (ext == string.Empty) ? "none" : ext;
                
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, new List<(string, bool)>());
                }

                dict[key].Add((filepath, linked));
                TotalCount += 1;
            }
        }

        public int TotalCount { get; }

        public IEnumerable<(string filename, bool linked)> this[string extension]
            => dict.TryGetValue(extension, out var list) ? list : Enumerable.Empty<(string, bool)>();

        public IEnumerable<string> Extensions => dict.Keys;
    }
}
