namespace BuddyLocator.ViewModels
{
	using Caliburn.Micro;
	using Services;

	public class CreateUserViewModel : ViewModelBase
	{
		public CreateUserViewModel(IServices services)
			: base(services)
		{
		}

		public string username;
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

		private string confirmPassword;
		public string ConfirmPassword
		{
			get { return confirmPassword; }
			set { confirmPassword = value; NotifyOfPropertyChange(() => ConfirmPassword); EvaluateValid(); }
		}

		private string email;
		public string Email
		{
			get { return email; }
			set { email = value; NotifyOfPropertyChange(() => Email); }
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
			          !string.IsNullOrEmpty(Password) &&
			          !string.IsNullOrEmpty(ConfirmPassword) &&
			          Password == ConfirmPassword;
		}

		public void CreateUser()
		{
			if (!IsValid)
			{
				return;
			}

			BeginLoading("Creating User...");
			
			//First, check if the username is unique
			Services.BuddyClient.CheckUserName((b, checkUserState) =>
			{
				if (checkUserState.Exception == null)
				{
					Services.BuddyClient.CreateUser((user, createUserState) =>
					{
						EndLoading();
						if (createUserState.Exception != null)
						{
							//TODO: Handle Error
						}
						else
						{
							Services.State.User = user;
							Execute.OnUIThread(() => Services.Navigation.UriFor<MainPageViewModel>().Navigate());
						}
					}, Username, Password);
				}
				else
				{
					EndLoading();
					//TODO: Handle username not unique
				}
			}, Username);
		}
	}
}