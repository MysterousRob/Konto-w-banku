using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bank;
using System;

namespace Bank.Tests
{
    [TestClass]
    public class KontoPlusTests
    {
        [TestMethod]
        public void Withdraw_WithinBalance_ShouldWork()
        {
            var acc = new KontoPlus("John", 100, 50);
            acc.Withdraw(80);

            Assert.AreEqual(20 + 50, acc.Balance);
        }

        [TestMethod]
        public void Withdraw_UsingOverdraft_ShouldBlockAccount()
        {
            var acc = new KontoPlus("John", 100, 50);

            acc.Withdraw(120);

            Assert.IsTrue(acc.IsBlocked);
        }

        [TestMethod]
        public void Withdraw_ExceedingLimit_ShouldThrow()
        {
            var acc = new KontoPlus("John", 100, 50);

            Assert.ThrowsException<InvalidOperationException>(() =>
                acc.Withdraw(200));
        }

        [TestMethod]
        public void Deposit_AfterOverdraft_ShouldUnblock()
        {
            var acc = new KontoPlus("John", 100, 50);

            acc.Withdraw(120);
            acc.Deposit(30);

            Assert.IsFalse(acc.IsBlocked);
        }

        [TestMethod]
        public void OverdraftLimit_SetNegative_ShouldThrow()
        {
            var acc = new KontoPlus("John", 100, 50);

            Assert.ThrowsException<ArgumentException>(() =>
                acc.OverdraftLimit = -10);
        }
    }
}