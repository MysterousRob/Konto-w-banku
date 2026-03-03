using System;

namespace Bank
{
    public class Account : IAccount
    {
        private string client;
        private decimal balance;
        private bool isBlocked = false;

        public Account(string client, decimal initialBalance = 0)
        {
            if (string.IsNullOrWhiteSpace(client))
                throw new ArgumentException("Client name cannot be empty.");

            if (initialBalance < 0)
                throw new ArgumentException("Initial balance cannot be negative.");

            this.client = client;
            this.balance = initialBalance;
        }

        public string Name => client;

        public virtual decimal Balance => balance;

        public bool IsBlocked => isBlocked;

        protected void IncreaseBalance(decimal amount)
        {
            balance += amount;
        }

        protected void DecreaseBalance(decimal amount)
        {
            balance -= amount;
        }

        public virtual void Deposit(decimal amount)
        {
            if (isBlocked)
                throw new InvalidOperationException("Operation not allowed. The account is blocked.");

            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");

            IncreaseBalance(amount);
        }

        public virtual void Withdraw(decimal amount)
        {
            if (isBlocked)
                throw new InvalidOperationException("Operation not allowed. The account is blocked.");

            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero.");

            if (amount > balance)
                throw new InvalidOperationException("Insufficient funds in the account.");

            DecreaseBalance(amount);
        }

        public void BlockAccount()
        {
            isBlocked = true;
        }

        public void UnblockAccount()
        {
            isBlocked = false;
        }

        public KontoPlus ToKontoPlus(decimal limit)
        {
            return new KontoPlus(this.Name, this.Balance, limit);
        }
    }
}