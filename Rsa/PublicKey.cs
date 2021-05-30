using System;
using Org.BouncyCastle.Math;

namespace Rsa
{
    [Serializable]
    public class PublicKey
    {
        public BigInteger N { get; }
        public BigInteger E { get; }

        public PublicKey(BigInteger n, BigInteger e)
        {
            N = n;
            E = e;
        }
    }
}