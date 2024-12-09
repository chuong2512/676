using System;
using System.Collections.Generic;
using UnityEngine;

public class SuitHandler
{
	[Serializable]
	public struct Suits
	{
		public List<SuitInfo> AllSuits;
	}

	[Serializable]
	public struct SuitInfo
	{
		public string SuitType;

		public string SuitIconName;

		public List<string> SuitEquips;

		public int[] SuitNeedAmount;

		public string[] SuitContentKeys;

		public List<KeyValuePair> SuitKeys;
	}

	public static readonly Color ContainColor = Color.white;

	public static readonly Color LackColor = "5A4024FF".HexColorToColor();

	public static readonly Color LackColor_White = "735434FF".HexColorToColor();

	private static bool IsInitSuit;

	private static Dictionary<SuitType, SuitInfo> AllSuits;

	private Player player;

	private PlayerEquipment playerEquipment;

	private Dictionary<SuitType, Suit> allSuits;

	public SuitHandler(Player player, PlayerEquipment playerEquipment)
	{
		this.player = player;
		this.playerEquipment = playerEquipment;
		LoadSuitInfo();
		allSuits = new Dictionary<SuitType, Suit>
		{
			{
				SuitType.YZ_Knight,
				new YZKnightSuit()
			},
			{
				SuitType.TW_Knight,
				new TWKnightSuit()
			},
			{
				SuitType.LW_Knight,
				new LWKnightSuit()
			},
			{
				SuitType.ST_Knight,
				new STKnightSuit()
			},
			{
				SuitType.NatureHuge,
				new NatureHugeSuit()
			},
			{
				SuitType.FlameDevour,
				new FlameDevourSuit()
			},
			{
				SuitType.Thief,
				new ThiefSuit()
			},
			{
				SuitType.RockWalker,
				new RockWalkerSuit()
			},
			{
				SuitType.OceanSpirit,
				new OceanSpiritSuit()
			},
			{
				SuitType.ImmortalFlame,
				new ImmortalFlameSuit()
			},
			{
				SuitType.KnightProphesy,
				new KnightProphesySuit()
			},
			{
				SuitType.ArrowCreator,
				new ArrowCreatorSuit()
			},
			{
				SuitType.Sniper,
				new SniperSuit()
			},
			{
				SuitType.PoisonShooter,
				new PoisonShooterSuit()
			},
			{
				SuitType.PhantomRanger,
				new PhantomRangerSuit()
			}
		};
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnBattleStart(EventData data)
	{
		foreach (KeyValuePair<SuitType, Suit> allSuit in allSuits)
		{
			allSuit.Value.OnBattleStart();
		}
	}

	private static void LoadSuitInfo()
	{
		if (IsInitSuit)
		{
			return;
		}
		IsInitSuit = true;
		Suits suits = JsonUtility.FromJson<Suits>("SuitData.json".GetAllPlatformStreamingAssetsData());
		AllSuits = new Dictionary<SuitType, SuitInfo>();
		for (int i = 0; i < suits.AllSuits.Count; i++)
		{
			if (Enum.TryParse<SuitType>(suits.AllSuits[i].SuitType, out var result))
			{
				AllSuits.Add(result, suits.AllSuits[i]);
			}
		}
	}

	public static SuitInfo GetSuitInfo(SuitType suitType)
	{
		LoadSuitInfo();
		return AllSuits[suitType];
	}

	public HashSet<string> GetContainSuits(SuitType suitType)
	{
		if (!allSuits.TryGetValue(suitType, out var value))
		{
			return null;
		}
		return value.SuitInfomation;
	}

	public void AddSuit(SuitType suitType, string equipCode)
	{
		if (!allSuits.ContainsKey(suitType))
		{
			throw new KeyNotFoundException($"All suit not contain {suitType}, please add suit class handly");
		}
		allSuits[suitType].AddSuit(equipCode);
	}

	public void RemoveSuit(SuitType suitType, string equipCode)
	{
		allSuits[suitType].RemoveSuit(equipCode);
	}
}
