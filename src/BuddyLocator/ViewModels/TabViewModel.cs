namespace BuddyLocator.ViewModels
{
	using Services;

	public class TabViewModel : ViewModelBase
	{
		public TabViewModel(IServices services)
			: base(services)
		{
		}

		public override void BeginLoading(string message)
		{
			ISupportsLoading parent = this.Parent as ISupportsLoading;
			if (parent != null)
			{
				parent.BeginLoading(message);
			}
			base.BeginLoading(message);
		}

		public override void EndLoading()
		{
			ISupportsLoading parent = this.Parent as ISupportsLoading;
			if (parent != null)
			{
				parent.EndLoading();
			}
			base.EndLoading();
		}
	}
}