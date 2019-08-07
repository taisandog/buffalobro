namespace OBS.Model
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public sealed class MetadataCollection
    {
        private IDictionary<string, string> values = new Dictionary<string, string>();

        public void Add(string name, string value)
        {
            this[name] = value;
        }

        public int Count
        {
            get
            {
                return this.values.Count;
            }
        }

        public string this[string name]
        {
            get
            {
                string str;
                if (this.values.TryGetValue(name, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                this.values[name] = value;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return this.values.Keys;
            }
        }

        public IList<KeyValuePair<string, string>> KeyValuePairs
        {
            get
            {
                return new List<KeyValuePair<string, string>>(this.values);
            }
        }

        public ICollection<string> Values
        {
            get
            {
                return this.values.Values;
            }
        }
    }
}

