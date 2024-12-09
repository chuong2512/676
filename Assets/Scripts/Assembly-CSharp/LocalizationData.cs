using System;
using System.Collections.Generic;

[Serializable]
public class LocalizationData
{
	public List<localizationItem> items = new List<localizationItem>();

	public List<ColorPaletteItem> colorPalette = new List<ColorPaletteItem>();
}
