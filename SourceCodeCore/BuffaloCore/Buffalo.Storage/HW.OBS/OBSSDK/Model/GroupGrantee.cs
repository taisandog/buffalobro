namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class GroupGrantee : Grantee
    {
        public GroupGrantee()
        {
        }

        public GroupGrantee(GroupGranteeEnum groupGranteeType)
        {
            this.GroupGranteeType = new GroupGranteeEnum?(groupGranteeType);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != base.GetType())
            {
                return false;
            }
            GroupGranteeEnum? groupGranteeType = this.GroupGranteeType;
            GroupGranteeEnum? nullable2 = (obj as GroupGrantee).GroupGranteeType;
            if (groupGranteeType.GetValueOrDefault() != nullable2.GetValueOrDefault())
            {
                return false;
            }
            return (groupGranteeType.HasValue == nullable2.HasValue);
        }

        public override int GetHashCode()
        {
            return this.GroupGranteeType.GetHashCode();
        }

        public GroupGranteeEnum? GroupGranteeType { get; set; }
    }
}

