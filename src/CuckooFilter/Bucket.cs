using System;
using System.Linq;

namespace CuckooFilter
{
    public class Bucket
    {
        private readonly uint _capacity;
        private readonly byte[][] _slots;

        public Bucket(uint capacity)
        {
            _capacity = capacity;
            _slots = new byte[_capacity][];
        }

        public bool Insert(byte[] fingerprint)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] == null)
                {
                    _slots[i] = fingerprint;
                    return true;
                }
            }

            return false;
        }

        public bool Lookup(byte[] fingerprint)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] != null && _slots[i].SequenceEqual(fingerprint))
                    return true;
            }

            return false;

        }

        public bool Remove(byte[] fingerprint)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] != null && _slots[i].SequenceEqual(fingerprint))
                {
                    _slots[i] = null;
                    return true;
                }
            }

            return false;
        }

        public byte[] Swap(byte[] fingerprint)
        {
            var i = new Random().Next((int)_capacity);
            (_slots[i], fingerprint) = (fingerprint, _slots[i]);
            
            return fingerprint;
        }
    }
}
