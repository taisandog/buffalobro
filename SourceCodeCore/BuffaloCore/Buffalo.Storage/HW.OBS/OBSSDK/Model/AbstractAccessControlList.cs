namespace OBS.Model
{
    using System;
    using System.Collections.Generic;

    public abstract class AbstractAccessControlList
    {
        private IList<Grant> grants;

        protected AbstractAccessControlList()
        {
        }

        public void AddGrant(Grantee grantee, PermissionEnum permission)
        {
            Grant item = new Grant {
                Grantee = grantee,
                Permission = new PermissionEnum?(permission)
            };
            this.Grants.Add(item);
        }

        public void RemoveGrant(Grantee grantee)
        {
            IList<Grant> list = new List<Grant>();
            foreach (Grant grant in this.Grants)
            {
                if (grant.Grantee.Equals(grantee))
                {
                    list.Add(grant);
                }
            }
            foreach (Grant grant2 in list)
            {
                this.Grants.Remove(grant2);
            }
        }

        public void RemoveGrant(Grantee grantee, PermissionEnum permission)
        {
            foreach (Grant grant in this.Grants)
            {
                if (grant.Grantee.Equals(grantee))
                {
                    PermissionEnum? nullable = grant.Permission;
                    PermissionEnum enum2 = permission;
                    if ((((PermissionEnum) nullable.GetValueOrDefault()) == enum2) ? nullable.HasValue : false)
                    {
                        this.Grants.Remove(grant);
                        break;
                    }
                }
            }
        }

        public IList<Grant> Grants
        {
            get
            {
                return (this.grants ?? (this.grants = new List<Grant>()));
            }
            set
            {
                this.grants = value;
            }
        }
    }
}

