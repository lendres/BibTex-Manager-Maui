﻿using BibTeXLibrary;
using BibtexManager;
using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.Validation;

namespace BibTexManager.ViewModels;

[QueryProperty(nameof(AddMode), "AddMode")]
[QueryProperty(nameof(BibEntry), "BibEntry")]
public partial class BibEntryViewModel : ObservableObject
{
	#region Fields

	private bool							_addMode { get; set; }				= true;

	#endregion

	#region Construction

	public BibEntryViewModel()
	{
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial string						Title { get; set; }					= "Add Bibtex Entry";
	
	[ObservableProperty]
	public partial string						RawBibEntry { get; set; }			= "";

	[ObservableProperty]
	public partial BibEntry						BibEntry { get; set; }				= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	public partial ValidatableObject<string>	Key { get; set; }					= new();

	[ObservableProperty]
	public partial bool							Modified  { get; set; }				= false;

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }			= false;

	public bool AddMode
	{
		get => _addMode;
		set
		{
			_addMode = value;
			if (_addMode)
			{
				Title = "Add Bibtex Entry";
			}
			else
			{
				Title = "Edit Bibtex Entry";
			}
		}
	}

	#endregion

	#region Initialize and Validation

	public bool ValidateSubmittable() => IsSubmittable =  Parse();

	#endregion

	#region

	/// <summary>
	/// Check the quality of the text in the text box.
	/// </summary>
	private void CheckQuality()
	{
		if (Parse())
		{
			// Mapping.
			BibtexProject.Instance!.RemapEntryNames(BibEntry);

			// Cleaning.
			bool breakNext = false;
			foreach (TagProcessingData tagProcessingData in BibtexProject.Instance.CleanEntry(BibEntry))
			{
				// If the processing was cancelled, we break.  We have to loop back around here to give the
				// processing a chance to finish (it was yielded).  Now exit before processing another entry.
				if (breakNext)
				{
					break;
				}

				//CorrectionForm correctionForm	= new CorrectionForm(tagProcessingData);
				//DialogResult dialogResult		= correctionForm.Show(this);

				//breakNext = dialogResult == DialogResult.Cancel;
			}

			// String constants replacement.
			BibtexProject.Instance.ApplyStringConstants(BibEntry);

			// Key.
			if (_addMode)
			{
				BibtexProject.Instance.GenerateNewKey(BibEntry);
			}
			else
			{
				BibtexProject.Instance.ValidateKey(BibEntry);
			}

				RawBibEntry = BibEntry.ToString(BibtexProject.Instance.WriteSettings);
			}
		}

	/// <summary>
	/// Parse the text in the text box.  Returns true if successful and false otherwise.
	/// </summary>
	private bool Parse()
	{
		bool success = true;
		try
		{
			BibEntry = BibtexProject.Instance!.ParseSingleEntryText(RawBibEntry);
		}
		catch
		{
			// The text entered contained an error.  Display it and cancel the "ok" (return).
			//MessageBox.Show(this, exception.Message, "Error Parsing Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
			success = false;
		}

		return success;
	}


	#endregion
}