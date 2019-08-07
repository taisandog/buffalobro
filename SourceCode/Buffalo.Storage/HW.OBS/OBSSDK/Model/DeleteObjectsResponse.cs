namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;

    public class DeleteObjectsResponse : ObsWebServiceResponse
    {
        private IList<DeletedObject> deleted;
        private IList<DeleteError> errors;

        public IList<DeletedObject> DeletedObjects
        {
            get
            {
                return (this.deleted ?? (this.deleted = new List<DeletedObject>()));
            }
            internal set
            {
                this.deleted = value;
            }
        }

        public IList<DeleteError> DeleteErrors
        {
            get
            {
                return (this.errors ?? (this.errors = new List<DeleteError>()));
            }
            internal set
            {
                this.errors = value;
            }
        }
    }
}

