namespace OBS.Internal.Auth
{
    using System;

    internal class V2Signer : AbstractSigner
    {
        private static V2Signer instance = new V2Signer();

        private V2Signer()
        {
        }

        protected override string GetAuthPrefix()
        {
            return "AWS";
        }

        public static Signer GetInstance()
        {
            return instance;
        }
    }
}

