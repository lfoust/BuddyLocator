namespace BuddyLocator.Services
{
	using System;
	using System.Device.Location;

	public class LocationService : ILocationService, IDisposable
	{
		private readonly IGeoPositionWatcher<GeoCoordinate> watcher;
		private GeoCoordinate latestPosition;
		private GeoPositionStatus latestStatus = GeoPositionStatus.NoData;

		public LocationService()
		{
			this.watcher = new GeoCoordinateWatcher();
			watcher.PositionChanged += PositionChanged;
			watcher.StatusChanged += StatusChanged;
			watcher.Start();
		}

		private void StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
		{
			latestStatus = e.Status;
		}

		private void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
		{
			latestPosition = e.Position.Location;
		}

		public GeoCoordinate GetLatestLocation()
		{
			return latestPosition;
		}

		public GeoPositionStatus GetLatestStatus()
		{
			return latestStatus;
		}

		public void Dispose()
		{
			if (watcher != null)
			{
				watcher.Stop();
				watcher.PositionChanged -= PositionChanged;
				watcher.StatusChanged -= StatusChanged;
			}
		}
	}
}