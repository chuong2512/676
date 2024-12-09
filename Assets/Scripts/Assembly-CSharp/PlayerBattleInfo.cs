using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleInfo
{
	public const int MaxHandCardsAmount = 8;

	public const int MinEquipHandCardsAmount = 12;

	private Player player;

	private List<string> mainHandCardsPile;

	private List<string> mainHandDiscardsPile;

	private List<string> supHandCardsPile;

	private List<string> supHandDiscardsPile;

	public HashSet<BaseGift> allGifts;

	public Dictionary<string, int> AllEquipedMainHandCards { get; set; }

	public int EquipedMainHandCardAmount
	{
		get
		{
			if (AllEquipedMainHandCards == null || AllEquipedMainHandCards.Count == 0)
			{
				return 0;
			}
			int num = 0;
			foreach (KeyValuePair<string, int> allEquipedMainHandCard in AllEquipedMainHandCards)
			{
				num += allEquipedMainHandCard.Value;
			}
			return num;
		}
	}

	public Dictionary<string, int> AllEquipedSupHandCards { get; set; }

	public int EquipedSupHandCardAmount
	{
		get
		{
			if (AllEquipedSupHandCards == null || AllEquipedSupHandCards.Count == 0)
			{
				return 0;
			}
			int num = 0;
			foreach (KeyValuePair<string, int> allEquipedSupHandCard in AllEquipedSupHandCards)
			{
				num += allEquipedSupHandCard.Value;
			}
			return num;
		}
	}

	public List<string> MainHandCards { get; private set; }

	public int MainHandCardAmount { get; private set; }

	public List<string> SupHandCards { get; private set; }

	public int SupHandCardAmount { get; private set; }

	public List<string> CurrentSkillList { get; set; }

	public int MainHandCardsPileAmount => mainHandCardsPile.Count;

	public int MainHandDiscardPileAmount => mainHandDiscardsPile.Count;

	public int SupHandCardsPileAmount => supHandCardsPile.Count;

	public int SupHandDiscardPileAmount => supHandDiscardsPile.Count;

	public int PlayerCurrentCardAmount => MainHandCardAmount + SupHandCardAmount;

	public PlayerBattleInfo(Player player)
	{
		this.player = player;
		AllEquipedMainHandCards = new Dictionary<string, int>();
		AllEquipedSupHandCards = new Dictionary<string, int>();
		MainHandCards = new List<string>();
		SupHandCards = new List<string>();
		CurrentSkillList = new List<string>();
		mainHandCardsPile = new List<string>();
		mainHandDiscardsPile = new List<string>();
		supHandCardsPile = new List<string>();
		supHandDiscardsPile = new List<string>();
		allGifts = new HashSet<BaseGift>();
	}

	public void SetPlayerInitBattleInfo(OccupationInitSetting initSetting, CardPresuppositionStruct cardPresupposition)
	{
		SetPlayerInitHandCards(cardPresupposition);
		SetPlayerInitSkills(initSetting);
	}

	public void LoadPlayerBattleSaveInfo(PlayerBattleSaveInfo saveInfo)
	{
		foreach (KeyValuePair<string, int> allEquipedMainHandCard in saveInfo.allEquipedMainHandCards)
		{
			EquipMainHandCard(allEquipedMainHandCard.Key, allEquipedMainHandCard.Value);
		}
		foreach (KeyValuePair<string, int> allEquipedSupHandCard in saveInfo.allEquipedSupHandCards)
		{
			EquipSupHandCard(allEquipedSupHandCard.Key, allEquipedSupHandCard.Value);
		}
		LoadGift(saveInfo.allGift);
		CurrentSkillList = saveInfo.allEquipedSkills;
	}

	private void SetPlayerInitHandCards(CardPresuppositionStruct cardPresupposition)
	{
		foreach (KeyValuePair<string, int> mainHandcard in cardPresupposition.MainHandcards)
		{
			EquipMainHandCard(mainHandcard.Key, mainHandcard.Value);
			player.PlayerInventory.ReduceMainHandCards(mainHandcard.Key, mainHandcard.Value);
		}
		foreach (KeyValuePair<string, int> supHandCard in cardPresupposition.SupHandCards)
		{
			EquipSupHandCard(supHandCard.Key, supHandCard.Value);
			player.PlayerInventory.ReduceSupHandCards(supHandCard.Key, supHandCard.Value);
		}
	}

	private void SetPlayerInitSkills(OccupationInitSetting initSetting)
	{
		CurrentSkillList.Clear();
		if (initSetting.InitSkillArray != null)
		{
			for (int i = 0; i < initSetting.InitSkillArray.Length; i++)
			{
				EquipInitSkill(initSetting.InitSkillArray[i]);
			}
		}
	}

	public void GetGift(BaseGift gift)
	{
		if (!allGifts.Contains(gift))
		{
			allGifts.Add(gift);
			GiftManager.Instace.PlayerGetThisGift(gift);
			CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
			if (!characterInfoUI.IsNull())
			{
				characterInfoUI.AddGift(gift);
			}
		}
	}

	public void LoadGift(List<int> gifts)
	{
		allGifts = new HashSet<BaseGift>();
		for (int i = 0; i < gifts.Count; i++)
		{
			BaseGift.GiftName name = (BaseGift.GiftName)gifts[i];
			GiftManager.Instace.GetSpecificGift(name, out var gift);
			allGifts.Add(gift);
			GiftManager.Instace.PlayerGetThisGift(gift);
			CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
			if (!characterInfoUI.IsNull())
			{
				characterInfoUI.AddGift(gift);
			}
		}
	}

	public List<int> GetGiftList()
	{
		List<int> list = new List<int>();
		foreach (BaseGift allGift in allGifts)
		{
			list.Add((int)allGift.Name);
		}
		return list;
	}

	public void OnEffectAllGifts()
	{
		foreach (BaseGift allGift in allGifts)
		{
			allGift.OnBattleStart();
		}
		allGifts.Clear();
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.ForceGetView("CharacterInfoUI") as CharacterInfoUI;
		if (!characterInfoUI.IsNull())
		{
			characterInfoUI.RemoveAllGift();
		}
	}

	public void EquipSkill(string skillCode, int index)
	{
		if (CurrentSkillList.Contains(skillCode))
		{
			Debug.LogError("The skill ever add in skill list");
		}
		CurrentSkillList[index] = skillCode;
	}

	public void EquipInitSkill(string skillCode)
	{
		CurrentSkillList.Add(skillCode);
	}

	public void SwapSkill(int index1, int index2)
	{
		string value = CurrentSkillList[index1];
		CurrentSkillList[index1] = CurrentSkillList[index2];
		CurrentSkillList[index2] = value;
	}

	public void ReleaseSkill(string skillCode)
	{
		for (int i = 0; i < CurrentSkillList.Count; i++)
		{
			if (CurrentSkillList[i] == skillCode)
			{
				CurrentSkillList[i] = string.Empty;
				break;
			}
		}
	}

	public void ReduceSkillAmount(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			string text = CurrentSkillList[CurrentSkillList.Count - 1];
			CurrentSkillList.RemoveAt(CurrentSkillList.Count - 1);
			if (!text.IsNullOrEmpty())
			{
				player.PlayerInventory.ReleaseSkill(text);
			}
		}
	}

	public void AddSkillAmount(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			CurrentSkillList.Add(string.Empty);
		}
	}

	public bool IsEquipedSkill(string skillCode)
	{
		return CurrentSkillList.Contains(skillCode);
	}

	public void EquipMainHandCard(string cardCode, int amount)
	{
		if (AllEquipedMainHandCards.ContainsKey(cardCode))
		{
			AllEquipedMainHandCards[cardCode] += amount;
			return;
		}
		AllEquipedMainHandCards[cardCode] = amount;
		FactoryManager.GetUsualCard(cardCode).EquipCard();
	}

	public bool ReleaseMainHandCard(string cardCode, int amount)
	{
		if (!AllEquipedMainHandCards.ContainsKey(cardCode) || AllEquipedMainHandCards[cardCode] < amount)
		{
			return false;
		}
		AllEquipedMainHandCards[cardCode] -= amount;
		if (AllEquipedMainHandCards[cardCode] == 0)
		{
			AllEquipedMainHandCards.Remove(cardCode);
			FactoryManager.GetUsualCard(cardCode).ReleaseCard();
		}
		return true;
	}

	public void EquipSupHandCard(string cardCode, int amount)
	{
		if (AllEquipedSupHandCards.ContainsKey(cardCode))
		{
			AllEquipedSupHandCards[cardCode] += amount;
			return;
		}
		AllEquipedSupHandCards[cardCode] = amount;
		FactoryManager.GetUsualCard(cardCode).EquipCard();
	}

	public bool ReleaseSupHandCard(string cardCode, int amount)
	{
		if (!AllEquipedSupHandCards.ContainsKey(cardCode) || AllEquipedSupHandCards[cardCode] < amount)
		{
			return false;
		}
		AllEquipedSupHandCards[cardCode] -= amount;
		if (AllEquipedSupHandCards[cardCode] == 0)
		{
			AllEquipedSupHandCards.Remove(cardCode);
			FactoryManager.GetUsualCard(cardCode).ReleaseCard();
		}
		return true;
	}

	public void StartBattle()
	{
		ShuffleAllCards();
		EventManager.BroadcastEvent(EventEnum.E_OnShuffleAllCards, null);
		DrawMainHandCards(player.PlayerAttr.DrawCardAmount);
		DrawSupHandCards(player.PlayerAttr.DrawCardAmount);
		int num2 = (MainHandCardAmount = (SupHandCardAmount = player.PlayerAttr.DrawCardAmount));
		((BattleUI)SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI")).LoadSkill(CurrentSkillList);
	}

	public void DrawCardWhenStoringForce()
	{
		TryDrawMainHandCards(player.PlayerAttr.DrawCardAmount);
		TryDrawSupHandCards(player.PlayerAttr.DrawCardAmount);
	}

	public void ClearMainHandCards(List<string> cardsKeep, HashSet<string> cardsKeepType)
	{
		if ((cardsKeep.IsNull() || cardsKeep.Count == 0) && (cardsKeepType == null || cardsKeepType.Count == 0))
		{
			for (int i = 0; i < MainHandCards.Count; i++)
			{
				mainHandDiscardsPile.Add(MainHandCards[i]);
			}
			MainHandCards.Clear();
			MainHandCardAmount = 0;
		}
		else
		{
			List<string> list = new List<string>(cardsKeep);
			List<string> list2 = new List<string>(8);
			for (int j = 0; j < MainHandCards.Count; j++)
			{
				if (list != null && list.Contains(MainHandCards[j]))
				{
					list2.Add(MainHandCards[j]);
					list.Remove(MainHandCards[j]);
				}
				else if (cardsKeepType != null && cardsKeepType.Contains(MainHandCards[j]))
				{
					list2.Add(MainHandCards[j]);
				}
				else
				{
					mainHandDiscardsPile.Add(MainHandCards[j]);
				}
			}
			MainHandCards = list2;
			MainHandCardAmount = list2.Count;
		}
		OnPlayerHandCardChanged();
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).RecycleAllMainHandCardControllers(cardsKeep, cardsKeepType);
	}

	public void ClearMainHandCards(HashSet<string> cardsKeep)
	{
		if (cardsKeep.IsNull() || cardsKeep.Count == 0)
		{
			for (int i = 0; i < MainHandCards.Count; i++)
			{
				mainHandDiscardsPile.Add(MainHandCards[i]);
			}
			MainHandCards.Clear();
			MainHandCardAmount = 0;
		}
		else
		{
			List<string> list = new List<string>(8);
			for (int j = 0; j < MainHandCards.Count; j++)
			{
				if (cardsKeep.Contains(MainHandCards[j]))
				{
					list.Add(MainHandCards[j]);
				}
				else
				{
					mainHandDiscardsPile.Add(MainHandCards[j]);
				}
			}
			MainHandCards = list;
			MainHandCardAmount = list.Count;
		}
		OnPlayerHandCardChanged();
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).RecycleAllMainHandCardControllers(null, cardsKeep);
	}

	public void ClearMainHandAllPiles()
	{
		mainHandCardsPile.Clear();
		mainHandDiscardsPile.Clear();
	}

	public void ClearSupHandCards(HashSet<string> cardsKeep)
	{
		if (cardsKeep.IsNull() || cardsKeep.Count == 0)
		{
			for (int i = 0; i < SupHandCards.Count; i++)
			{
				supHandDiscardsPile.Add(SupHandCards[i]);
			}
			SupHandCards.Clear();
			SupHandCardAmount = 0;
		}
		else
		{
			List<string> list = new List<string>(8);
			for (int j = 0; j < SupHandCards.Count; j++)
			{
				if (cardsKeep.Contains(SupHandCards[j]))
				{
					list.Add(SupHandCards[j]);
				}
				else
				{
					supHandDiscardsPile.Add(SupHandCards[j]);
				}
			}
			SupHandCards = list;
			SupHandCardAmount = SupHandCards.Count;
		}
		OnPlayerHandCardChanged();
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).RecycleAllSupHandCardControllers(null, cardsKeep);
	}

	public void ClearSupHandCards(List<string> cardsKeep, HashSet<string> cardsKeepType)
	{
		if ((cardsKeep.IsNull() || cardsKeep.Count == 0) && (cardsKeepType == null || cardsKeepType.Count == 0))
		{
			for (int i = 0; i < SupHandCards.Count; i++)
			{
				supHandDiscardsPile.Add(SupHandCards[i]);
			}
			SupHandCards.Clear();
			SupHandCardAmount = 0;
		}
		else
		{
			List<string> list = new List<string>(cardsKeep);
			List<string> list2 = new List<string>(8);
			for (int j = 0; j < SupHandCards.Count; j++)
			{
				if (list != null && list.Contains(SupHandCards[j]))
				{
					list2.Add(SupHandCards[j]);
					list.Remove(SupHandCards[j]);
				}
				else if (cardsKeepType != null && cardsKeepType.Contains(SupHandCards[j]))
				{
					list2.Add(SupHandCards[j]);
				}
				else
				{
					supHandDiscardsPile.Add(SupHandCards[j]);
				}
			}
			SupHandCards = list2;
			SupHandCardAmount = list2.Count;
		}
		OnPlayerHandCardChanged();
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).RecycleAllSupHandCardControllers(cardsKeep, cardsKeepType);
	}

	public void ClearSupHandAllPiles()
	{
		supHandCardsPile.Clear();
		supHandDiscardsPile.Clear();
	}

	public void RandomDropHandCard()
	{
		bool flag = MainHandCardAmount > 0;
		bool flag2 = SupHandCardAmount > 0;
		if (!flag && !flag2)
		{
			return;
		}
		if (flag && flag2)
		{
			if (Random.value < 0.5f)
			{
				RandomDropMainHandCard();
			}
			else
			{
				RandomDropSupHandCard();
			}
		}
		else if (flag && !flag2)
		{
			RandomDropMainHandCard();
		}
		else
		{
			RandomDropSupHandCard();
		}
	}

	public void RandomDropMainHandCard()
	{
		if (MainHandCards.Count > 0)
		{
			BattleUI obj = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			string text = MainHandCards[Random.Range(0, MainHandCards.Count)];
			ComsumeMainHandCards(text, isDrop: true);
			obj.PlayerDropACard(text, isMain: true);
		}
	}

	public void RandomDropSupHandCard()
	{
		if (SupHandCards.Count > 0)
		{
			BattleUI obj = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			string text = SupHandCards[Random.Range(0, SupHandCards.Count)];
			ComsumeSupHandCards(text, isDrop: true);
			obj.PlayerDropACard(text, isMain: false);
		}
	}

	public void DropSuphandCard(string cardCode)
	{
		ComsumeSupHandCards(cardCode, isDrop: true);
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).PlayerDropACard(cardCode, isMain: false);
	}

	private void ShuffleAllCards()
	{
		ShuffleInitMainCards();
		ShuffleInitSupCards();
	}

	public void ShuffleInitMainCards()
	{
		if (AllEquipedMainHandCards == null || AllEquipedMainHandCards.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, int> allEquipedMainHandCard in AllEquipedMainHandCards)
		{
			for (int i = 0; i < allEquipedMainHandCard.Value; i++)
			{
				mainHandCardsPile.Add(allEquipedMainHandCard.Key);
			}
		}
		if (mainHandCardsPile.Count >= 8)
		{
			mainHandCardsPile = mainHandCardsPile.RandomListSort();
		}
		else
		{
			mainHandCardsPile.Clear();
		}
	}

	public void ShuffleInitSupCards()
	{
		if (AllEquipedSupHandCards == null || AllEquipedSupHandCards.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, int> allEquipedSupHandCard in AllEquipedSupHandCards)
		{
			for (int i = 0; i < allEquipedSupHandCard.Value; i++)
			{
				supHandCardsPile.Add(allEquipedSupHandCard.Key);
			}
		}
		if (supHandCardsPile.Count >= 8)
		{
			supHandCardsPile = supHandCardsPile.RandomListSort();
		}
		else
		{
			supHandCardsPile.Clear();
		}
	}

	public void TurnAllMainHandSpecificCard(string originCardCode, string targetCardCode)
	{
		for (int i = 0; i < mainHandCardsPile.Count; i++)
		{
			if (mainHandCardsPile[i] == originCardCode)
			{
				mainHandCardsPile[i] = targetCardCode;
			}
		}
	}

	private void ShuffleMainHandCards()
	{
		for (int i = 0; i < mainHandDiscardsPile.Count; i++)
		{
			mainHandCardsPile.Add(mainHandDiscardsPile[i]);
		}
		mainHandDiscardsPile.Clear();
		mainHandCardsPile = mainHandCardsPile.RandomListSort();
	}

	private void ShuffleSupHandCards()
	{
		for (int i = 0; i < supHandDiscardsPile.Count; i++)
		{
			supHandCardsPile.Add(supHandDiscardsPile[i]);
		}
		supHandDiscardsPile.Clear();
		supHandCardsPile = supHandCardsPile.RandomListSort();
	}

	public void TryDrawMainHandCards(int amount)
	{
		if (PlayerCurrentCardAmount == 0)
		{
			(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).SetStoringForceBtnHighlight(isHighlight: false);
		}
		int b = 8 - MainHandCardAmount;
		int num = Mathf.Min(amount, b);
		if (num > 0)
		{
			MainHandCardAmount += num;
			EventManager.BroadcastEvent(EventEnum.E_PlayerDrawCard, new SimpleEventData
			{
				intValue = num
			});
			DrawMainHandCards(num);
		}
	}

	private void DrawMainHandCards(int amount)
	{
		if (mainHandCardsPile == null || (mainHandCardsPile.Count == 0 && mainHandDiscardsPile.Count == 0))
		{
			return;
		}
		BattleUI battleUi = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		if (amount <= mainHandCardsPile.Count)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < amount; i++)
			{
				int index = mainHandCardsPile.Count - 1;
				string item = mainHandCardsPile[index];
				mainHandCardsPile.RemoveAt(index);
				MainHandCards.Add(item);
				list.Add(item);
			}
			battleUi.AddMainHandCard(list, null);
		}
		else
		{
			int remain = amount - mainHandCardsPile.Count;
			if (mainHandCardsPile.Count > 0)
			{
				List<string> list2 = new List<string>();
				for (int j = 0; j < mainHandCardsPile.Count; j++)
				{
					MainHandCards.Add(mainHandCardsPile[j]);
					list2.Add(mainHandCardsPile[j]);
				}
				mainHandCardsPile.Clear();
				battleUi.AddMainHandCard(list2, delegate
				{
					battleUi.ShuffleMainHandCard(delegate
					{
						ShuffleMainHandCards();
						DrawMainHandCards(remain);
					});
				});
			}
			else
			{
				battleUi.ShuffleMainHandCard(delegate
				{
					DrawMainHandCards(remain);
				});
				ShuffleMainHandCards();
			}
		}
		OnPlayerHandCardChanged();
	}

	public void TryDrawSupHandCards(int amount)
	{
		if (PlayerCurrentCardAmount == 0)
		{
			(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).SetStoringForceBtnHighlight(isHighlight: false);
		}
		int b = 8 - SupHandCardAmount;
		int num = Mathf.Min(amount, b);
		if (num > 0)
		{
			SupHandCardAmount += num;
			EventManager.BroadcastEvent(EventEnum.E_PlayerDrawCard, new SimpleEventData
			{
				intValue = num
			});
			DrawSupHandCards(num);
		}
	}

	private void DrawSupHandCards(int amount)
	{
		if (supHandCardsPile == null || (supHandCardsPile.Count == 0 && supHandDiscardsPile.Count == 0))
		{
			return;
		}
		BattleUI battleUi = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		if (amount <= supHandCardsPile.Count)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < amount; i++)
			{
				int index = supHandCardsPile.Count - 1;
				string item = supHandCardsPile[index];
				supHandCardsPile.RemoveAt(index);
				SupHandCards.Add(item);
				list.Add(item);
			}
			battleUi.AddSupHandCard(list, null);
		}
		else
		{
			int remain = amount - supHandCardsPile.Count;
			if (supHandCardsPile.Count > 0)
			{
				List<string> list2 = new List<string>();
				for (int j = 0; j < supHandCardsPile.Count; j++)
				{
					SupHandCards.Add(supHandCardsPile[j]);
					list2.Add(supHandCardsPile[j]);
				}
				supHandCardsPile.Clear();
				battleUi.AddSupHandCard(list2, delegate
				{
					battleUi.ShuffleSupHandCard(delegate
					{
						ShuffleSupHandCards();
						DrawSupHandCards(remain);
					});
				});
			}
			else
			{
				battleUi.ShuffleSupHandCard(delegate
				{
					DrawSupHandCards(remain);
				});
				ShuffleSupHandCards();
			}
		}
		OnPlayerHandCardChanged();
	}

	public void PutSpecificCardsIntoMainHand(string cardCode, int amount)
	{
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		List<string> list = new List<string>();
		for (int i = 0; i < amount; i++)
		{
			MainHandCards.Add(cardCode);
			list.Add(cardCode);
		}
		battleUI.AddMainHandCard(list, null);
		MainHandCardAmount = MainHandCards.Count;
		OnPlayerHandCardChanged();
	}

	public void PutSpecificCardsIntoSupHand(string cardCode, int amount)
	{
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		List<string> list = new List<string>();
		for (int i = 0; i < amount; i++)
		{
			SupHandCards.Add(cardCode);
			list.Add(cardCode);
		}
		battleUI.AddSupHandCard(list, null);
		SupHandCardAmount = SupHandCards.Count;
		OnPlayerHandCardChanged();
	}

	public bool EnoughMainCards(string cardCode, int amount)
	{
		int num = 0;
		for (int i = 0; i < MainHandCards.Count; i++)
		{
			if (MainHandCards[i] == cardCode)
			{
				num++;
			}
		}
		if (num >= amount)
		{
			return true;
		}
		return false;
	}

	public bool EnoughSupCards(string cardCode, int amount)
	{
		int num = 0;
		for (int i = 0; i < SupHandCards.Count; i++)
		{
			if (SupHandCards[i] == cardCode)
			{
				num++;
			}
		}
		if (num >= amount)
		{
			return true;
		}
		return false;
	}

	private void OnPlayerHandCardChanged()
	{
		if (PlayerCurrentCardAmount == 0 && !player.IsPlayerEverStoringForce)
		{
			(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).SetStoringForceBtnHighlight(isHighlight: true);
		}
		else if (PlayerCurrentCardAmount > 0)
		{
			(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).SetStoringForceBtnHighlight(isHighlight: false);
		}
		EventManager.BroadcastEvent(EventEnum.E_PlayerHandCardChanged, null);
	}

	public void EndBattle()
	{
		MainHandCards.Clear();
		SupHandCards.Clear();
		mainHandCardsPile.Clear();
		mainHandDiscardsPile.Clear();
		supHandCardsPile.Clear();
		supHandDiscardsPile.Clear();
		int num3 = (MainHandCardAmount = (SupHandCardAmount = 0));
	}

	public void ComsumeMainHandCards(string card, bool isDrop)
	{
		bool flag = false;
		for (int i = 0; i < MainHandCards.Count; i++)
		{
			if (MainHandCards[i] == card)
			{
				MainHandCards.RemoveAt(i);
				flag = true;
				MainHandCardAmount--;
				break;
			}
		}
		if (flag)
		{
			OnPlayerHandCardChanged();
			UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(card);
			if (isDrop || !usualCardAttr.IsComsumeableCard)
			{
				mainHandDiscardsPile.Add(card);
			}
		}
	}

	public void ComsumeMainHandCards(string card, int amount, bool isDrop)
	{
		for (int i = 0; i < amount; i++)
		{
			ComsumeMainHandCards(card, isDrop: false);
		}
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).RemoveMainHandCard(card, amount, isDrop);
	}

	public void ComsumeSupHandCards(string card, bool isDrop)
	{
		bool flag = false;
		for (int i = 0; i < SupHandCards.Count; i++)
		{
			if (SupHandCards[i] == card)
			{
				SupHandCards.RemoveAt(i);
				flag = true;
				SupHandCardAmount--;
				break;
			}
		}
		if (flag)
		{
			OnPlayerHandCardChanged();
			UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(card);
			if (isDrop || !usualCardAttr.IsComsumeableCard)
			{
				supHandDiscardsPile.Add(card);
			}
		}
	}

	public void ComsumeSupHandCards(string card, int amount, bool isDrop)
	{
		for (int i = 0; i < amount; i++)
		{
			ComsumeSupHandCards(card, isDrop: false);
		}
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).RemoveSupHandCard(card, amount, isDrop);
	}

	public Dictionary<string, int> GetMainHandRemainCardsWithAmount()
	{
		for (int i = 0; i < mainHandCardsPile.Count; i++)
		{
			Debug.Log(mainHandCardsPile[i]);
		}
		if (mainHandCardsPile.Count > 0)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			for (int j = 0; j < mainHandCardsPile.Count; j++)
			{
				if (dictionary.ContainsKey(mainHandCardsPile[j]))
				{
					dictionary[mainHandCardsPile[j]]++;
				}
				else
				{
					dictionary[mainHandCardsPile[j]] = 1;
				}
			}
			return dictionary;
		}
		return null;
	}

	public Dictionary<string, int> GetMainHandDiscardCardsWithAmount()
	{
		if (mainHandDiscardsPile.Count > 0)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			for (int i = 0; i < mainHandDiscardsPile.Count; i++)
			{
				if (dictionary.ContainsKey(mainHandDiscardsPile[i]))
				{
					dictionary[mainHandDiscardsPile[i]]++;
				}
				else
				{
					dictionary[mainHandDiscardsPile[i]] = 1;
				}
			}
			return dictionary;
		}
		return null;
	}

	public Dictionary<string, int> GetSupHandRemainCardsWithAmount()
	{
		if (supHandCardsPile.Count > 0)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			for (int i = 0; i < supHandCardsPile.Count; i++)
			{
				if (dictionary.ContainsKey(supHandCardsPile[i]))
				{
					dictionary[supHandCardsPile[i]]++;
				}
				else
				{
					dictionary[supHandCardsPile[i]] = 1;
				}
			}
			return dictionary;
		}
		return null;
	}

	public Dictionary<string, int> GetSupHandDiscardCardsWithAmount()
	{
		if (supHandDiscardsPile.Count > 0)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			for (int i = 0; i < supHandDiscardsPile.Count; i++)
			{
				if (dictionary.ContainsKey(supHandDiscardsPile[i]))
				{
					dictionary[supHandDiscardsPile[i]]++;
				}
				else
				{
					dictionary[supHandDiscardsPile[i]] = 1;
				}
			}
			return dictionary;
		}
		return null;
	}

	public void ChangeSkillsInBattle_Cheat(List<string> skillsToChangedTo)
	{
		CurrentSkillList = skillsToChangedTo;
		BattleUI obj = (BattleUI)SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI");
		obj.RecycleAllBattleSkillSlot();
		obj.LoadSkill(CurrentSkillList);
	}
}
