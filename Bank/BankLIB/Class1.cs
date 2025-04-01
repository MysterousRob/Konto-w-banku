using System;

namespace Bank
{
    public class Account
    {
        protected string client;
        protected decimal balance;
        protected bool isBlocked = false;

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

        public virtual void Deposit(decimal amount)
        {
            if (isBlocked)
                throw new InvalidOperationException("Operation not allowed. The account is blocked.");

            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");

            balance += amount;
        }

        public virtual void Withdraw(decimal amount)
        {
            if (isBlocked)
                throw new InvalidOperationException("Operation not allowed. The account is blocked.");

            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero.");

            if (amount > balance)
                throw new InvalidOperationException("Insufficient funds in the account.");

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

    public class KontoPlus : Account
    {
        private decimal overdraftLimit;
        private bool overdraftUsed = false;

        public KontoPlus(string client, decimal initialBalance, decimal overdraftLimit) : base(client, initialBalance)
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

        public override decimal Balance => balance + (overdraftUsed ? 0 : overdraftLimit);

        public override void Withdraw(decimal amount)
        {
            if (isBlocked)
                throw new InvalidOperationException("Operation not allowed. The account is blocked.");

            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero.");

            if (amount > Balance)
                throw new InvalidOperationException("Amount exceeds available balance and overdraft limit.");

            balance -= amount;

            if (balance < 0)
            {
                overdraftUsed = true;
                BlockAccount();
            }
        }

        public override void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");

            balance += amount;

            if (balance >= 0 && isBlocked)
            {
                overdraftUsed = false;
                UnblockAccount();
            }
        }
    }

    public class KontoLimit
    {
        private Account konto;
        private decimal limit;
        private bool overdraftUsed = false;

        public KontoLimit(string client, decimal initialBalance = 0, decimal limit = 100)
        {
            if (limit < 0)
                throw new ArgumentException("Limit cannot be negative.");

            this.konto = new Account(client, initialBalance);
            this.limit = limit;
        }

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

        public string Name => konto.Name;
        public decimal Balance => konto.Balance + (overdraftUsed ? 0 : limit);
        public bool IsBlocked => konto.IsBlocked;

        public void Deposit(decimal amount)
        {
            konto.Deposit(amount);
            if (konto.Balance >= 0 && konto.IsBlocked)
            {
                overdraftUsed = false;
                konto.UnblockAccount();
            }
        }

        public void Withdraw(decimal amount)
        {
            if (konto.IsBlocked)
                throw new InvalidOperationException("Operation is not allowed. The account is blocked.");

            if (amount <= 0)
                throw new ArgumentException("Withdrawl amount must be greater that zero.");

            if (amount > Balance)
                throw new InvalidOperationException("Amount exceeds available balance and limit.");

            konto.Withdraw(amount);

            if (konto.Balance < 0)
            {
                overdraftUsed = true;
                konto.BlockAccount();
            }
        }

        public void BlockAccount()
        {
            konto.BlockAccount();
        }

        public void UnblockAccount()
        {
            konto.UnblockAccount();
        }
    }
}
      