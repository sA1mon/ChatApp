using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace RsaUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //arrange
            var rsa = new Rsa.Rsa();
            var message = "hello world";
            var data = Encoding.Default.GetBytes(message);

            //act
            var enc = rsa.Crypt(data, rsa.Key);
            var dec = rsa.Decrypt(enc);
            var actual = Encoding.Default.GetString(dec);

            //assert
            Assert.AreEqual(message, actual);
        }
    }
}
