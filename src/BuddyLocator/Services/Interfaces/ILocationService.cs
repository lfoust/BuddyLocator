namespace BuddyLocator.Services
{
	using System.Device.Location;

	public interface ILocationService
	{
		GeoCoordinate GetLatestLocation();
	}
}