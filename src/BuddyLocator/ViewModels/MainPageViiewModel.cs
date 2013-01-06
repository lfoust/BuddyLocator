namespace BuddyLocator.ViewModels
{
	using System.Windows;
	using Buddy;
	using Caliburn.Micro;
	using Microsoft.Phone.Tasks;
	using Services;

	public class MainPageViewModel : ViewModelBase, IHandle<TaskCompleted<PhotoResult>>
	{
		public MainPageViewModel(IServices services)
			: base(services)
		{
		}

		protected override void OnActivate()
		{
			Services.Events.Subscribe(this);
			if (Services.State.User == null)
			{
				string token = Services.Settings.UserToken;
				if (string.IsNullOrEmpty(token))
				{
					Services.Navigation.UriFor<LoginViewModel>().Navigate();
					return;
				}
				else
				{
					BeginLoading("Loading User...");
					Services.BuddyClient.Login((user, state) =>
					{
						EndLoading();
						if (user != null)
						{
							Services.State.User = user;
							UserImage = Services.State.User.ProfilePicture.ToString();
							UserName = Services.State.User.Name;
						}
					}, token);
				}
			}
			else
			{
				UserImage = Services.State.User.ProfilePicture.ToString();
				UserName = Services.State.User.Name;
			}
		}

		protected override void OnDeactivate(bool close)
		{
			Services.Events.Unsubscribe(this);
			base.OnDeactivate(close);
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

		private IObservableCollection<Buddy.Place> nearbyPlaces = new BindableCollection<Place>();
		public IObservableCollection<Buddy.Place> NearbyPlaces
		{
			get { return nearbyPlaces; }
		}

		public void UpdateLocation()
		{
			var location = Services.Location.GetLatestLocation();
			BeginLoading("Checking in...");
			Services.BuddyClient.UpdateLocation(Services.State.User, (result, state) =>
			{
				EndLoading();
				if (result)
				{
					Execute.OnUIThread(() => MessageBox.Show("Checkin successful"));
				}
			}, location.Longitude, location.Latitude);
		}

		public void FindNearbyLocations()
		{
			var location = Services.Location.GetLatestLocation();
			nearbyPlaces.Clear();
			Services.BuddyClient.GetNearbyLocations(Services.State.User, (places, state) =>
			{
				Execute.OnUIThread(() => NearbyPlaces.AddRange(places));
			}, 200, location.Longitude, location.Latitude);
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