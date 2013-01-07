namespace BuddyLocator.ViewModels
{
	using System;
	using System.Windows;
	using Buddy;
	using Caliburn.Micro;
	using Services;

	public class CheckinViewModel : TabViewModel
	{
		public CheckinViewModel(IServices services)
			: base(services)
		{
			DisplayName = "check-in";
		}

		protected override void OnActivate()
		{
			SetUpdatedLocation();
		}

		private void SetUpdatedLocation()
		{
			var location = Services.Location.GetLatestLocation();
			if (location == null)
			{
				CurrentLocation = "none";
			}
			else
			{
				string newLocation = string.Format("{0}, {1}", location.Latitude, location.Longitude);
				if (newLocation != CurrentLocation)
				{
					CurrentLocation = newLocation;
					this.FindNearbyLocations();
				}
			}
		}

		private string currentLocation;
		public string CurrentLocation
		{
			get { return currentLocation; }
			set { currentLocation = value; NotifyOfPropertyChange(() => CurrentLocation); }
		}

		private readonly IObservableCollection<Buddy.Place> nearbyPlaces = new BindableCollection<Place>();
		public IObservableCollection<Buddy.Place> NearbyPlaces
		{
			get { return nearbyPlaces; }
		}

		public void CheckIn()
		{
			var location = Services.Location.GetLatestLocation();
			BeginLoading("Checking in...");
			Services.BuddyClient.UpdateLocation(Services.State.User, (result, state) =>
			{
				EndLoading();
				if (result)
				{
					SetUpdatedLocation();
					Services.Notification.UpdateLiveTile(
						title: "New Check-in", 
						backTitle: string.Format("{0:d}", DateTime.Now), 
						backContent: String.Format("({0}, {1})", location.Latitude, location.Longitude));
				}
			}, location.Longitude, location.Latitude);
		}

		public void FindNearbyLocations()
		{
			var location = Services.Location.GetLatestLocation();
			nearbyPlaces.Clear();
			BeginLoading("Finding Nearby Locations...");
			Services.BuddyClient.GetNearbyLocations(Services.State.User, (places, state) =>
			{
				EndLoading();
				Execute.OnUIThread(() => NearbyPlaces.AddRange(places));
			}, 200, location.Longitude, location.Latitude);
		}
	}
}