﻿using BibTexManager.ViewModels;
using CommunityToolkit.Maui.Views;
using DigitalProduction.Maui.Controls;
using DigitalProduction.Maui.ViewModels;
using DigitalProduction.Maui.Views;

namespace BibTexManager.Views;

public partial class MainPage : DigitalProductionMainPage
{
	#region Fields

	private readonly FilePickerFileType _bibliographyProjectFileType = new(new Dictionary<DevicePlatform, IEnumerable<string>>
	{
		{
			DevicePlatform.WinUI, new[]
			{
				".bibproj"
			}
		},
	});

	#endregion

	#region Construction

	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	#endregion

	#region Menu Events

	async void OnOpen(object sender, EventArgs eventArgs)
	{
		PickOptions pickOptions = new()
		{
			PickerTitle = "Select an Bibliography Project File",
			FileTypes   = _bibliographyProjectFileType
		};
		FileResult? result = await BrowseForFile(pickOptions);
		if (result != null)
		{
			MainViewModel? viewModel = BindingContext as MainViewModel;
			viewModel?.OpenProjectWithPathSave(result.FullPath);
		}
	}

	public static async Task<FileResult?> BrowseForFile(PickOptions options)
	{
		try
		{
			return await FilePicker.PickAsync(options);
		}
		catch
		{
			//(Exception exception)
			//string message = exception.Message;
			// The user canceled or something went wrong.
		}
		return null;
	}

	async void OnShowProgramOptions(object sender, EventArgs eventArgs)
	{
		ProgramOptionsViewModel viewModel = new();

		ProgramOptionsView	view	= new(viewModel);
		object?				result	= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
		}
	}

	async void OnHelp(object sender, EventArgs eventArgs)
	{
		System.Reflection.Assembly? entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
		System.Diagnostics.Debug.Assert(entryAssembly != null);
		string url = DigitalProduction.Reflection.Assembly.DocumentationAddress(entryAssembly);
		await Launcher.OpenAsync(url);
	}

	async void OnAbout(object sender, EventArgs eventArgs)
	{
		AboutView1 view = new(new AboutViewModel(true));
		_ = await Shell.Current.ShowPopupAsync(view);
	}

	#endregion

	#region Button Events

	async void OnNew(object sender, EventArgs eventArgs)
	{
		//TranslationMatrix translationMatrix = TranslationMatrix.CreateNewTranslationMatrix(TranslationMatrixNewName, InputProcessor, InputFile);

		await Shell.Current.GoToAsync(nameof(EditRawBibEntryForm), true, new Dictionary<string, object>
		{
			{"AddMode",  true},
		});






		//ConfigurationsViewModel? configurationsViewModel = BindingContext as ConfigurationsViewModel;
		//System.Diagnostics.Debug.Assert(configurationsViewModel != null);

		//ConfigurationViewModel	viewModel	= new(Interface.ConfigurationList?.ConfigurationNames ?? []);
		//ConfigurationView		view		= new(viewModel);
		//object?					result		= await Shell.Current.ShowPopupAsync(view);

		//if (result is bool boolResult && boolResult)
		//{
		//	configurationsViewModel?.Insert(viewModel.Configuration);
		//}
	}

	#endregion
}