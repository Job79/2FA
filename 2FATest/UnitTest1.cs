/* TwoFactorTest
 * 
 * This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2.
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoFactorAuthentication;

namespace TwoFactorTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string secret = TwoFactor.GenerateRandomSecret();
            var twoFactor = new TwoFactor(secret);

            string x = twoFactor.GenerateCode();
            Assert.AreEqual(x.Length, 6);

            Assert.IsTrue(twoFactor.ValidateCode(x)&&!twoFactor.ValidateCode("000000"));
        }
    }
}
