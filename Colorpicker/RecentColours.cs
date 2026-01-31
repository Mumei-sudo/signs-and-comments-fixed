using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace Dark.Signs.Colorpicker
{
    public class RecentColours
	{
		static RecentColours()
		{
			RecentColours.Read();
		}
        
		public Color this[int index] => RecentColours._colors[index];
		


		public int Count  => RecentColours._colors.Count;

		
		public void Add(Color color)
		{
			RecentColours._colors.RemoveAll((Color c) => c == color);
			RecentColours._colors.Insert(0, color);
			while (RecentColours._colors.Count > max)
			{
				RecentColours._colors.RemoveAt(RecentColours._colors.Count - 1);
			}
			RecentColours.Write();
		}
        
		private static void Read()
		{
			string text = Path.Combine(GenFilePaths.ConfigFolderPath, "ColourPicker.xml");
			if (File.Exists(text))
			{
				try
				{
					Scribe.loader.InitLoading(text);
					RecentColours.ExposeData();
				}
				catch (Exception e)
				{
					Log.Error("ColourPicker :: Error loading recent colours from file:" + e);
				}
				finally
				{
					Scribe.loader.FinalizeLoading();
				}
			}
		}
		private static void Write()
		{
			try
			{
				string filePath = Path.Combine(GenFilePaths.ConfigFolderPath, "ColourPicker.xml");
				Scribe.saver.InitSaving(filePath, "ColourPicker");
				RecentColours.ExposeData();
			}
			catch (Exception ex3)
			{
				string str = "ColourPicker :: Error saving recent colours to file:";
				Exception ex2 = ex3;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			finally
			{
				Scribe.saver.FinalizeSaving();
			}
		}

		private static void ExposeData()
		{
			Scribe_Collections.Look<Color>(ref RecentColours._colors, "RecentColors", LookMode.Undefined, Array.Empty<object>());
		}

		private const int max = 20;
		private static List<Color> _colors = new List<Color>();
	}
}
