namespace BuddyLocator.Services
{
	public interface INotificationService
	{
		void UpdateLiveTile(string title, string backTitle = null, string backContent = null, int? count = null);
	}
}