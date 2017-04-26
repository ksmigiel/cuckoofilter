using System;

namespace CuckooFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            var cuckooFilter = new CuckooFilter();
            var item = new byte[] { 0, 255 };

            cuckooFilter.Add(item);

            var deleted = cuckooFilter.Delete(new byte[] { 0, 255 });
            var found = cuckooFilter.Lookup(new byte[] { 0, 255 });

        }
    }
}