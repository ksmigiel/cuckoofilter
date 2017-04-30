using System;
using System.Linq;
using System.Security.Cryptography;

namespace CuckooFilter
{
    public class CuckooFilter
    {
        private readonly uint _bucketCapacity;
        private readonly uint _fingerprintSize;
        private readonly uint _filterSize;
        private readonly uint _kicks;
        private uint _count;
        private readonly Bucket[] _buckets;
        private readonly Func<HashAlgorithm> _hashAlgorithm;

        public CuckooFilter() : this(new CuckooFilterOptions())
        {
        }

        public CuckooFilter(CuckooFilterOptions options)
        {
            _bucketCapacity = options.BucketCapacity;
            _fingerprintSize = options.FingerprintSize;
            _filterSize = options.FilterSize;
            _kicks = options.MaximumKicks;
            _hashAlgorithm = options.HashAlgorithm;

            _buckets = new Bucket[_filterSize];

            for (int i = 0; i < _filterSize; i++)
                _buckets[i] = new Bucket(_bucketCapacity);
        }

        public bool Insert(byte[] item)
        {
            var hashedItem = ComputeHash(item);
            var fingerprint = ExtractFingerprint(hashedItem);
            var i = GetIndex(hashedItem);
            var j = GetAlternateIndex(i, fingerprint);

            if (_buckets[i].Insert(fingerprint) || _buckets[j].Insert(fingerprint))
            {
                _count++;
                return true;
            }

            var index = new[] { i, j }[new Random().Next(2)];

            for (int n = 0; n < _kicks; n++)
            {
                fingerprint = _buckets[index].Swap(fingerprint);
                index = GetAlternateIndex(index, fingerprint);

                if (_buckets[index].Insert(fingerprint))
                {
                    _count++;
                    return true;
                }
            }

            return false;
        }

        public bool Lookup(byte[] item)
        {
            var hashedItem = ComputeHash(item);
            var fingerprint = ExtractFingerprint(hashedItem);
            var i = GetIndex(hashedItem);
            var j = GetAlternateIndex(i, fingerprint);

            return _buckets[i].Lookup(fingerprint) || _buckets[j].Lookup(fingerprint);
        }

        public bool Remove(byte[] item)
        {
            var hashedItem = ComputeHash(item);
            var fingerprint = ExtractFingerprint(hashedItem);
            var i = GetIndex(hashedItem);
            var j = GetAlternateIndex(i, fingerprint);

            if (_buckets[i].Remove(fingerprint) || _buckets[j].Remove(fingerprint))
            {
                _count--;
                return true;
            }

            return false;
        }

        private byte[] ExtractFingerprint(byte[] hashedItem)
        {
            // We could go with Array.Copy() or Buffer.BlockCopy()
            // but I don't see any performance improvement since
            // we are operating on small byte arrays.
            var fingerprint = new byte[_fingerprintSize];

            for (int i = 0; i < _fingerprintSize; i++)
                fingerprint[i] = hashedItem[i];

            return fingerprint;
        }

        private byte[] ComputeHash(byte[] item)
        {
            using (var hashAlgorithm = _hashAlgorithm())
            {
                return hashAlgorithm.ComputeHash(item);
            }
        }

        private uint GetIndex(byte[] hashedItem)
        {
            // TODO Get working more than 32 bits hashes
            // TODO What with int overflow?
            return BitConverter.ToUInt32(hashedItem, 0) % (uint)_filterSize;
        }

        private uint GetAlternateIndex(uint index, byte[] fingerprint)
        {
            return index ^ GetIndex(ComputeHash(fingerprint));
        }
    }
}
