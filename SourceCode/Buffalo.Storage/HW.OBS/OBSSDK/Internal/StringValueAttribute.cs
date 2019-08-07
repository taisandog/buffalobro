namespace OBS.Internal
{
    using System;

    internal class StringValueAttribute : Attribute
    {
        private string _value;

        public StringValueAttribute(string value)
        {
            this._value = value;
        }

        public string StringValue
        {
            get
            {
                return this._value;
            }
        }
    }
}

