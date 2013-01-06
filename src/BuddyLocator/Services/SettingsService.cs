namespace BuddyLocator.Services
{
	using System.IO.IsolatedStorage;

	public class SettingsService : ISettingsService
	{
		private readonly IsolatedStorageSettings settings;

		public SettingsService()
		{
			settings = IsolatedStorageSettings.ApplicationSettings;
		}

		public string UserToken
		{
			get { return GetString("UserToken"); }
			set { settings["UserToken"] = value; settings.Save(); }
		}

		private string GetString(string key)
		{
			if (!settings.Contains(key))
				return null;
			return (string) settings[key];
		}
	}
}