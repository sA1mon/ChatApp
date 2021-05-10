using System.Runtime.Serialization;
using Org.BouncyCastle.Math;

namespace Rsa
{
    [DataContract]
    public class PublicKey
    {
        [DataMember]
        public BigInteger N { get; }
        [DataMember]
        public BigInteger E { get; }

        public PublicKey(BigInteger n, BigInteger e)
        {
            N = n;
            E = e;
        }
    }
}