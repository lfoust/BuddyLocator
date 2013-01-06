namespace BuddyLocator.Services
{
	using System;
	using System.Collections.Generic;
	using Buddy;

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

		public void Login(Action<AuthenticatedUser, BuddyCallbackParams> callback, string userToken)
		{
			client.LoginAsync(callback, userToken);
		}

		public void CreateUser(Action<AuthenticatedUser, BuddyCallbackParams> callback, string userName, string password)
		{
			client.CreateUserAsync(callback, userName, password);
		}

		public void CheckUserName(Action<bool, BuddyCallbackParams> callback, string userName)
		{
			client.CheckIfUsernameExistsAsync(callback, userName);
		}

		public void UpdateLocation(Buddy.AuthenticatedUser user, Action<bool, BuddyCallbackParams> callback, double longitude, double latitude)
		{
			user.CheckInAsync(callback, latitude, longitude);
		}

		public void GetNearbyLocations(Buddy.AuthenticatedUser user, Action<List<Place>, BuddyCallbackParams> callback, int searchDistance, double longitude, double latitude)
		{
			user.Places.FindAsync(callback, searchDistance, latitude, longitude);
		}

		public void ChangeUserProfilePicture(Buddy.AuthenticatedUser user, Action<bool, BuddyCallbackParams> callback, byte[] imageData)
		{
			user.AddProfilePhotoAsync(callback, imageData);
		}
	}
}