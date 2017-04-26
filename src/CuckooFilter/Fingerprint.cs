using System;
using System.Linq;

namespace CuckooFilter
{
    public class Fingerprint : IEquatable<Fingerprint>
    {
        protected byte[] _fingerprint;

        public Fingerprint(byte[] fingerprint)
        {
            _fingerprint = fingerprint;
        }

        public bool Equals(Fingerprint other)
        {
            return _fingerprint.SequenceEqual(other._fingerprint);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Equals(obj);
        }

        public override int GetHashCode()
        {
            return _fingerprint.GetHashCode();
        }
    }
}
