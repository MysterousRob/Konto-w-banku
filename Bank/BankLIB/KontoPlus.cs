using System;

namespace Bank
{
    public class KontoPlus : Account
    {
        private decimal overdraftLimit;
        private bool overdraftUsed = false;

        public KontoPlus(string client, decimal initialBalance, decimal overdraftLimit)
            : base(client, initialBalance)
        {
            if (overdraftLimit < 0)
                throw new ArgumentException("Overdraft limit cannot be negative.");

            this.overdraftLimit = overdraftLimit;
        }

        public decimal OverdraftLimit
        {
            get => overdraftLimit;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Overdraft limit cannot be negative.");

                overdraftLimit = value;
            }
        }

        public override decimal Balance =>
            base.Balance + (overdraftUsed ? 0 : overdraftLimit);

        public override void Withdraw(decimal amount)
        {
            if (IsBlocked)
                throw new InvalidOperationException("Operation not allowed. The account is blocked.");

            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero.");

            if (amount > Balance)
                throw new InvalidOperationException("Amount exceeds available balance and overdraft limit.");

            DecreaseBalance(amount);

            if (base.Balance < 0)
            {
                overdraftUsed = true;
                BlockAccount();
            }
        }

        public override void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");

            IncreaseBalance(amount);

            if (base.Balance >= 0 && IsBlocked)
            {
                overdraftUsed = false;
                UnblockAccount();
            }
        }

        public Account ToStandardAccount()
        {
            return new Account(this.Name, base.Balance);
        }
    }
}