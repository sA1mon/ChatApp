using System;
using BigInteger = Org.BouncyCastle.Math.BigInteger;

namespace Rsa
{
    public class Rsa
    {
        #region Secret prime numbers

        private readonly BigInteger _p;
        private readonly BigInteger _q;

        #endregion

        private readonly BigInteger _d; //private part
        private readonly BigInteger _e; //public part
        private const int BitLength = 1024;
        private readonly Random _rnd;
        private BigInteger n => _p.Multiply(_q);

        public PublicKey Key => new PublicKey(n, _e);

        public Rsa()
        {
            _rnd = new Random();

            _p = BigInteger.ProbablePrime(BitLength, _rnd);
            _q = BigInteger.ProbablePrime(BitLength, _rnd);

            var fi = _p.Subtract(BigInteger.One).Multiply(_q.Subtract(BigInteger.One));

            _e = CalculateE(fi);
            _d = CalculateD(_e, fi);
        }

        public byte[] Crypt(byte[] data, PublicKey key)
        {
            return Encode(data, key.N, key.E);
        }

        public byte[] Decrypt(byte[] data)
        {
            return Decode(data, n, _d);
        }

        private static byte[] Encode(byte[] data, BigInteger n, BigInteger e)
        {
            return new BigInteger(data).ModPow(e, n).ToByteArray();
        }

        private static byte[] Decode(byte[] data, BigInteger n, BigInteger d)
        {
            return new BigInteger(data).ModPow(d, n).ToByteArray();
        }

        private BigInteger CalculateE(BigInteger phi)
        {
            var e = BigInteger.ProbablePrime(BitLength / 2, _rnd);
            while (phi.Gcd(e).CompareTo(BigInteger.One) > 0 && e.CompareTo(phi) < 0)
            {
                e.Add(BigInteger.One);
            }

            return e;
        }

        private static BigInteger CalculateD(BigInteger e, BigInteger phi)
        {
            return e.ModInverse(phi);
        }
    }
}