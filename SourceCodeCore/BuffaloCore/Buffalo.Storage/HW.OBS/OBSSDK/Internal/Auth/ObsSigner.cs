namespace OBS.Internal.Auth
{
    using System;

    internal class ObsSigner : AbstractSigner
    {
        private static ObsSigner instance = new ObsSigner();

        private ObsSigner()
        {
        }

        protected override string GetAuthPrefix()
        {
            return "OBS";
        }

        public static Signer GetInstance()
        {
            return instance;
        }
    }
}

