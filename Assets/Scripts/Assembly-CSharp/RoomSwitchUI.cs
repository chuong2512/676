using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSwitchUI : UIView, ILocalization
{
	private const int JumpUpTime = 3;

	public Sprite CNTitleSprite;

	public Sprite ENTitleSprite;

	private Dictionary<int, Image> roomIndexToImg;

	private MapCharacterCtrl character;

	private Transform enterPoint;

	private Transform startPoint;

	private Action callback;

	private Image targetImg;

	private int currentLevel;

	private int currentLayer;

	private GameObject hiddenRoomObject;

	public override string UIViewName => "RoomSwitchUI";

	public override string UILayerName => "NormalLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		SingletonDontDestroy<AudioManager>.Instance.PauseMainBGM();
		Check(Singleton<GameManager>.Instance.CurrentMapLevel, Singleton<GameManager>.Instance.CurrentMapLayer);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		callback = null;
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		Transform transform = base.transform.Find("Bg/Point_Root");
		roomIndexToImg = new Dictionary<int, Image>(9);
		roomIndexToImg[11] = transform.Find("Room1_1").GetComponent<Image>();
		roomIndexToImg[12] = transform.Find("Room1_2").GetComponent<Image>();
		roomIndexToImg[13] = transform.Find("Room1_3").GetComponent<Image>();
		roomIndexToImg[21] = transform.Find("Room2_1").GetComponent<Image>();
		roomIndexToImg[22] = transform.Find("Room2_2").GetComponent<Image>();
		roomIndexToImg[23] = transform.Find("Room2_3").GetComponent<Image>();
		roomIndexToImg[31] = transform.Find("Room3_1").GetComponent<Image>();
		roomIndexToImg[32] = transform.Find("Room3_2").GetComponent<Image>();
		roomIndexToImg[33] = transform.Find("Room3_3").GetComponent<Image>();
		roomIndexToImg[41] = base.transform.Find("Bg/HiddenRoom/Room4_1").GetComponent<Image>();
		roomIndexToImg[42] = base.transform.Find("Bg/HiddenRoom/Room4_2").GetComponent<Image>();
		hiddenRoomObject = base.transform.Find("Bg/HiddenRoom").gameObject;
		character = base.transform.Find("Bg/Character").GetComponent<MapCharacterCtrl>();
		enterPoint = base.transform.Find("Bg/EnterPoint");
		startPoint = base.transform.Find("Bg/StartPoint");
		Localization();
	}

	public void Localization()
	{
		base.transform.Find("Bg/TitleImg").GetComponent<Image>().sprite = ((SingletonDontDestroy<SettingManager>.Instance.Language == 0) ? CNTitleSprite : ENTitleSprite);
	}

	private void UnlockHiddenRoom()
	{
		hiddenRoomObject.SetActive(value: true);
	}

	private void LockHiddenRoom()
	{
		hiddenRoomObject.SetActive(value: false);
	}

	public void InitMap()
	{
		foreach (KeyValuePair<int, Image> item in roomIndexToImg)
		{
			item.Value.gameObject.SetActive(value: false);
		}
		character.transform.position = enterPoint.position;
	}

	public void StartMap(Action callback)
	{
		this.callback = callback;
		StartCoroutine(Start_IE());
	}

	private IEnumerator Start_IE()
	{
		int i = 1;
		while (i <= 3)
		{
			Vector3 targetPos = Vector3.Lerp(enterPoint.position, startPoint.position, (float)i / 3f);
			bool isJumping = true;
			character.JumpToPosition(targetPos, delegate
			{
				isJumping = false;
			});
			while (isJumping)
			{
				yield return null;
			}
			int num = i + 1;
			i = num;
		}
		yield return new WaitForSeconds(1f);
		targetImg = roomIndexToImg[11];
		character.JumpToPosition(roomIndexToImg[11].transform.position, HideUI);
	}

	private void Check(int level, int layer)
	{
		foreach (KeyValuePair<int, Image> item in roomIndexToImg)
		{
			item.Value.gameObject.SetActive(value: false);
		}
		for (int i = 1; i <= level; i++)
		{
			if (i == level)
			{
				for (int j = 1; j < layer; j++)
				{
					int key = i * 10 + j;
					roomIndexToImg[key].gameObject.SetActive(value: true);
				}
			}
			else
			{
				for (int k = 1; k <= 3; k++)
				{
					int key2 = i * 10 + k;
					roomIndexToImg[key2].gameObject.SetActive(value: true);
				}
			}
		}
		if (level == 1 && layer == 1)
		{
			targetImg = null;
			character.transform.position = enterPoint.position;
		}
		else
		{
			targetImg = ((layer == 1) ? roomIndexToImg[(level - 1) * 10 + 3] : roomIndexToImg[level * 10 + (layer - 1)]);
			character.transform.position = targetImg.transform.position;
		}
		if (level == 4)
		{
			UnlockHiddenRoom();
		}
		else
		{
			LockHiddenRoom();
		}
	}

	public void SwitchToNextRoom(int level, int layer, Action callback)
	{
		if (level == 1 && layer == 1)
		{
			ResetSwitchUI();
		}
		if (level == 4 && layer == 1)
		{
			UnlockHiddenRoom();
		}
		currentLevel = level;
		currentLayer = layer;
		targetImg.gameObject.SetActive(value: true);
		this.callback = callback;
		int key = level * 10 + layer;
		targetImg = roomIndexToImg[key];
		character.JumpToPosition(roomIndexToImg[key].transform.position, HideUI);
	}

	private void ResetSwitchUI()
	{
		LockHiddenRoom();
		character.transform.position = enterPoint.position;
	}

	private void HideUI()
	{
		MaskUI maskUi = SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI;
		maskUi.ShowMask(delegate
		{
			callback?.Invoke();
			maskUi.ShowFade(null);
			SingletonDontDestroy<UIManager>.Instance.HideView(this);
		});
	}
}
