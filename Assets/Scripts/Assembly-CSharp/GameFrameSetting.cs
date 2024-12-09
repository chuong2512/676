using System.Collections.Generic;
using UnityEngine;

public class GameFrameSetting
{
	private static GameFrameSetting _instace;

	private static readonly List<int> AllMaxFrame = new List<int>(3)
	{
		30,
		60,
		int.MaxValue
	};

	public static GameFrameSetting Instance
	{
		get
		{
			if (_instace == null)
			{
				_instace = new GameFrameSetting();
			}
			return _instace;
		}
	}

	private GameFrameSetting()
	{
	}

	public void SetSettingGameFrame()
	{
		Application.targetFrameRate = AllMaxFrame[SingletonDontDestroy<SettingManager>.Instance.MaxFrame];
	}
}
