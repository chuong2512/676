using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowsSetting : SingletonDontDestroy<WindowsSetting>
{
	private abstract class ScreenHandler
	{
		protected static readonly List<int> Screen_Usual_Width = new List<int>(5) { 1920, 1760, 1600, 1440, 1280 };

		public abstract void OnValueChanged(int value);

		public abstract List<Dropdown.OptionData> GetAllDropdownOptionDatas();

		public abstract int GetCurrentDropdownValue();

		protected void SetScreenResolution(int screenWidth, int screenHeight)
		{
			if (SingletonDontDestroy<SettingManager>.Instance.WindowType == 2)
			{
				SingletonDontDestroy<WindowsSetting>.Instance.SetWindows(screenWidth, screenHeight);
			}
		}
	}

	private class Screen16_9Handler : ScreenHandler
	{
		private readonly List<int> Screen_16_9_Height = new List<int>(5) { 1080, 990, 900, 810, 720 };

		public override void OnValueChanged(int value)
		{
			SingletonDontDestroy<SettingManager>.Instance.ChangeResolution(ScreenHandler.Screen_Usual_Width[value], Screen_16_9_Height[value]);
			SetScreenResolution(ScreenHandler.Screen_Usual_Width[value], Screen_16_9_Height[value]);
		}

		public override List<Dropdown.OptionData> GetAllDropdownOptionDatas()
		{
			List<Dropdown.OptionData> list = new List<Dropdown.OptionData>(ScreenHandler.Screen_Usual_Width.Count);
			for (int i = 0; i < ScreenHandler.Screen_Usual_Width.Count; i++)
			{
				string text = ScreenHandler.Screen_Usual_Width[i] + " × " + Screen_16_9_Height[i];
				list.Add(new Dropdown.OptionData
				{
					text = text
				});
			}
			return list;
		}

		public override int GetCurrentDropdownValue()
		{
			for (int i = 0; i < Screen_16_9_Height.Count; i++)
			{
				if (SingletonDontDestroy<SettingManager>.Instance.Resolution_Height == Screen_16_9_Height[i])
				{
					return i;
				}
			}
			return 0;
		}
	}

	private class Screen16_10Handler : ScreenHandler
	{
		private readonly List<int> Screen_16_10_Height = new List<int>(5) { 1200, 1100, 1000, 900, 800 };

		public override void OnValueChanged(int value)
		{
			SingletonDontDestroy<SettingManager>.Instance.ChangeResolution(ScreenHandler.Screen_Usual_Width[value], Screen_16_10_Height[value]);
			SetScreenResolution(ScreenHandler.Screen_Usual_Width[value], Screen_16_10_Height[value]);
		}

		public override List<Dropdown.OptionData> GetAllDropdownOptionDatas()
		{
			List<Dropdown.OptionData> list = new List<Dropdown.OptionData>(ScreenHandler.Screen_Usual_Width.Count);
			for (int i = 0; i < ScreenHandler.Screen_Usual_Width.Count; i++)
			{
				string text = ScreenHandler.Screen_Usual_Width[i] + " × " + Screen_16_10_Height[i];
				list.Add(new Dropdown.OptionData
				{
					text = text
				});
			}
			return list;
		}

		public override int GetCurrentDropdownValue()
		{
			for (int i = 0; i < Screen_16_10_Height.Count; i++)
			{
				if (SingletonDontDestroy<SettingManager>.Instance.Resolution_Height == Screen_16_10_Height[i])
				{
					return i;
				}
			}
			return 0;
		}
	}

	private class ScreenSpecialHandler : ScreenHandler
	{
		private readonly List<int> ScreenAllHeight = new List<int>(10) { 1200, 1080, 1100, 990, 1000, 900, 900, 810, 800, 720 };

		public override void OnValueChanged(int value)
		{
			SingletonDontDestroy<SettingManager>.Instance.ChangeResolution(ScreenHandler.Screen_Usual_Width[value / 2], ScreenAllHeight[value]);
			SetScreenResolution(ScreenHandler.Screen_Usual_Width[value / 2], ScreenAllHeight[value]);
		}

		public override List<Dropdown.OptionData> GetAllDropdownOptionDatas()
		{
			List<Dropdown.OptionData> list = new List<Dropdown.OptionData>(ScreenAllHeight.Count);
			for (int i = 0; i < ScreenAllHeight.Count; i++)
			{
				string text = ScreenHandler.Screen_Usual_Width[i / 2] + " × " + ScreenAllHeight[i];
				list.Add(new Dropdown.OptionData
				{
					text = text
				});
			}
			return list;
		}

		public override int GetCurrentDropdownValue()
		{
			for (int i = 0; i < ScreenAllHeight.Count; i++)
			{
				if (SingletonDontDestroy<SettingManager>.Instance.Resolution_Height == ScreenAllHeight[i])
				{
					return i;
				}
			}
			return 0;
		}
	}

	private ScreenHandler currentHandler;

	private static IntPtr currentWindow;

	protected override void Awake()
	{
		base.Awake();
		if (!(SingletonDontDestroy<WindowsSetting>.Instance != this))
		{
			if ((float)Screen.currentResolution.width / (float)Screen.currentResolution.height - 1.6f < 0.01f)
			{
				currentHandler = new Screen16_10Handler();
			}
			else if ((float)Screen.currentResolution.width / (float)Screen.currentResolution.height - 1.77777779f < 0.01f)
			{
				currentHandler = new Screen16_9Handler();
			}
			else
			{
				currentHandler = new ScreenSpecialHandler();
			}
		}
	}

	public void SetWindowType(bool broadcastEvent)
	{
		switch (SingletonDontDestroy<SettingManager>.Instance.WindowType)
		{
		case 0:
			SetFullScreen();
			break;
		case 1:
			SetFullScreenNoFrame();
			break;
		case 2:
			SetWindows(SingletonDontDestroy<SettingManager>.Instance.Resolution_Width, SingletonDontDestroy<SettingManager>.Instance.Resolution_Height);
			break;
		}
		if (broadcastEvent)
		{
			EventManager.BroadcastEvent(EventEnum.E_ScreenResolutionChanged, null);
		}
	}

	public int GetCurrentDropdownValue()
	{
		return currentHandler.GetCurrentDropdownValue();
	}

	public List<Dropdown.OptionData> GetAllDropdownOptionDatas()
	{
		return currentHandler.GetAllDropdownOptionDatas();
	}

	public void OnResolutionDropdownValueChanged(int value)
	{
		currentHandler.OnValueChanged(value);
	}

	public void SetWindows(int windowWidth, int windowHeight)
	{
		Screen.SetResolution(windowWidth, windowHeight, fullscreen: false);
	}

	public void SetFullScreen()
	{
		Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, fullscreen: true);
	}

	public void SetFullScreenNoFrame()
	{
		Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, fullscreen: false);
	}
}
