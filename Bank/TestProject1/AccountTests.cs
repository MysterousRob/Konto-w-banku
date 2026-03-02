using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bank;
using System;

namespace Bank.Tests
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void Constructor_Valid_ShouldCreateAccount()
        {
            var acc = new Account("John", 100);
            Assert.AreEqual("John", acc.Name);
            Assert.AreEqual(100, acc.Balance);
            Assert.IsFalse(acc.IsBlocked);
        }

        [TestMethod]
        public void Constructor_InvalidClient_ShouldThrow()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Account(null!, 100));

            Assert.ThrowsException<ArgumentException>(() =>
                new Account("", 100));
        }

        [TestMethod]
        public void Constructor_NegativeBalance_ShouldThrow()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Account("John", -10));
        }

        [TestMethod]
        public void Deposit_Valid_ShouldIncreaseBalance()
        {
            var acc = new Account("John", 100);
            acc.Deposit(50);
            Assert.AreEqual(150, acc.Balance);
        }

        [TestMethod]
        public void Deposit_InvalidAmount_ShouldThrow()
        {
            var acc = new Account("John", 100);

            Assert.ThrowsException<ArgumentException>(() => acc.Deposit(0));
            Assert.ThrowsException<ArgumentException>(() => acc.Deposit(-5));
        }

        [TestMethod]
        public void Withdraw_Valid_ShouldDecreaseBalance()
        {
            var acc = new Account("John", 100);
            acc.Withdraw(40);
            Assert.AreEqual(60, acc.Balance);
        }

        [TestMethod]
        public void Withdraw_InsufficientFunds_ShouldThrow()
        {
            var acc = new Account("John", 50);

            Assert.ThrowsException<InvalidOperationException>(() =>
                acc.Withdraw(100));
        }

        [TestMethod]
        public void Withdraw_InvalidAmount_ShouldThrow()
        {
            var acc = new Account("John", 100);

            Assert.ThrowsException<ArgumentException>(() => acc.Withdraw(0));
            Assert.ThrowsException<ArgumentException>(() => acc.Withdraw(-5));
        }

        [TestMethod]
        public void Operations_WhenBlocked_ShouldThrow()
        {
            var acc = new Account("John", 100);
            acc.BlockAccount();

            Assert.ThrowsException<InvalidOperationException>(() =>
                acc.Deposit(10));

            Assert.ThrowsException<InvalidOperationException>(() =>
                acc.Withdraw(10));
        }

        [TestMethod]
        public void Block_Unblock_ShouldChangeState()
        {
            var acc = new Account("John", 100);

            acc.BlockAccount();
            Assert.IsTrue(acc.IsBlocked);

            acc.UnblockAccount();
            Assert.IsFalse(acc.IsBlocked);
        }
    }
}