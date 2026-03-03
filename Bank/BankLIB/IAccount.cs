

namespace Bank
{
    public interface IAccount
    {
        string Name { get; }
        decimal Balance { get; }
        bool IsBlocked { get; }


        void Deposit(decimal amount);
        void Withdraw(decimal amount);
        void BlockAccount();
        void UnblockAccount();
    }
}