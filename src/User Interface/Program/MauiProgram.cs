﻿using BibTexManager.ViewModels;
using BibTexManager.Views;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Maui.Storage;
using DigitalProduction.Maui;
using DigitalProduction.Maui.Services;
using DigitalProduction.Maui.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace BibTexManager;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		MauiAppBuilder builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseDigitalProductionMauiAppToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		DigitalProduction.Maui.UI.LifecycleEventsInstaller.ConfigureLifecycleEvents(builder);
		#if WINDOWS
			builder.ConfigureLifecycleEvents(lifecycle =>  
			{
				lifecycle.AddWindows((builder) =>  
				{  
					builder.OnWindowCreated(del =>  
					{  
						del.Title = "BibTex Manager";
					});  
				});  
			});
		#endif

		RegisterViewsAndViewModels(builder.Services);
		RegisterEssentials(builder.Services);
		CreateServices(builder.Services);
		#if DEBUG
			builder.Logging.AddDebug();
		#endif

		return builder.Build();
	}

	static void RegisterViewsAndViewModels(IServiceCollection services)
	{
		services.AddSingleton<MainViewModel>();
		services.AddSingleton<MainPage>();

		services.AddTransient<EditRawBibEntryForm>();
		services.AddTransient<BibEntryViewModel>();

		services.AddTransientPopup<ProgramOptionsView, ProgramOptionsViewModel>();
		services.AddTransientPopup<ProjectOptionsView, ProjectOptionsViewModel>();
	}

	private static void CreateServices(IServiceCollection services)
	{
		services.AddSingleton<IBibTexFilePicker, BibTexFilePicker>();
		services.AddSingleton<IDialogService, DialogService>();
		services.AddSingleton<IRecentPathsManagerService, RecentPathsManagerService>();
		services.AddSingleton<ISaveFilePicker, SaveFilePicker>();
	}

	static void RegisterEssentials(in IServiceCollection services)
	{
		services.AddSingleton<IFileSaver>(FileSaver.Default);
		services.AddSingleton<IFolderPicker>(FolderPicker.Default);
		services.AddSingleton<ISpeechToText>(SpeechToText.Default);
		services.AddSingleton<ITextToSpeech>(TextToSpeech.Default);
	}
}