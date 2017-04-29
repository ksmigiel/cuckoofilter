using System;
using Xunit;

namespace CuckooFilter.Tests
{
    public class CuckooFilterTests
    {
        [Fact]
        public void ShouldInsertItem_WhenByteArrayPassed()
        {
            var cuckooFilter = new CuckooFilter();
            var item = new byte[] { 0, 255 };

            var added = cuckooFilter.Insert(item);

            Assert.True(added);
        }

        [Fact]
        public void ShouldFindItem_WhenItemWasInsertedToFilter()
        {
            var cuckooFilter = new CuckooFilter();
            var item = new byte[] { 0, 255 };

            cuckooFilter.Insert(item);
            var found = cuckooFilter.Lookup(new byte[] { 0, 255 });

            Assert.True(found);
        }

        [Fact]
        public void ShouldRemoveItem_WhenArrayOfBytesPassed()
        {
            var cuckooFilter = new CuckooFilter();
            var item = new byte[] { 0, 255 };

            cuckooFilter.Insert(item);

            var deleted = cuckooFilter.Remove(new byte[] { 0, 255 });
            var found = cuckooFilter.Lookup(new byte[] { 0, 255 });

            Assert.True(deleted);
            Assert.False(found);
        }
    }
}
