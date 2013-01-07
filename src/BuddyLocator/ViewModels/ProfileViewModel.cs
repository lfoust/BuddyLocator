namespace BuddyLocator.ViewModels
{
	using System.Windows;
	using Caliburn.Micro;
	using Microsoft.Phone.Tasks;
	using Services;

	public class ProfileViewModel : TabViewModel, IHandle<TaskCompleted<PhotoResult>>
	{
		public ProfileViewModel(IServices services)
			: base(services)
		{
			DisplayName = "profile";
		}

		protected override void OnActivate()
		{
			Services.Events.Subscribe(this);
			RefreshUserInfo();
		}

		protected override void OnDeactivate(bool close)
		{
			Services.Events.Unsubscribe(this);
			base.OnDeactivate(close);
		}

		public void RefreshUserInfo()
		{
			var user = Services.State.User;
			if (user != null)
			{
				UserName = user.Name;
				UserImage = user.ProfilePicture.ToString();
			}
		}

		private string userImage;
		public string UserImage
		{
			get { return userImage; }
			set { userImage = value; NotifyOfPropertyChange(() => UserImage); }
		}

		private string userName;
		public string UserName
		{
			get { return userName; }
			set { userName = value; NotifyOfPropertyChange(() => UserName); }
		}

		public void Logout()
		{
			Services.Settings.UserToken = null;
			Services.State.User = null;
			Services.Navigation.UriFor<LoginViewModel>().Navigate();
		}

		public void ChangePicture()
		{
			Services.Events.RequestTask<PhotoChooserTask>();
		}

		public void Handle(TaskCompleted<PhotoResult> message)
		{
			byte[] imageData = new byte[message.Result.ChosenPhoto.Length];
			message.Result.ChosenPhoto.Read(imageData, 0, imageData.Length);

			BeginLoading("Updating Profile Image...");
			Services.BuddyClient.ChangeUserProfilePicture(Services.State.User, (result, state) =>
			{
				if (result)
				{
					// Refresh Image
					Services.BuddyClient.Login((user, loginState) =>
					{
						EndLoading();
						if (user != null)
						{
							Services.State.User = user;
							UserImage = Services.State.User.ProfilePicture.ToString();
						}
					}, Services.State.User.Token);
				}
				else
				{
					Execute.OnUIThread(() => MessageBox.Show("Image Update NOT Successful"));
				}
			}, imageData);
		}
	}
}