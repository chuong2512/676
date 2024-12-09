using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDesPanelCtrl : MonoBehaviour
{
	private Text tmpContentText;

	private Text contentText;

	private Image skillIconImg;

	private Text skillNameText;

	private Text apCostText;

	private Image specialCostImg;

	private Text specialCostText;

	private Text mainHandCardComsumeText;

	private Text supHandCardComsumeText;

	private Button functionBtn;

	private Text functionBtnText;

	private Transform keyRoot;

	private RectTransform keyRectTrans;

	private Queue<KeyCtrl> allKeyPools = new Queue<KeyCtrl>();

	private HashSet<KeyCtrl> allShowingKeys = new HashSet<KeyCtrl>();

	private void Awake()
	{
		tmpContentText = base.transform.Find("Content").GetComponent<Text>();
		contentText = base.transform.Find("Content/Bg/Content").GetComponent<Text>();
		skillIconImg = base.transform.Find("Content/Bg/Icon").GetComponent<Image>();
		skillNameText = base.transform.Find("Content/Bg/NameText").GetComponent<Text>();
		apCostText = base.transform.Find("Content/Bg/ApCost/Amount").GetComponent<Text>();
		specialCostImg = base.transform.Find("Content/Bg/SpecialCost").GetComponent<Image>();
		specialCostText = specialCostImg.transform.Find("Amount").GetComponent<Text>();
		mainHandCardComsumeText = base.transform.Find("Content/Bg/MainHand/Amount").GetComponent<Text>();
		supHandCardComsumeText = base.transform.Find("Content/Bg/SupHand/Amount").GetComponent<Text>();
		keyRoot = base.transform.Find("KeyRoot");
		keyRectTrans = keyRoot.GetComponent<RectTransform>();
		functionBtn = base.transform.Find("Content/FunctionBtn").GetComponent<Button>();
		functionBtnText = functionBtn.transform.Find("Text").GetComponent<Text>();
	}

	private void OnDisable()
	{
		functionBtn.gameObject.SetActive(value: false);
		functionBtn.onClick.RemoveAllListeners();
	}

	public void ShowDescription(PlayerOccupation playerOccupation, string skillCode, bool isOnBattle)
	{
		SkillCardAttr skillCardAttr = DataManager.Instance.GetSkillCardAttr(playerOccupation, skillCode);
		skillIconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(skillCardAttr.IllustrationName, "Sprites/SkillIcon");
		SkillCard skillCard = FactoryManager.GetSkillCard(playerOccupation, skillCode);
		skillNameText.text = skillCard.CardName;
		string text3 = (tmpContentText.text = (contentText.text = (isOnBattle ? skillCard.GetOnBattleDes(Singleton<GameManager>.Instance.Player, isMain: false) : skillCard.CardNormalDes)));
		mainHandCardComsumeText.text = (skillCardAttr.MainHandCardCode.IsNullOrEmpty() ? string.Empty : $"{FactoryManager.GetUsualCard(skillCardAttr.MainHandCardCode).CardName}×{skillCardAttr.MainHandCardConsumeAmount}");
		supHandCardComsumeText.text = (skillCardAttr.SupHandCardCode.IsNullOrEmpty() ? string.Empty : $"{FactoryManager.GetUsualCard(skillCardAttr.SupHandCardCode).CardName}×{skillCardAttr.SupHandCardConsumeAmount}");
		int num = skillCard.ApCost;
		if (isOnBattle)
		{
			Singleton<GameManager>.Instance.Player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardApCostReduce, (BattleEffectData)new SimpleEffectData
			{
				strData = skillCard.CardCode
			}, out int IntData);
			num -= IntData;
			if (!Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(Singleton<GameManager>.Instance.Player, BuffType.Buff_Agile).IsNull())
			{
				num = 0;
			}
		}
		apCostText.text = $"× {num}";
		OccupationData occupationData = DataManager.Instance.GetOccupationData(playerOccupation);
		specialCostImg.gameObject.SetActive(value: true);
		specialCostImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(occupationData.BattleUISpecialAttrSprite, occupationData.DefaultSpritePath);
		specialCostText.text = ((skillCardAttr.SpecialAttrCost >= 0) ? $"× {skillCardAttr.SpecialAttrCost}" : string.Format("COMSUMEALLFAITH".LocalizeText(), occupationData.OccupationSpecialAttrDes.key.LocalizeText()));
		AddKey(skillCard.GetKeyDescription());
	}

	public void ShowDescription(PlayerOccupation playerOccupation, string skillCode, bool isOnBattle, string btnName, Action callBack, bool isInteractive)
	{
		functionBtn.interactable = isInteractive;
		functionBtn.gameObject.SetActive(value: true);
		functionBtn.onClick.AddListener(delegate
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
			callBack?.Invoke();
		});
		functionBtnText.text = btnName;
		ShowDescription(playerOccupation, skillCode, isOnBattle);
	}

	private void AddKey(List<KeyValuePair> keyDes)
	{
		RecycleAllKeys();
		if (keyDes.IsNull() || keyDes.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair keyDe in keyDes)
		{
			KeyCtrl key = GetKey();
			key.LoadKey(keyDe.key.LocalizeText(), keyDe.value.LocalizeText());
			allShowingKeys.Add(key);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(keyRectTrans);
	}

	private KeyCtrl GetKey()
	{
		if (allKeyPools.Count > 0)
		{
			KeyCtrl keyCtrl = allKeyPools.Dequeue();
			keyCtrl.gameObject.SetActive(value: true);
			return keyCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("KeyCtrl", "Prefabs", keyRoot).GetComponent<KeyCtrl>();
	}

	private void RecycleAllKeys()
	{
		if (allShowingKeys.Count <= 0)
		{
			return;
		}
		foreach (KeyCtrl allShowingKey in allShowingKeys)
		{
			allShowingKey.gameObject.SetActive(value: false);
			allKeyPools.Enqueue(allShowingKey);
		}
		allShowingKeys.Clear();
	}
}
