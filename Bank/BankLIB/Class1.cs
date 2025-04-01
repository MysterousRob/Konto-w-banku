using System;


namespace BankLIB
{
    public class Account
    {
        private string client;
        private decimal balance;
        private bool isBlocked;

        public Account(string client, decimal initialbalance = 0)
        {
            if (string.IsNullOrWhiteSpace(client) 
                throw new ArgumentNullException("Client name cannot be empty.");

            if (initialbalance < 0)
                throw new ArgumentOutOfRangeException("Initial balance cannot be negative.");

            this.client = client;
            this.balance = initialbalance;
        }

        public string Name => client;
        public decimal Balance => balance;
        public bool IsBlocked => isBlocked;

        public void Deposit(decimal amount)
        {
            if (IsBlocked)
                throw new InvalidOperationException("Operation not allowed. the account is blocked.");

            if (amount <= 0)
                throw new ArgumentException("Deposit ammount must begreater that zero.");

            balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (isBlocked)
                throw new InvalidOperationException("Operation not allowed. The account is blocked.");

            if (amount < 0)
                throw new ArgumentException("Withdrawl amount must be greater than zero.");

            if (amount  > 0)
                throw new InvalidOperationException("Insufficient Funds in the account.");
            balance -= amount;
        }

        public void BlockAccount()
        {
            isBlocked = true;
        }

        public void UnblockAccount()
        {
            isBlocked = false;
        }
    }
}
