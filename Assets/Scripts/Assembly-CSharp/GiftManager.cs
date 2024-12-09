using System.Collections.Generic;
using UnityEngine;

public class GiftManager
{
	private static GiftManager _instance;

	private Dictionary<BaseGift.GiftName, BaseGift> allGifts;

	private List<BaseGift> allRandomBlessingGifts;

	private List<BaseGift> allRandomDamnationGifts;

	public static GiftManager Instace => _instance ?? (_instance = new GiftManager());

	private GiftManager()
	{
	}

	public void InitGiftManager()
	{
		allGifts = new Dictionary<BaseGift.GiftName, BaseGift>();
		allRandomBlessingGifts = new List<BaseGift>();
		allRandomDamnationGifts = new List<BaseGift>();
		foreach (BaseGift item in FactoryManager.GetAllGiftInstance())
		{
			GiftData giftDataByGiftName = DataManager.Instance.GetGiftDataByGiftName(item.Name);
			allGifts.Add(item.Name, item);
			if (giftDataByGiftName.IsCanBeRandom)
			{
				if (giftDataByGiftName.GiftType == BaseGift.GiftType.Blessing)
				{
					allRandomBlessingGifts.Add(item);
				}
				else
				{
					allRandomDamnationGifts.Add(item);
				}
			}
		}
	}

	public void PlayerGetThisGift(BaseGift gift)
	{
		GiftData giftDataByGiftName = DataManager.Instance.GetGiftDataByGiftName(gift.Name);
		allGifts.Remove(gift.Name);
		if (giftDataByGiftName.GiftType == BaseGift.GiftType.Damnation)
		{
			allRandomDamnationGifts.Remove(gift);
		}
		else
		{
			allRandomBlessingGifts.Remove(gift);
		}
	}

	public BaseGift GetRandomBlessingGift()
	{
		return allRandomBlessingGifts[Random.Range(0, allRandomBlessingGifts.Count)];
	}

	public BaseGift GetRandomDamnationGift()
	{
		return allRandomDamnationGifts[Random.Range(0, allRandomDamnationGifts.Count)];
	}

	public void GetSpecificGift(BaseGift.GiftName name, out BaseGift gift)
	{
		allGifts.TryGetValue(name, out gift);
	}

	public void OnGiftEffectOver(BaseGift gift)
	{
		allGifts[gift.Name] = gift;
		if (DataManager.Instance.GetGiftDataByGiftName(gift.Name).GiftType == BaseGift.GiftType.Damnation)
		{
			allRandomDamnationGifts.Add(gift);
		}
		else
		{
			allRandomBlessingGifts.Add(gift);
		}
	}
}
