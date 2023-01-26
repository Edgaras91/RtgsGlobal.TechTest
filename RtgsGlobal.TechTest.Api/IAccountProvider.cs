using RtgsGlobal.TechTest.Api.Controllers;
using RtgsGlobal.TechTest.Api.Exceptions;

namespace RtgsGlobal.TechTest.Api;

public interface IAccountProvider
{
	MyBalance GetBalance(string accountIdentifier);
	void Deposit(string accountIdentifier, decimal amount);
	void Transfer(MyTransferDto transfer);
	void Withdraw(string accountIdentifier, decimal amount);
}

public class AccountProvider : IAccountProvider
{
	private readonly IDictionary<string, MyBalance> _accounts;

	public AccountProvider()
	{
		_accounts = new Dictionary<string, MyBalance> {{"account-a", new MyBalance()}, {"account-b", new MyBalance()}};
	}

	public MyBalance GetBalance(string accountIdentifier) => GetAccountBalance(accountIdentifier);

	public void Deposit(string accountIdentifier, decimal amount) => AddTransaction(accountIdentifier, amount);

	public void Transfer(MyTransferDto transfer)
	{
		AddTransaction(transfer.DebtorAccountIdentifier, -transfer.Amount);
		AddTransaction(transfer.CreditorAccountIdentifier, transfer.Amount);
	}

	public void Withdraw(string accountIdentifier, decimal amount) => AddTransaction(accountIdentifier, -1 * amount);

	private void AddTransaction(string accountIdentifier, decimal amount)
	{
		var accountBalance = GetAccountBalance(accountIdentifier);

		var newBalance = accountBalance.Balance + amount;

		_accounts[accountIdentifier] = new MyBalance(newBalance);
	}

	private MyBalance GetAccountBalance(string accountIdentifier)
	{
		if (_accounts.TryGetValue(accountIdentifier, out var accountBalance) == false)
		{
			throw new NotFoundException($"Account '{accountIdentifier}' not found");
		}

		return accountBalance;
	}
}

public record MyBalance(decimal Balance = 0);
