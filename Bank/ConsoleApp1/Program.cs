using Bank;
using System;


namespace Bank
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Creating a standard account...");
            Account account = new Account("Molenda", 100);
            Console.WriteLine($"Account created for {account.Name} with balance: {account.Balance}");

            Console.WriteLine("\nDepositing 50...");
            account.Deposit(50);
            Console.WriteLine($"New balance: {account.Balance}");

            Console.WriteLine("\nAttempting to withdraw 200...");
            try
            {
                account.Withdraw(200);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nConverting account to KontoPlus with overdraft limit of 100...");
            KontoPlus kontoPlus = new KontoPlus(account.Name, account.Balance, 100);
            Console.WriteLine($"KontoPlus created for {kontoPlus.Name} with balance: {kontoPlus.Balance} and overdraft limit: {kontoPlus.OverdraftLimit}");

            Console.WriteLine("\nWithdrawing 200 using overdraft...");
            try
            {
                kontoPlus.Withdraw(200);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.WriteLine($"New balance: {kontoPlus.Balance}, Account Blocked: {kontoPlus.IsBlocked}");

            Console.WriteLine("\nDepositing 150 to unblock the account...");
            kontoPlus.Deposit(150);
            Console.WriteLine($"New balance: {kontoPlus.Balance}, Account Blocked: {kontoPlus.IsBlocked}");

            Console.WriteLine("\nConverting back to a standard account...");
            account = new Account(kontoPlus.Name, kontoPlus.Balance);
            Console.WriteLine($"Standard account restored for {account.Name} with balance: {account.Balance}");

            Console.WriteLine("\nCreating KontoLimit with an internal Konto object...");
            KontoLimit kontoLimit = new KontoLimit("Kowalski", 200, 150);
            Console.WriteLine($"KontoLimit created for {kontoLimit.Name} with balance: {kontoLimit.Balance} and overdraft limit: {kontoLimit.Limit}");
        }
    }
}

