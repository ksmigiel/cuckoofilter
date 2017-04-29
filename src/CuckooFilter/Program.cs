using System;

namespace CuckooFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            var cuckooFilter = new CuckooFilter();
            var item = new byte[] { 0, 255 };

            cuckooFilter.Insert(item);

            var deleted = cuckooFilter.Remove(new byte[] { 0, 255 });
            var found = cuckooFilter.Lookup(new byte[] { 0, 255 });

        }
    }
}