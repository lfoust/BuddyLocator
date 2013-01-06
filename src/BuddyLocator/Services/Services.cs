namespace BuddyLocator.Services
{
	using Caliburn.Micro;

	public class Services : IServices
	{
		public Services(INavigationService navigation, IBuddyClient buddyClient, IEventAggregator events, ILocationService location, ISettingsService settings)
		{
			this.Navigation = navigation;
			this.BuddyClient = buddyClient;
			this.Events = events;
			this.Location = location;
			this.Settings = settings;
			this.State = new ApplicationState();
		}

		public INavigationService Navigation { get; private set; }
		public IBuddyClient BuddyClient { get; private set; }
		public IEventAggregator Events { get; private set; }
		public ILocationService Location { get; private set; }
		public ISettingsService Settings { get; private set; }
		public ApplicationState State { get; private set; }
	}
}
