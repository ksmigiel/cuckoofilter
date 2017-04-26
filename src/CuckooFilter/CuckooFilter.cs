using System;
using System.Security.Cryptography;

namespace CuckooFilter
{
    public class CuckooFilter
    {
        private int _bucketCapacity = 4;
        private int _fingerprintSize = 3;
        private int _filterSize = (1 << 18) / 4;
        private int _kicks = 500;
        private int _count;
        private Bucket[] _buckets;
        private HashAlgorithm _hashFunction = SHA1.Create();

        public CuckooFilter()
        {
            _buckets = new Bucket[_filterSize];

            for (int i = 0; i < _filterSize; i++)
                _buckets[i] = new Bucket(_bucketCapacity);
        }

        public bool Add(byte[] item)
        {
            var hashedItem = ComputeHash(item);
            var fingerprint = Fingerprint(hashedItem);
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
            var fingerprint = Fingerprint(hashedItem);
            var i = GetIndex(hashedItem);
            var j = GetAlternateIndex(i, fingerprint);

            return _buckets[i].Lookup(fingerprint) || _buckets[j].Lookup(fingerprint);
        }

        public bool Delete(byte[] item)
        {
            var hashedItem = ComputeHash(item);
            var fingerprint = Fingerprint(hashedItem);
            var i = GetIndex(hashedItem);
            var j = GetAlternateIndex(i, fingerprint);

            if (_buckets[i].Remove(fingerprint) || _buckets[j].Remove(fingerprint))
            {
                _count--;
                return true;
            }

            return false;
        }

        private byte[] Fingerprint(byte[] hashedItem)
        {
            var fingerprint = new byte[_fingerprintSize];

            for (int i = 0; i < _fingerprintSize; i++)
                fingerprint[i] = hashedItem[i];

            return fingerprint;
        }

        private byte[] ComputeHash(byte[] item)
        {
            return _hashFunction.ComputeHash(item);
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
