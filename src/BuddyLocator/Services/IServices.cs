﻿namespace BuddyLocator.Services
{
	using Caliburn.Micro;

	public interface IServices
	{
		INavigationService Navigation { get; }
		IBuddyClient BuddyClient { get; }
		IEventAggregator Events { get; }
		ILocationService Location { get; }
		ISettingsService Settings { get; }
		ApplicationState State { get; }
	}
}