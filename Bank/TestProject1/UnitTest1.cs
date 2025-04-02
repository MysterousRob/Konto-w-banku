using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bank;


namespace TestProject1
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void Deposit_ShouldIncreaseBalance()
        {
            var account = new Account("John", 100);
            account.Deposit(50);
            Assert.AreEqual(150, account.Balance);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Withdraw_ShouldThrowException_WhenInsufficientFunds()
        {
            var account = new Account("John", 50);
            account.Withdraw(100);
        }
    }
}