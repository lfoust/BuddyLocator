namespace BuddyLocator.ViewModels
{
	using System.Linq;
	using Caliburn.Micro;
	using Services;

	public class MainPageViewModel : Conductor<IScreen>.Collection.OneActive, ISupportsLoading
	{
		private IServices Services { get; set; }
		private readonly CheckinViewModel checkin;
		private readonly ProfileViewModel profile;

		public MainPageViewModel(IServices services)
		{
			this.Services = services;
			checkin = new CheckinViewModel(services);
			profile = new ProfileViewModel(services);
		}

		protected override void OnInitialize()
		{
			Items.Add(profile);
			Items.Add(checkin);

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
							profile.RefreshUserInfo();
						}
					}, token);
				}
			}

			ActivateItem(Items[0]);
		}

		private bool isLoading;
		public bool IsLoading
		{
			get { return isLoading; }
			set { isLoading = value; NotifyOfPropertyChange(() => IsLoading); }
		}

		private string loadingMessage;
		public string LoadingMessage
		{
			get { return loadingMessage; }
			set { loadingMessage = value; NotifyOfPropertyChange(() => LoadingMessage); }
		}

		public void BeginLoading(string message)
		{
			LoadingMessage = message;
			IsLoading = true;
			foreach (ISupportsLoading child in Items.OfType<ISupportsLoading>())
			{
				child.LoadingMessage = message;
				child.IsLoading = true;
			}
		}

		public void EndLoading()
		{
			IsLoading = false;
			foreach (ISupportsLoading child in Items.OfType<ISupportsLoading>())
			{
				child.IsLoading = false;
			}
		}
	}
}