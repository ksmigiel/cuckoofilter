using System;
using System.Security.Cryptography;

namespace CuckooFilter
{
    public class CuckooFilterOptions
    {
        private uint _filterSize;
        private uint _bucketCapacity;
        private uint _fingerprintSize;
        private uint _maximumKicks;
        private HashAlgorithm _hashAlgorithm;

        private const uint DEFAULT_FILTER_SIZE = (1 << 18) / 4;
        private const uint DEFAULT_BUCKET_CAPACITY = 4;
        private const uint DEFAULT_FINGERPRuint_SIZE = 3;
        private const uint DEFAULT_MAXIMUM_KICKS = 500;
        private readonly HashAlgorithm DEFAULT_HASH_ALGORITHM = SHA1.Create();

        public uint FilterSize
        {
            get
            {
                return _filterSize == 0 ? DEFAULT_FILTER_SIZE : _filterSize;
            }
            set
            {
                _filterSize = value;
            }
        }

        public uint BucketCapacity
        {
            get
            {
                return _bucketCapacity == 0 ? DEFAULT_BUCKET_CAPACITY : _bucketCapacity;
            }
            set
            {
                _bucketCapacity = value;
            }
        }

        public uint FingerprintSize
        {
            get
            {
                return _fingerprintSize == 0 ? DEFAULT_FINGERPRuint_SIZE : _fingerprintSize;
            }
            set
            {
                _fingerprintSize = value;
            }
        }

        public uint MaximumKicks
        {
            get
            {
                return _maximumKicks == 0 ? DEFAULT_MAXIMUM_KICKS : _maximumKicks;
            }
            set
            {
                _maximumKicks = value;
            }
        }

        public HashAlgorithm HashAlgorithm
        {
            get
            {
                return _hashAlgorithm == null ? DEFAULT_HASH_ALGORITHM : _hashAlgorithm;
            }
            set
            {
                _hashAlgorithm = value;
            }
        }
    }
}
