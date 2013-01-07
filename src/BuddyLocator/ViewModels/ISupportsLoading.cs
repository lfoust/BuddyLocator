namespace BuddyLocator.ViewModels
{
	public interface ISupportsLoading
	{
		bool IsLoading { get; set; }
		string LoadingMessage { get; set; }
		void BeginLoading(string message);
		void EndLoading();
	}
}