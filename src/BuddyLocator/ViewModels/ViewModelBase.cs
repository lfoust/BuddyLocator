namespace BuddyLocator.ViewModels
{
	using Caliburn.Micro;
	using Services;

	public abstract class ViewModelBase : Screen, ISupportsLoading
	{
		public ViewModelBase(IServices services)
		{
			this.Services = services;
		}

		public IServices Services { get; private set; }

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

		public virtual void BeginLoading(string message)
		{
			LoadingMessage = message;
			IsLoading = true;
		}

		public virtual void EndLoading()
		{
			IsLoading = false;
		}
	}
}