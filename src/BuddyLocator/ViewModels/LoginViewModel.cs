namespace BuddyLocator.ViewModels
{
	using Buddy;
	using Caliburn.Micro;
	using Services;

	public class LoginViewModel : ViewModelBase
	{
		public LoginViewModel(IServices services)
			: base(services)
		{
		}

		private string username;
		public string Username
		{
			get { return username; }
			set { username = value; NotifyOfPropertyChange(() => Username); }
		}

		private string password;
		public string Password
		{
			get { return password; }
			set { password = value; NotifyOfPropertyChange(() => Password); }
		}

		public void CreateNewUser()
		{
			Services.Navigation.UriFor<CreateUserViewModel>().Navigate();
		}

		public void LoginUser()
		{
			BeginLoading("Logging In...");
			Services.BuddyClient.Login((user, state) =>
			{
				EndLoading();
				if (state.Exception != null)
				{
					//TODO: Handle Error
				}
				else
				{
					Services.State.User = user;
					Services.Settings.UserToken = user.Token;
					Execute.OnUIThread(() => Services.Navigation.UriFor<MainPageViewModel>().Navigate());
				}
			}, Username, Password);
		}
	}
}