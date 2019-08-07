namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class CanonicalGrantee : Grantee
    {
        public CanonicalGrantee()
        {
        }

        public CanonicalGrantee(string id)
        {
            this.Id = id;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (base.GetType() != obj.GetType())
            {
                return false;
            }
            CanonicalGrantee grantee = obj as CanonicalGrantee;
            if (!string.IsNullOrEmpty(this.Id))
            {
                return this.Id.Equals(grantee.Id);
            }
            return string.IsNullOrEmpty(grantee.Id);
        }

        public override int GetHashCode()
        {
            if (!string.IsNullOrEmpty(this.Id))
            {
                return this.Id.GetHashCode();
            }
            return 0;
        }

        [Obsolete]
        public string DisplayName { get; set; }

        public string Id { get; set; }
    }
}

