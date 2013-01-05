namespace BuddyLocator
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Windows.Controls;
	using Caliburn.Micro;
	using Microsoft.Phone.Controls;
	using Services;
	using ViewModels;

	public class DebugLog : ILog
	{
		public void Info(string format, params object[] args)
		{
			Debug.WriteLine(format, args);
		}

		public void Warn(string format, params object[] args)
		{
			Debug.WriteLine(format, args);
		}

		public void Error(Exception exception)
		{
			Debug.WriteLine(exception.ToString());
		}
	}

	public class AppBootstrapper : PhoneBootstrapper
	{
		PhoneContainer container;
		static readonly ILog DebugLogInstance = new DebugLog();

		protected override void OnUnhandledException(object sender, System.Windows.ApplicationUnhandledExceptionEventArgs e)
		{
			LogManager.GetLog(null).Error(e.ExceptionObject);
			base.OnUnhandledException(sender, e);
		}

		protected override void Configure()
		{
			LogManager.GetLog = type => DebugLogInstance;

			container = new PhoneContainer(RootFrame);

			container.RegisterPhoneServices();
			container.RegisterSingleton(typeof(IServices), null, typeof(Services.Services));
			container.RegisterInstance(typeof(Buddy.BuddyClient), null, new Buddy.BuddyClient(ApiKeys.ApplicationName, ApiKeys.ApplicationPassword));
			container.RegisterSingleton(typeof(IBuddyClient), null, typeof(BuddyClient));
			container.PerRequest<MainPageViewModel>();
			container.PerRequest<LoginViewModel>();
			container.PerRequest<CreateUserViewModel>();

			AddCustomConventions();
		}

		static void AddCustomConventions()
		{
			ConventionManager.AddElementConvention<Pivot>(Pivot.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
				(viewModelType, path, property, element, convention) =>
				{
					if (ConventionManager
						.GetElementConvention(typeof(ItemsControl))
						.ApplyBinding(viewModelType, path, property, element, convention))
					{
						ConventionManager
							.ConfigureSelectedItem(element, Pivot.SelectedItemProperty, viewModelType, path);
						ConventionManager
							.ApplyHeaderTemplate(element, Pivot.HeaderTemplateProperty, null, viewModelType);
						return true;
					}

					return false;
				};

			ConventionManager.AddElementConvention<Panorama>(Panorama.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
				(viewModelType, path, property, element, convention) =>
				{
					if (ConventionManager
						.GetElementConvention(typeof(ItemsControl))
						.ApplyBinding(viewModelType, path, property, element, convention))
					{
						ConventionManager
							.ConfigureSelectedItem(element, Panorama.SelectedItemProperty, viewModelType, path);
						ConventionManager
							.ApplyHeaderTemplate(element, Panorama.HeaderTemplateProperty, null, viewModelType);
						return true;
					}

					return false;
				};
		}

		protected override object GetInstance(Type service, string key)
		{
			return container.GetInstance(service, key);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return container.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			container.BuildUp(instance);
		}
	}
}
