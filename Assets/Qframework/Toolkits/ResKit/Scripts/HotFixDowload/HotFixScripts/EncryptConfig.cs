using System;

namespace QFramework
{
    [Serializable]
    public class EncryptConfig
    {
        public bool EncryptAB = false;
        public bool EncryptKey = false;
        public string AESKey = "QFramework";
    }
}

