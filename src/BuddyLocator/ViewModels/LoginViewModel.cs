namespace BuddyLocator.ViewModels
{
	using System.Windows;
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
			set { username = value; NotifyOfPropertyChange(() => Username); EvaluateValid(); }
		}

		private string password;
		public string Password
		{
			get { return password; }
			set { password = value; NotifyOfPropertyChange(() => Password); EvaluateValid(); }
		}

		private bool isValid;
		public bool IsValid
		{
			get { return isValid; }
			set { isValid = value; NotifyOfPropertyChange(() => IsValid); }
		}

		private void EvaluateValid()
		{
			IsValid = !string.IsNullOrEmpty(Username) &&
			          !string.IsNullOrEmpty(Password);
		}

		public void CreateNewUser()
		{
			Services.Navigation.UriFor<CreateUserViewModel>().Navigate();
		}

		public void LoginUser()
		{
			if (!IsValid)
			{
				return;
			}

			BeginLoading("Logging In...");
			Services.BuddyClient.Login((user, state) =>
			{
				EndLoading();
				if (state.Exception != null)
				{
					Execute.OnUIThread(() => MessageBox.Show("Login not successful."));
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