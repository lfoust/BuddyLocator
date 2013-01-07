namespace BuddyLocator.Services
{
	using System.Linq;
	using Microsoft.Phone.Shell;

	public class NotificationService : INotificationService
	{
		public void UpdateLiveTile(string title, string backTitle = null, string backContent = null, int? count = null)
		{
			ShellTile tile = ShellTile.ActiveTiles.First();
			tile.Update(new StandardTileData
			{
				Title = title,
				BackTitle = backTitle,
				BackContent = backContent,
				Count = count
			});
		}
	}
}