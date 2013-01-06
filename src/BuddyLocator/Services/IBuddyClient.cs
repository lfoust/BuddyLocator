namespace BuddyLocator.Services
{
	using System;
	using System.Collections.Generic;
	using Buddy;

	public interface IBuddyClient
	{
		void Login(Action<Buddy.AuthenticatedUser, Buddy.BuddyCallbackParams> callback, string userName, string password);
		void Login(Action<Buddy.AuthenticatedUser, Buddy.BuddyCallbackParams> callback, string userToken);
		void CreateUser(Action<Buddy.AuthenticatedUser, Buddy.BuddyCallbackParams> callback, string userName, string password);
		void CheckUserName(Action<bool, BuddyCallbackParams> callback, string userName);
		void UpdateLocation(Buddy.AuthenticatedUser user, Action<bool, BuddyCallbackParams> callback, double longitude, double latitude);
		void GetNearbyLocations(Buddy.AuthenticatedUser user, Action<List<Place>, BuddyCallbackParams> callback, int searchDistance, double longitude, double latitude);
		void ChangeUserProfilePicture(Buddy.AuthenticatedUser user, Action<bool, BuddyCallbackParams> callback, byte[] imageData);
	}
}