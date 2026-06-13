using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLibrary.Security;

namespace UnitTestProject1.Security
{
    [TestClass]
    public class PasswordHasherTests
    {
        [TestMethod]
        public void Hash_DoesNotReturnPlaintext()
        {
            var hash = PasswordHasher.Hash("geheim123");
            Assert.AreNotEqual("geheim123", hash);
        }

        [TestMethod]
        public void Hash_SamePasswordTwice_GivesDifferentHashes()
        {
            // Door de salt zijn twee hashes van hetzelfde wachtwoord verschillend.
            var hash1 = PasswordHasher.Hash("geheim123");
            var hash2 = PasswordHasher.Hash("geheim123");
            Assert.AreNotEqual(hash1, hash2);
        }

        [TestMethod]
        public void Verify_ReturnsTrue_ForCorrectPassword()
        {
            var hash = PasswordHasher.Hash("geheim123");
            Assert.IsTrue(PasswordHasher.Verify("geheim123", hash));
        }

        [TestMethod]
        public void Verify_ReturnsFalse_ForWrongPassword()
        {
            var hash = PasswordHasher.Hash("geheim123");
            Assert.IsFalse(PasswordHasher.Verify("verkeerd", hash));
        }
    }
}
