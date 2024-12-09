using UnityEngine;

public class ProphesyCard_PC_13 : ProphesyCard
{
	public override string ProphesyCode => "PC_13";

	public override void Active(bool isLoad)
	{
		EventManager.RegisterEvent(EventEnum.E_PlayerLevelUp, OnPlayerLevelUp);
	}

	~ProphesyCard_PC_13()
	{
		EventManager.UnregisterEvent(EventEnum.E_PlayerLevelUp, OnPlayerLevelUp);
	}

	private void OnPlayerLevelUp(EventData data)
	{
		if (Random.value < 0.5f)
		{
			BaseGift randomDamnationGift = GiftManager.Instace.GetRandomDamnationGift();
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.GetGift(randomDamnationGift);
		}
		else
		{
			BaseGift randomBlessingGift = GiftManager.Instace.GetRandomBlessingGift();
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.GetGift(randomBlessingGift);
		}
	}
}
