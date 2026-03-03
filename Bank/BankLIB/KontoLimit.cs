using System;

namespace Bank
{
    public class KontoLimit : IAccount
    {
        private Account konto;
        private decimal limit;
        private bool overdraftUsed = false;

        public KontoLimit(string client, decimal initialBalance = 0, decimal limit = 100)
        {
            if (limit < 0)
                throw new ArgumentException("Limit cannot be negative.");

            konto = new Account(client, initialBalance);
            this.limit = limit;
        }

        public string Name => konto.Name;

        // Available funds (same logic as KontoPlus)
        public decimal Balance =>
            konto.Balance + (overdraftUsed ? 0 : limit);

        public bool IsBlocked => konto.IsBlocked;

        public decimal Limit
        {
            get => limit;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Limit cannot be negative.");

                limit = value;
            }
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");

            // allow deposit even if blocked
            konto.UnblockAccount();
            konto.Deposit(amount);

            // if balance restored above 0 → reset overdraft
            if (konto.Balance >= 0)
            {
                overdraftUsed = false;
                konto.UnblockAccount();
            }
        }

        public void Withdraw(decimal amount)
        {
            if (IsBlocked)
                throw new InvalidOperationException("Operation not allowed. The account is blocked.");

            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero.");

            if (amount > Balance)
                throw new InvalidOperationException("Amount exceeds available balance and limit.");

            // Manual balance handling (since Account.Withdraw prevents overdraft)
            konto.UnblockAccount();
            typeof(Account)
                .GetMethod("DecreaseBalance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(konto, new object[] { amount });

            if (konto.Balance < 0)
            {
                overdraftUsed = true;
                konto.BlockAccount();
            }
        }

        public void BlockAccount() => konto.BlockAccount();

        public void UnblockAccount() => konto.UnblockAccount();
    }
}