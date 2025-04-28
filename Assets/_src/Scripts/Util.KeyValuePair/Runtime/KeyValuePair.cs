using System;
using System.Collections.Generic;
using System.Text;

namespace _src.Scripts.Util.KeyValuePair.Runtime
{
    [Serializable]
    public struct KeyValuePair<TKey, TValue> : IEquatable<KeyValuePair<TKey, TValue>>
        where TValue : unmanaged, IEquatable<TValue>
    {
        public TKey key;
        public TValue value;


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            if ((object)this.key != null)
                sb.Append(this.key.ToString());
            sb.Append(", ");
            sb.Append(this.value.ToString());
            sb.Append(']');
            return sb.ToString();
        }

        public bool Equals(KeyValuePair<TKey, TValue> other)
        {
            return EqualityComparer<TKey>.Default.Equals(key, other.key) &&
                   EqualityComparer<TValue>.Default.Equals(value, other.value);
        }

        public override bool Equals(object obj)
        {
            return obj is KeyValuePair<TKey, TValue> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(key, value);
        }
    }
}