﻿using BibTeXManager;
using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.Controls;

namespace BibTeXManager.ViewModels;

public partial class CorrectionViewModel : ObservableObject
{
	#region Construction

	public CorrectionViewModel(TagProcessingData tagProcessingData)
	{
		TagProcessingData = tagProcessingData;
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial string				Title { get; set; }					= "Replace Text?";

	[ObservableProperty]
	public partial TagProcessingData	TagProcessingData { get; set; }

	[ObservableProperty]
	public partial string				ReplacementText  { get; set; }		= "";

	public bool							ReplaceText
	{
		get => TagProcessingData.Correction.ReplaceText;
		set => TagProcessingData.Correction.ReplaceText = value;
	}

	#endregion

	#region Events

	partial void OnTitleChanged(string value)
	{
		ReplacementText = value;
	}
	
	partial void OnTagProcessingDataChanged(TagProcessingData value)
	{
		ReplacementText = value.Correction.ReplacementText;
	}

	#endregion

	#region Methods

	public void SetResult(MessageBoxYesNoToAllResult dialogResult)
	{
		switch (dialogResult)
		{
			case MessageBoxYesNoToAllResult.YesToAll:
				TagProcessingData.AcceptAll = true;
				TagProcessingData.Correction.ReplaceText        = true;
				TagProcessingData.Correction.ReplacementText	= ReplacementText;
				break;

			case MessageBoxYesNoToAllResult.Yes:
				TagProcessingData.Correction.ReplaceText		= true;
				TagProcessingData.Correction.ReplacementText	= ReplacementText;
				break;

			case MessageBoxYesNoToAllResult.No:
				TagProcessingData.Correction.ReplaceText		= false;
				break;

			case MessageBoxYesNoToAllResult.Cancel:
				break;

			default:
				throw new Exception("Bad value.");
		}
	}

	#endregion
}