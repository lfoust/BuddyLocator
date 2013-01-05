namespace BuddyLocator.ViewModels
{
	using Caliburn.Micro;
	using Services;

	public class MainPageViewModel : ViewModelBase
	{
		public MainPageViewModel(IServices services)
			: base(services)
		{
		}

		protected override void OnActivate()
		{
			if (Services.State.User == null)
			{
				Services.Navigation.UriFor<LoginViewModel>().Navigate();
			}
		}
	}
}
