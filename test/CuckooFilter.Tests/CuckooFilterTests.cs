using System;
using Xunit;

namespace CuckooFilter.Tests
{
    public class CuckooFilterTests
    {
        [Fact]
        public void ShouldAddItem_WhenByteArrayPassed()
        {
            var cuckooFilter = new CuckooFilter();
            var item = new byte[] { 0, 255 };

            var added = cuckooFilter.Add(item);

            Assert.True(added);
        }

        [Fact]
        public void ShouldFindItem_WhenItemWasAddedToFilter()
        {
            var cuckooFilter = new CuckooFilter();
            var item = new byte[] { 0, 255 };

            cuckooFilter.Add(item);
            var found = cuckooFilter.Lookup(new byte[] { 0, 255 });

            Assert.True(found);
        }

        [Fact]
        public void ShouldDeleteItem()
        {
            var cuckooFilter = new CuckooFilter();
            var item = new byte[] { 0, 255 };

            cuckooFilter.Add(item);

            var deleted = cuckooFilter.Delete(new byte[] { 0, 255 });
            var found = cuckooFilter.Lookup(new byte[] { 0, 255 });

            Assert.True(deleted);
            Assert.False(found);
        }
    }
}

