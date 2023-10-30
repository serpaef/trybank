using System.Data;

namespace Trybank.Lib;

public class TrybankLib
{
    public bool Logged;
    public int loggedUser;

    public const int ACCOUNT_NUMBER_POS = 0;
    public const int ACCOUNT_AGENCY_POS = 1;
    public const int ACCOUNT_PWD_POS = 2;
    public const int ACCOUNT_BALANCE_POS = 3;
    public int[,] Bank;
    public int registeredAccounts;
    private int maxAccounts = 50;


    public TrybankLib()
    {
        loggedUser = -99;
        registeredAccounts = 0;
        Logged = false;
        Bank = new int[maxAccounts, 4];
    }

    // 1. Construa a funcionalidade de cadastrar novas contas
    public void RegisterAccount(int number, int agency, int pass)
    {
        for(int i = 0; i <= registeredAccounts; i++)
        {
            if(Bank[i, 0] == number && Bank[i, 1] == agency)
            {
                throw new ArgumentException("A conta já está sendo usada!");
            }
        }

        Bank[registeredAccounts,ACCOUNT_NUMBER_POS] = number;
        Bank[registeredAccounts,ACCOUNT_AGENCY_POS] = agency;
        Bank[registeredAccounts,ACCOUNT_PWD_POS] = pass;
        Bank[registeredAccounts,ACCOUNT_BALANCE_POS] = 0;

        registeredAccounts+= 1;
    }

    // 2. Construa a funcionalidade de fazer Login
    public void Login(int number, int agency, int pass)
    {
        int retrievedPass = 0;
        int accountPosition = -1;

        if (Logged) throw new AccessViolationException("Usuário já está logado");
        
        for (int i = 0; i <= registeredAccounts; i++)
        {
            if (Bank[i,ACCOUNT_NUMBER_POS] == number && Bank[i,ACCOUNT_AGENCY_POS] == agency)
            {
                accountPosition = i;
                retrievedPass = Bank[i,ACCOUNT_PWD_POS];
                break;
            }

            throw new ArgumentException("Agência + Conta não encontrada");
        }

        if (retrievedPass != pass)
        {
            throw new ArgumentException("Senha incorreta");
        }

        loggedUser = accountPosition;
        Logged = true;
    }

    // 3. Construa a funcionalidade de fazer Logout
    public void Logout()
    {
        if (!Logged) throw new AccessViolationException("Usuário não está logado");

        Logged = false;
        loggedUser = -99;
    }

    // 4. Construa a funcionalidade de checar o saldo
    public int CheckBalance()
    {
        if (!Logged) throw new AccessViolationException("Usuário não está logado");
        return Bank[loggedUser,ACCOUNT_BALANCE_POS];
    }

    // 5. Construa a funcionalidade de depositar dinheiro
    public void Deposit(int value)
    {
        if (!Logged) throw new AccessViolationException("Usuário não está logado");
        Bank[loggedUser,ACCOUNT_BALANCE_POS] += value;
    }

    // 6. Construa a funcionalidade de sacar dinheiro
    public void Withdraw(int value)
    {
        if (!Logged) throw new AccessViolationException("Usuário não está logado");
        if (value > Bank[loggedUser,ACCOUNT_BALANCE_POS]) throw new InvalidOperationException("Saldo insuficiente");
        Bank[loggedUser,ACCOUNT_BALANCE_POS] -= value;
    }

    // 7. Construa a funcionalidade de transferir dinheiro entre contas
    public void Transfer(int destinationNumber, int destinationAgency, int value)
    {
        if (!Logged) throw new AccessViolationException("Usuário não está logado");
        if (value > Bank[loggedUser,ACCOUNT_BALANCE_POS]) throw new InvalidOperationException("Saldo insuficiente");
        
        int destinationAccountId = -99;
        
        for (int i = 0; i <= registeredAccounts; i++)
        {
            if (Bank[i, 0] == destinationNumber && Bank[i, 1] == destinationAgency)
            {
                destinationAccountId = i;
                break;
            }
        }

        Bank[loggedUser,ACCOUNT_BALANCE_POS] -= value;
        Bank[destinationAccountId,ACCOUNT_BALANCE_POS] += value;
    }
   
}
