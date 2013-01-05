using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuddyLocator.Services
{
	using Buddy;
	using Caliburn.Micro;

	public interface IServices
	{
		INavigationService Navigation { get; }
		IBuddyClient BuddyClient { get; }
		ApplicationState State { get; }
	}

	public class Services : IServices
	{
		public Services(INavigationService navigation, IBuddyClient buddyClient)
		{
			this.Navigation = navigation;
			this.BuddyClient = buddyClient;
			this.State = new ApplicationState();
		}

		public INavigationService Navigation { get; private set; }
		public IBuddyClient BuddyClient { get; private set; }
		public ApplicationState State { get; private set; }
	}

	public interface IBuddyClient
	{
		void Login(Action<Buddy.AuthenticatedUser, Buddy.BuddyCallbackParams> callback, string userName, string password);
		void CreateUser(Action<Buddy.AuthenticatedUser, Buddy.BuddyCallbackParams> callback, string userName, string password);
		void CheckUserName(Action<bool, BuddyCallbackParams> callback, string userName);
	}

	public class BuddyClient : IBuddyClient
	{
		private readonly Buddy.BuddyClient client;

		public BuddyClient(Buddy.BuddyClient client)
		{
			this.client = client;
		}

		public void Login(Action<AuthenticatedUser, BuddyCallbackParams> callback, string userName, string password)
		{
			client.LoginAsync(callback, userName, password);
		}

		public void CreateUser(Action<AuthenticatedUser, BuddyCallbackParams> callback, string userName, string password)
		{
			client.CreateUserAsync(callback, userName, password);
		}

		public void CheckUserName(Action<bool, BuddyCallbackParams> callback, string userName)
		{
			client.CheckIfUsernameExistsAsync(callback, userName);
		}
	}

	public static class ApiKeys
	{
		public static readonly string ApplicationName = "Buddy Locator";
		public static readonly string ApplicationPassword = "18AF7859-65E4-41F5-BA7E-14F6BA63438C";
	}

	public class ApplicationState
	{
		public Buddy.User User { get; set; }
	}
}
