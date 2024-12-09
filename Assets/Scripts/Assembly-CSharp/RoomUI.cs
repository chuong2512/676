using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : UIView
{
	private const string RoomBlockName = "RoomBlock";

	private const string ObstacleBlock = "ObstacleBlock";

	private const string RoomBlockPath = "Prefabs/Room";

	public const int BlockSize = 175;

	private Transform[] roomBlockRoots;

	private Transform bossBlockRoot;

	private RoomInfo currentRoomInfo;

	private GameObject blockBrokenObject;

	private Action blockBrokenAnimOverAction;

	private Text coinAmountText;

	private Outline coinAmountTextOutline;

	public Button bagBtn;

	private Tween bagBtnTransformHintTween;

	private Image bgImg;

	private Text roomInfoText;

	public Sprite[] allRoomLevelBgSprites;

	private TimePiecesCtrl _timePiecesCtrl;

	private int goldCacheAmount;

	private Dictionary<Vector2Int, BaseBlock> allShowingBaseBlocks = new Dictionary<Vector2Int, BaseBlock>();

	private Queue<RoomBlock> allRoomBlockPool = new Queue<RoomBlock>();

	private Dictionary<Vector2Int, RoomBlock> allShowingRoomBlocks = new Dictionary<Vector2Int, RoomBlock>();

	private Dictionary<Vector2Int, RoomBlock> allShowingEmptyBlocks = new Dictionary<Vector2Int, RoomBlock>();

	private Queue<ObstacleBlock> allObstacleBlockPool = new Queue<ObstacleBlock>();

	private List<ObstacleBlock> allShowingObstacleBlocks = new List<ObstacleBlock>();

	private NextRoomBlock normalNextRoomBlock;

	private NextRoomBlock bossNextRoomBlock;

	private EliteMonsterBlock eliteMonsterBlock;

	private Queue<NormalMonsterBlock> allNormalMonsterBlockPool = new Queue<NormalMonsterBlock>();

	private Dictionary<Vector2Int, NormalMonsterBlock> allShowingNormalMonsterBlocks = new Dictionary<Vector2Int, NormalMonsterBlock>();

	private BossBlock bossBlock;

	private EquipShopBlock equipShopBlock;

	private CardShopBlock cardShopBlock;

	private Queue<RandomEventBlock> allRandomEventBlockPool = new Queue<RandomEventBlock>();

	private Dictionary<Vector2Int, RandomEventBlock> allShowingRandomEventBlocks = new Dictionary<Vector2Int, RandomEventBlock>();

	private Dictionary<string, SpecialEventBlock> specialEventBlockPool = new Dictionary<string, SpecialEventBlock>();

	private Dictionary<string, SpecialEventBlock> allShowingSpecialEventBlocks = new Dictionary<string, SpecialEventBlock>();

	public Sprite NormalMonsterSpriteHint;

	public Sprite EliteMonsterSpriteHint;

	private Transform enemyHintRoot;

	private Dictionary<Vector2Int, Image> allShowingEnemyHints = new Dictionary<Vector2Int, Image>();

	private Queue<Image> allEnemyHintPool = new Queue<Image>();

	private Queue<RoomPlotBlock> allRoomPlotBlockPools = new Queue<RoomPlotBlock>();

	private Dictionary<Vector2Int, RoomPlotBlock> allShowingRoomPlotBlocks = new Dictionary<Vector2Int, RoomPlotBlock>();

	private HiddenBossBlock hiddenBossBlock;

	public override string UIViewName => "RoomUI";

	public override string UILayerName => "DefaultLayer";

	public Transform BossBlockRoot => bossBlockRoot;

	public RoomInfo CurrentRoomInfo => currentRoomInfo;

	public static bool IsAnyBlockInteractiong { get; set; }

	public HiddenBossBlock HiddenBossBlock => hiddenBossBlock;

	public override void OnSpawnUI()
	{
		InitRoomUI();
		InitEnemyHint();
	}

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		SetPlayerCoinAmount(null);
		_timePiecesCtrl.SetAmount(Singleton<GameManager>.Instance.Player.TimePiecesAmount);
		EventManager.RegisterEvent(EventEnum.E_PlayerCoinUpdate, SetPlayerCoinAmount);
		SingletonDontDestroy<UIManager>.Instance.ShowView("CharacterInfoUI");
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		SingletonDontDestroy<UIManager>.Instance.HideView("CharacterInfoUI");
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Room UI ...");
		EventManager.UnregisterEvent(EventEnum.E_PlayerCoinUpdate, SetPlayerCoinAmount);
	}

	private void InitRoomUI()
	{
		base.transform.Find("Root/SettingBtn").GetComponent<Button>().onClick.AddListener(OnClickSettingBtn);
		bagBtn = base.transform.Find("Root/BagBtn").GetComponent<Button>();
		bagBtn.onClick.AddListener(OnClickBagBtn);
		blockBrokenObject = base.transform.Find("Root/BlockBrokenAnim").gameObject;
		roomBlockRoots = new Transform[5];
		for (int i = 0; i < 5; i++)
		{
			roomBlockRoots[i] = base.transform.Find("Root/RoomRoot").GetChild(i);
		}
		bossBlockRoot = base.transform.Find("Root/RoomRoot/BossRoot");
		coinAmountText = base.transform.Find("Root/CoinAmount").GetComponent<Text>();
		coinAmountTextOutline = coinAmountText.GetComponent<Outline>();
		bgImg = base.transform.Find("Root/Bg").GetComponent<Image>();
		roomInfoText = base.transform.Find("Root/RoomInfo").GetComponent<Text>();
		_timePiecesCtrl = base.transform.Find("Root/TimePiecesBottom").GetComponent<TimePiecesCtrl>();
	}

	public void UpdateTimePiecesAmount(int amount)
	{
		_timePiecesCtrl.UpdateAmount(amount);
	}

	public void BagBtnHint()
	{
		if (bagBtnTransformHintTween != null && bagBtnTransformHintTween.IsActive())
		{
			bagBtnTransformHintTween.Complete();
		}
		bagBtnTransformHintTween = bagBtn.transform.TransformHint();
	}

	private void SetPlayerCoinAmount(EventData data)
	{
		coinAmountText.DOKill();
		coinAmountTextOutline.DOKill();
		int playerMoney = Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney;
		int currentMoney = goldCacheAmount;
		goldCacheAmount = playerMoney;
		coinAmountTextOutline.effectColor = coinAmountText.color;
		coinAmountTextOutline.DOColor(Color.black, 0.7f);
		DOTween.To(() => currentMoney, delegate(int x)
		{
			currentMoney = x;
		}, playerMoney, 0.7f).OnUpdate(delegate
		{
			coinAmountText.text = $"{currentMoney}G";
		});
	}

	public void LoadRoomInfo()
	{
		IsAnyBlockInteractiong = false;
		LoadRoomInfo(RoomManager.Instance.GetRoomInfo(Singleton<GameManager>.Instance.CurrentMapLevel, Singleton<GameManager>.Instance.CurrentMapLayer));
		bgImg.sprite = allRoomLevelBgSprites[Singleton<GameManager>.Instance.CurrentMapLevel - 1];
		roomInfoText.text = Singleton<GameManager>.Instance.CurrentMapLevel + "-" + Singleton<GameManager>.Instance.CurrentMapLayer;
		if (Singleton<GameManager>.Instance.CurrentMapLayer == 3 || (Singleton<GameManager>.Instance.CurrentMapLayer == 2 && Singleton<GameManager>.Instance.CurrentMapLevel == 4))
		{
			ForceOpenAllRoomBlocks(isNeedAnim: false);
		}
	}

	public void LoadRoomInfo(GameSaveProcessInfo info)
	{
		RecycleAllEnemyHints();
		RecycleAllRoomBlock();
		IsAnyBlockInteractiong = false;
		Dictionary<string, List<Vec2Seri>> allBlockLoadHandleInfos = info.allBlockLoadHandleInfos;
		currentRoomInfo = info.currentRoomInfo;
		for (int i = 0; i < currentRoomInfo.room.GetLength(1); i++)
		{
			for (int j = 0; j < currentRoomInfo.room.GetLength(0); j++)
			{
				RoomBlock roomBlock = GetRoomBlock(i);
				roomBlock.transform.localPosition = new Vector3(j * 175, 0f, 0f);
				Vector2Int vector2Int = new Vector2Int(j, i);
				roomBlock.LoadRoomBlock(this, currentRoomInfo.room[j, i], currentRoomInfo.seeds[j, i], vector2Int);
				allShowingRoomBlocks[vector2Int] = roomBlock;
				allShowingBaseBlocks.Add(vector2Int, roomBlock);
			}
		}
		foreach (KeyValuePair<string, List<Vec2Seri>> item in allBlockLoadHandleInfos)
		{
			if (!item.Key.IsNullOrEmpty())
			{
				MethodInfo method = typeof(RoomUI).GetMethod(item.Key);
				for (int k = 0; k < item.Value.Count; k++)
				{
					method.Invoke(this, BindingFlags.Public, null, new object[1] { item.Value[k] }, CultureInfo.CurrentCulture);
				}
			}
		}
		roomInfoText.text = Singleton<GameManager>.Instance.CurrentMapLevel + "-" + Singleton<GameManager>.Instance.CurrentMapLayer;
		bgImg.sprite = allRoomLevelBgSprites[Singleton<GameManager>.Instance.CurrentMapLevel - 1];
		EventManager.BroadcastEvent(EventEnum.E_LoadRoomInfo, null);
	}

	public Vector3 GetSpecialRoomBlockWorldPos(Vector2Int pos)
	{
		return allShowingBaseBlocks[pos].transform.position;
	}

	private void OnClickBagBtn()
	{
		if (!IsAnyBlockInteractiong)
		{
			SingletonDontDestroy<UIManager>.Instance.HideView(this);
			SingletonDontDestroy<UIManager>.Instance.ShowView("BagUI");
		}
	}

	private void OnClickSettingBtn()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("SettingUI", false);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
	}

	private void LoadRoomInfo(RoomInfo info)
	{
		RecycleAllRoomBlock();
		RecycleAllEnemyHints();
		currentRoomInfo = info;
		RoomBlock roomBlock = null;
		for (int i = 0; i < info.room.GetLength(1); i++)
		{
			for (int j = 0; j < info.room.GetLength(0); j++)
			{
				RoomBlock roomBlock2 = GetRoomBlock(i);
				roomBlock2.transform.localPosition = new Vector3(j * 175, 0f, 0f);
				Vector2Int vector2Int = new Vector2Int(j, i);
				roomBlock2.LoadRoomBlock(this, info.room[j, i], info.seeds[j, i], vector2Int);
				allShowingRoomBlocks[vector2Int] = roomBlock2;
				allShowingBaseBlocks.Add(vector2Int, roomBlock2);
				if (info.room[j, i] == 2)
				{
					roomBlock = roomBlock2;
				}
			}
		}
		if (roomBlock == null)
		{
			foreach (KeyValuePair<Vector2Int, RoomBlock> allShowingRoomBlock in allShowingRoomBlocks)
			{
				if (allShowingRoomBlock.Value.RoomIndex == 0)
				{
					roomBlock = allShowingRoomBlock.Value;
					break;
				}
			}
		}
		roomBlock.StartBlock(isClick: true);
		EventManager.BroadcastEvent(EventEnum.E_LoadRoomInfo, null);
	}

	private void RecycleAllRoomBlock()
	{
		RecycleAllRoomBlocks();
		RecycleAllObstacleBlock();
		if (normalNextRoomBlock != null)
		{
			normalNextRoomBlock.gameObject.SetActive(value: false);
		}
		if (bossNextRoomBlock != null)
		{
			bossNextRoomBlock.gameObject.SetActive(value: false);
		}
		RecycleAllMonsterBlock();
		RecycleAllEventBlock();
		RecycleAllSpecialEventBlock();
		RecycleAllRoomPlotBlocks();
		if (equipShopBlock != null)
		{
			equipShopBlock.gameObject.SetActive(value: false);
		}
		if (cardShopBlock != null)
		{
			cardShopBlock.gameObject.SetActive(value: false);
		}
		allShowingBaseBlocks.Clear();
		RecycleHiddenBossBlock();
	}

	public bool PlayRoomBlockBrokenAnim(Vector3 pos, Action animOverAction)
	{
		blockBrokenAnimOverAction = animOverAction;
		blockBrokenObject.transform.position = pos;
		blockBrokenObject.gameObject.SetActive(value: true);
		return true;
	}

	public void OnRoomBlockBrokenAnimOver()
	{
		IsAnyBlockInteractiong = false;
		blockBrokenAnimOverAction?.Invoke();
		blockBrokenObject.gameObject.SetActive(value: false);
	}

	public void ActiveRoundRoomBlock(Vector2Int pos)
	{
		int length = currentRoomInfo.room.GetLength(0);
		int length2 = currentRoomInfo.room.GetLength(1);
		if (pos.x + 1 < length && allShowingRoomBlocks.TryGetValue(new Vector2Int(pos.x + 1, pos.y), out var value))
		{
			value.ActiveBlock();
		}
		if (pos.x - 1 >= 0 && allShowingRoomBlocks.TryGetValue(new Vector2Int(pos.x - 1, pos.y), out var value2))
		{
			value2.ActiveBlock();
		}
		if (pos.y + 1 < length2 && allShowingRoomBlocks.TryGetValue(new Vector2Int(pos.x, pos.y + 1), out var value3))
		{
			value3.ActiveBlock();
		}
		if (pos.y - 1 >= 0 && allShowingRoomBlocks.TryGetValue(new Vector2Int(pos.x, pos.y - 1), out var value4))
		{
			value4.ActiveBlock();
		}
	}

	public void ForceOpenAllRoomBlocks(bool isNeedAnim)
	{
		List<RoomBlock> list = new List<RoomBlock>(allShowingRoomBlocks.Values);
		if (isNeedAnim)
		{
			base.transform.DOPunchPosition(Vector3.up * 25f, 0.3f);
		}
		foreach (RoomBlock item in list)
		{
			item.StartBlock(isClick: false);
		}
	}

	public Dictionary<string, List<Vec2Seri>> GetAllBlockLoadHandleInfos()
	{
		Dictionary<string, List<Vec2Seri>> dictionary = new Dictionary<string, List<Vec2Seri>>();
		foreach (KeyValuePair<Vector2Int, BaseBlock> allShowingBaseBlock in allShowingBaseBlocks)
		{
			Vec2Seri item;
			if (dictionary.TryGetValue(allShowingBaseBlock.Value.HandleLoadActionName, out var value))
			{
				List<Vec2Seri> list = value;
				item = new Vec2Seri
				{
					x = allShowingBaseBlock.Key.x,
					y = allShowingBaseBlock.Key.y
				};
				list.Add(item);
			}
			else
			{
				string handleLoadActionName = allShowingBaseBlock.Value.HandleLoadActionName;
				List<Vec2Seri> list2 = new List<Vec2Seri>();
				item = new Vec2Seri
				{
					x = allShowingBaseBlock.Key.x,
					y = allShowingBaseBlock.Key.y
				};
				list2.Add(item);
				dictionary.Add(handleLoadActionName, list2);
			}
		}
		return dictionary;
	}

	public void AddEmptyBlock(Vector2Int pos, bool isSetSprite)
	{
		if (allShowingRoomBlocks.TryGetValue(pos, out var value))
		{
			allShowingRoomBlocks.Remove(pos);
			allShowingEmptyBlocks[pos] = value;
			value.SetEmptyBlock();
			value.ActiveRoundBlock();
			if (isSetSprite)
			{
				value.SetSprite(SingletonDontDestroy<ResourceManager>.Instance.LoadSprite("空白格2", "Sprites/RoomUI"));
			}
		}
		else
		{
			RoomBlock roomBlock = GetRoomBlock(pos.y);
			roomBlock.LoadRoomBlock(this, 0, currentRoomInfo.seeds[pos.x, pos.y], pos);
			roomBlock.transform.localPosition = new Vector3(pos.x * 175, 0f, 0f);
			allShowingRoomBlocks[pos] = roomBlock;
			allShowingBaseBlocks[pos] = roomBlock;
			roomBlock.StartBlock(isClick: false);
		}
	}

	private RoomBlock GetRoomBlock(int posIntY)
	{
		if (allRoomBlockPool.Count > 0)
		{
			RoomBlock roomBlock = allRoomBlockPool.Dequeue();
			roomBlock.gameObject.SetActive(value: true);
			roomBlock.transform.SetParent(roomBlockRoots[roomBlockRoots.Length - posIntY - 1]);
			return roomBlock;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("RoomBlock", "Prefabs/Room", roomBlockRoots[roomBlockRoots.Length - posIntY - 1]).GetComponent<RoomBlock>();
	}

	public void RecycleAEmptyBlock(Vector2Int pos)
	{
		if (allShowingEmptyBlocks.TryGetValue(pos, out var value))
		{
			value.gameObject.SetActive(value: false);
			allRoomBlockPool.Enqueue(value);
			allShowingEmptyBlocks.Remove(pos);
		}
	}

	private void RecycleAllRoomBlocks()
	{
		if (allShowingRoomBlocks.Count > 0)
		{
			foreach (KeyValuePair<Vector2Int, RoomBlock> allShowingRoomBlock in allShowingRoomBlocks)
			{
				allShowingRoomBlock.Value.gameObject.SetActive(value: false);
				allRoomBlockPool.Enqueue(allShowingRoomBlock.Value);
			}
			allShowingRoomBlocks.Clear();
		}
		if (allShowingEmptyBlocks.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<Vector2Int, RoomBlock> allShowingEmptyBlock in allShowingEmptyBlocks)
		{
			allShowingEmptyBlock.Value.gameObject.SetActive(value: false);
			allRoomBlockPool.Enqueue(allShowingEmptyBlock.Value);
		}
		allShowingEmptyBlocks.Clear();
	}

	private void RecycleRoomBlock(Vector2Int pos)
	{
		if (allShowingRoomBlocks.TryGetValue(pos, out var value))
		{
			value.gameObject.SetActive(value: false);
			allRoomBlockPool.Enqueue(value);
			allShowingRoomBlocks.Remove(pos);
		}
	}

	public void AddObstacleBlockToMap(Vector2Int pos)
	{
		RecycleRoomBlock(pos);
		ObstacleBlock obstacleBlock = GetObstacleBlock(roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
		obstacleBlock.transform.localPosition = new Vector3(pos.x * 175, 0f, 0f);
		obstacleBlock.transform.SetAsLastSibling();
		obstacleBlock.AcitveObstacleBlock(pos, currentRoomInfo.seeds[pos.x, pos.y]);
		allShowingObstacleBlocks.Add(obstacleBlock);
		allShowingBaseBlocks[pos] = obstacleBlock;
	}

	private ObstacleBlock GetObstacleBlock(Transform root)
	{
		if (allObstacleBlockPool.Count > 0)
		{
			ObstacleBlock obstacleBlock = allObstacleBlockPool.Dequeue();
			obstacleBlock.gameObject.SetActive(value: true);
			obstacleBlock.transform.SetParent(root);
			return obstacleBlock;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("ObstacleBlock", "Prefabs/Room", root).GetComponent<ObstacleBlock>();
	}

	private void RecycleAllObstacleBlock()
	{
		if (allShowingObstacleBlocks.Count > 0)
		{
			for (int i = 0; i < allShowingObstacleBlocks.Count; i++)
			{
				ObstacleBlock obstacleBlock = allShowingObstacleBlocks[i];
				obstacleBlock.gameObject.SetActive(value: false);
				allObstacleBlockPool.Enqueue(obstacleBlock);
			}
			allShowingObstacleBlocks.Clear();
		}
	}

	public void AddNormalNextRoomBlock(Vector2Int pos)
	{
		if (normalNextRoomBlock == null)
		{
			GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("NextRoomDoor_Normal", "Prefabs/Room", roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
			normalNextRoomBlock = gameObject.GetComponent<NextRoomBlock>();
		}
		else
		{
			normalNextRoomBlock.gameObject.SetActive(value: true);
			normalNextRoomBlock.transform.SetParent(roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
		}
		RecycleRoomBlock(pos);
		normalNextRoomBlock.transform.localPosition = new Vector3(pos.x * 175, 0f, 0f);
		normalNextRoomBlock.transform.SetAsLastSibling();
		normalNextRoomBlock.ActiveNextRoomBlock(pos, CurrentRoomInfo.seeds[pos.x, pos.y]);
		allShowingBaseBlocks[pos] = normalNextRoomBlock;
		ActiveRoundRoomBlock(pos);
	}

	public void AddNormalNextRoomBlock_ForBoss(Vector2Int pos)
	{
		if (normalNextRoomBlock == null)
		{
			GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("NextRoomDoor_Normal", "Prefabs/Room", bossBlockRoot);
			normalNextRoomBlock = gameObject.GetComponent<NextRoomBlock>();
		}
		else
		{
			normalNextRoomBlock.gameObject.SetActive(value: true);
			normalNextRoomBlock.transform.SetParent(bossBlockRoot);
		}
		normalNextRoomBlock.transform.localPosition = new Vector3(pos.x * 175, pos.y * 175, 0f);
		normalNextRoomBlock.transform.SetAsLastSibling();
		normalNextRoomBlock.ActiveNextRoomBlock(pos, CurrentRoomInfo.seeds[pos.x, pos.y]);
	}

	public void AddBossNextRoomBlock(Vector2Int pos)
	{
		if (bossNextRoomBlock == null)
		{
			GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("NextRoomDoor_Boss", "Prefabs/Room", roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
			bossNextRoomBlock = gameObject.GetComponent<NextRoomBlock>();
		}
		else
		{
			bossNextRoomBlock.gameObject.SetActive(value: true);
			bossNextRoomBlock.transform.SetParent(roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
		}
		RecycleRoomBlock(pos);
		bossNextRoomBlock.transform.localPosition = new Vector3(pos.x * 175, 0f, 0f);
		bossNextRoomBlock.transform.SetAsLastSibling();
		bossNextRoomBlock.ActiveNextRoomBlock(pos, CurrentRoomInfo.seeds[pos.x, pos.y]);
		allShowingBaseBlocks[pos] = bossNextRoomBlock;
		ActiveRoundRoomBlock(pos);
	}

	public void AddEliteMonsterBlock(Vector2Int pos, bool isClick)
	{
		if (eliteMonsterBlock == null)
		{
			GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("EliteMonsterBlock", "Prefabs/Room", roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
			eliteMonsterBlock = gameObject.GetComponent<EliteMonsterBlock>();
		}
		else
		{
			eliteMonsterBlock.gameObject.SetActive(value: true);
			eliteMonsterBlock.transform.SetParent(roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
		}
		RecycleRoomBlock(pos);
		eliteMonsterBlock.transform.localPosition = new Vector3(175 * pos.x, 0f, 0f);
		eliteMonsterBlock.transform.SetAsLastSibling();
		eliteMonsterBlock.ActiveEliteMonsterBlock(pos, currentRoomInfo.specialMonsterHeapCode, currentRoomInfo.seeds[pos.x, pos.y], isClick);
		allShowingBaseBlocks[pos] = eliteMonsterBlock;
	}

	public void AddNormalMonsterBlock(Vector2Int pos, bool isClick)
	{
		RecycleRoomBlock(pos);
		NormalMonsterBlock normalMonsterBlock = GetNormalMonsterBlock(roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
		normalMonsterBlock.transform.localPosition = new Vector3(pos.x * 175, 0f, 0f);
		normalMonsterBlock.transform.SetAsLastSibling();
		normalMonsterBlock.ActiveNormalMonsterBlock(pos, currentRoomInfo.monsterHeapInfos[new Vec2Seri(pos.x, pos.y)], currentRoomInfo.seeds[pos.x, pos.y], isClick);
		allShowingNormalMonsterBlocks.Add(pos, normalMonsterBlock);
		allShowingBaseBlocks[pos] = normalMonsterBlock;
	}

	private NormalMonsterBlock GetNormalMonsterBlock(Transform root)
	{
		if (allNormalMonsterBlockPool.Count > 0)
		{
			NormalMonsterBlock normalMonsterBlock = allNormalMonsterBlockPool.Dequeue();
			normalMonsterBlock.gameObject.SetActive(value: true);
			normalMonsterBlock.transform.SetParent(root);
			return normalMonsterBlock;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("NormalMonsterBlock", "Prefabs/Room", root).GetComponent<NormalMonsterBlock>();
	}

	public void RecycleNormalMonsterBlock(Vector2Int pos)
	{
		NormalMonsterBlock normalMonsterBlock = allShowingNormalMonsterBlocks[pos];
		normalMonsterBlock.gameObject.SetActive(value: false);
		allNormalMonsterBlockPool.Enqueue(normalMonsterBlock);
		allShowingNormalMonsterBlocks.Remove(pos);
	}

	private void RecycleAllMonsterBlock()
	{
		if (eliteMonsterBlock != null)
		{
			eliteMonsterBlock.gameObject.SetActive(value: false);
		}
		if (bossBlock != null)
		{
			bossBlock.gameObject.SetActive(value: false);
		}
		if (allShowingNormalMonsterBlocks.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<Vector2Int, NormalMonsterBlock> allShowingNormalMonsterBlock in allShowingNormalMonsterBlocks)
		{
			allShowingNormalMonsterBlock.Value.gameObject.SetActive(value: false);
			allNormalMonsterBlockPool.Enqueue(allShowingNormalMonsterBlock.Value);
		}
		allShowingNormalMonsterBlocks.Clear();
	}

	public void AddBossBlock(Vector2Int pos)
	{
		if (bossBlock == null)
		{
			GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("BossBlock", "Prefabs/Room", bossBlockRoot);
			bossBlock = gameObject.GetComponent<BossBlock>();
		}
		else
		{
			bossBlock.gameObject.SetActive(value: true);
		}
		EnemyHeapData enemyHeapData = Singleton<EnemyController>.Instance.GetEnemyHeapData(currentRoomInfo.specialMonsterHeapCode);
		Singleton<EnemyController>.Instance.SetTriggeringEnemyHeap(enemyHeapData.EnemyHeapCode);
		bool isEnd = Singleton<GameManager>.Instance.AllClearBossHeapIdList.Contains(enemyHeapData.EnemyHeapCode);
		RecycleRoomBlock(pos);
		bossBlock.transform.localPosition = new Vector3(pos.x * 175, pos.y * 175, 0f);
		bossBlock.transform.SetAsLastSibling();
		bossBlock.ActiveBossBlock(enemyHeapData, currentRoomInfo.seeds[pos.x, pos.y], isEnd);
		allShowingBaseBlocks[pos] = bossBlock;
	}

	public void AddEquipShopBlock(Vector2Int pos)
	{
		RecycleRoomBlock(pos);
		if (equipShopBlock == null)
		{
			GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("EquipShopBlock", "Prefabs/Room", roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
			equipShopBlock = gameObject.GetComponent<EquipShopBlock>();
		}
		else
		{
			equipShopBlock.gameObject.SetActive(value: true);
			equipShopBlock.transform.SetParent(roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
		}
		equipShopBlock.transform.localPosition = new Vector3(pos.x * 175, 0f, 0f);
		equipShopBlock.transform.SetAsLastSibling();
		equipShopBlock.ActiveEquipShopBlock(pos, currentRoomInfo.seeds[pos.x, pos.y]);
		allShowingBaseBlocks[pos] = equipShopBlock;
		ActiveRoundRoomBlock(pos);
	}

	public void AddCardShopBlock(Vector2Int pos)
	{
		RecycleRoomBlock(pos);
		if (cardShopBlock == null)
		{
			GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("CardShopBlock", "Prefabs/Room", roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
			cardShopBlock = gameObject.GetComponent<CardShopBlock>();
		}
		else
		{
			cardShopBlock.gameObject.SetActive(value: true);
			cardShopBlock.transform.SetParent(roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
		}
		cardShopBlock.transform.localPosition = new Vector3(pos.x * 175, 0f, 0f);
		cardShopBlock.transform.SetAsLastSibling();
		cardShopBlock.ActiveCardShopBlock(pos, currentRoomInfo.seeds[pos.x, pos.y]);
		allShowingBaseBlocks[pos] = cardShopBlock;
		ActiveRoundRoomBlock(pos);
	}

	public void AddEventBlock(Vector2Int pos, bool isClick)
	{
		UnityEngine.Random.InitState(currentRoomInfo.seeds[pos.x, pos.y]);
		string eventCode = ((SingletonDontDestroy<Game>.Instance.isTest && !SingletonDontDestroy<Game>.Instance.testGameEventCode.IsNullOrEmpty()) ? SingletonDontDestroy<Game>.Instance.testGameEventCode : currentRoomInfo.eventInfos[new Vec2Seri(pos.x, pos.y)]);
		BaseGameEvent @event = GameEventManager.Instace.GetEvent(eventCode);
		@event.SetBlockPosition(pos);
		GameEventManager.Instace.TriggeredEvent(pos, @event);
		GameEventData gameEventData = DataManager.Instance.GetGameEventData(@event.GameEventCode);
		if (gameEventData.GameEventPrefab == "RandomEventBlock")
		{
			RecycleRoomBlock(pos);
			RandomEventBlock randomEventBlock = GetRandomEventBlock(roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
			randomEventBlock.transform.localPosition = new Vector3(pos.x * 175, 0f, 0f);
			randomEventBlock.transform.SetAsLastSibling();
			allShowingRandomEventBlocks.Add(pos, randomEventBlock);
			randomEventBlock.ActiveRandomEventBlock(pos, currentRoomInfo.seeds[pos.x, pos.y], @event, isClick);
			allShowingBaseBlocks[pos] = randomEventBlock;
			ActiveRoundRoomBlock(pos);
		}
		else
		{
			AddSpecialEventBlock(pos, gameEventData.GameEventPrefab, @event, isClick);
		}
	}

	private void AddRandomEventBlock_Load(Vector2Int pos)
	{
		BaseGameEvent eventOnMap = GameEventManager.Instace.GetEventOnMap(pos);
		if (eventOnMap != null)
		{
			eventOnMap.SetBlockPosition(pos);
			GameEventData gameEventData = DataManager.Instance.GetGameEventData(eventOnMap.GameEventCode);
			if (gameEventData.GameEventPrefab == "RandomEventBlock")
			{
				RecycleRoomBlock(pos);
				RandomEventBlock randomEventBlock = GetRandomEventBlock(roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
				randomEventBlock.transform.localPosition = new Vector3(pos.x * 175, 0f, 0f);
				randomEventBlock.transform.SetAsLastSibling();
				allShowingRandomEventBlocks.Add(pos, randomEventBlock);
				randomEventBlock.ActiveRandomEventBlock(pos, currentRoomInfo.seeds[pos.x, pos.y], eventOnMap, isAutoActive: false);
				allShowingBaseBlocks[pos] = randomEventBlock;
			}
			else
			{
				AddSpecialEventBlock(pos, gameEventData.GameEventPrefab, eventOnMap, isClick: false);
			}
		}
		else
		{
			Debug.LogError("Event do not exist in event map");
		}
	}

	private RandomEventBlock GetRandomEventBlock(Transform root)
	{
		if (allRandomEventBlockPool.Count > 0)
		{
			RandomEventBlock randomEventBlock = allRandomEventBlockPool.Dequeue();
			randomEventBlock.gameObject.SetActive(value: true);
			randomEventBlock.transform.SetParent(root);
			return randomEventBlock;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("RandomEventBlock", "Prefabs/Room", root).GetComponent<RandomEventBlock>();
	}

	public void RecycleRandomEventBlock(Vector2Int pos)
	{
		if (allShowingRandomEventBlocks.TryGetValue(pos, out var value))
		{
			value.gameObject.SetActive(value: false);
			allRandomEventBlockPool.Enqueue(value);
		}
	}

	private void RecycleAllEventBlock()
	{
		if (allShowingRandomEventBlocks.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<Vector2Int, RandomEventBlock> allShowingRandomEventBlock in allShowingRandomEventBlocks)
		{
			allShowingRandomEventBlock.Value.gameObject.SetActive(value: false);
			allRandomEventBlockPool.Enqueue(allShowingRandomEventBlock.Value);
		}
		allShowingRandomEventBlocks.Clear();
	}

	public void VarifyEventRoomInfo(string prefabName, string newEventCode)
	{
		SpecialEventBlock specialEventBlock = allShowingSpecialEventBlocks[prefabName];
		specialEventBlock.ChangeEventCode(newEventCode);
		allShowingSpecialEventBlocks.Remove(prefabName);
		allShowingSpecialEventBlocks.Add(newEventCode, specialEventBlock);
	}

	public void AddSpecialEventBlock(Vector2Int pos, string prefabName, BaseGameEvent gameEvent, bool isClick)
	{
		RecycleRoomBlock(pos);
		SpecialEventBlock specialEventBlock = GetSpecialEventBlock(prefabName, roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
		specialEventBlock.transform.SetAsLastSibling();
		specialEventBlock.transform.localPosition = new Vector3(pos.x * 175, 0f, 0f);
		allShowingSpecialEventBlocks.Add(prefabName, specialEventBlock);
		specialEventBlock.ActiveEventBlock(pos, currentRoomInfo.seeds[pos.x, pos.y], gameEvent, isClick);
		allShowingBaseBlocks[pos] = specialEventBlock;
		ActiveRoundRoomBlock(pos);
	}

	public void AddSpecialEventBlock_ForBoss(Vector2Int pos, string prefabName, BaseGameEvent gameEvent)
	{
		RecycleRoomBlock(pos);
		SpecialEventBlock specialEventBlock = GetSpecialEventBlock(prefabName, bossBlockRoot);
		specialEventBlock.transform.SetAsLastSibling();
		specialEventBlock.transform.localPosition = new Vector3(pos.x * 175, pos.y * 175, 0f);
		allShowingSpecialEventBlocks.Add(prefabName, specialEventBlock);
		specialEventBlock.ActiveEventBlock(pos, currentRoomInfo.seeds[pos.x, pos.y], gameEvent, isAutoActive: false);
		allShowingBaseBlocks[pos] = specialEventBlock;
		ActiveRoundRoomBlock(pos);
	}

	private SpecialEventBlock GetSpecialEventBlock(string prefabName, Transform root)
	{
		if (specialEventBlockPool.TryGetValue(prefabName, out var value))
		{
			specialEventBlockPool.Remove(prefabName);
			value.gameObject.SetActive(value: true);
			value.transform.SetParent(root);
			return value;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace(prefabName, "Prefabs/Room", root).GetComponent<SpecialEventBlock>();
	}

	public void RecycleSpecialEventBlock(string prefabName)
	{
		if (allShowingSpecialEventBlocks.TryGetValue(prefabName, out var value))
		{
			value.gameObject.SetActive(value: false);
			allShowingSpecialEventBlocks.Remove(prefabName);
			specialEventBlockPool.Add(prefabName, value);
		}
	}

	private void RecycleAllSpecialEventBlock()
	{
		if (allShowingSpecialEventBlocks.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, SpecialEventBlock> allShowingSpecialEventBlock in allShowingSpecialEventBlocks)
		{
			allShowingSpecialEventBlock.Value.gameObject.SetActive(value: false);
			specialEventBlockPool[allShowingSpecialEventBlock.Key] = allShowingSpecialEventBlock.Value;
		}
		allShowingSpecialEventBlocks.Clear();
	}

	public void HandleLoad_EmptyBlock(Vec2Seri pos)
	{
		AddEmptyBlock(new Vector2Int(pos.x, pos.y), isSetSprite: true);
	}

	public void HandleLoad_ObstacleBlock(Vec2Seri pos)
	{
		AddObstacleBlockToMap(new Vector2Int(pos.x, pos.y));
	}

	public void HandleLoad_ExitBlock(Vec2Seri pos)
	{
		if (Singleton<GameManager>.Instance.CurrentMapLayer == 2 || Singleton<GameManager>.Instance.CurrentMapLevel == 4)
		{
			AddBossNextRoomBlock(new Vector2Int(pos.x, pos.y));
		}
		else
		{
			AddNormalNextRoomBlock(new Vector2Int(pos.x, pos.y));
		}
	}

	public void HandleLoad_NormalMonsterBlock(Vec2Seri pos)
	{
		AddNormalMonsterBlock(new Vector2Int(pos.x, pos.y), isClick: false);
	}

	public void HandleLoad_EliteMonsterBlock(Vec2Seri pos)
	{
		AddEliteMonsterBlock(new Vector2Int(pos.x, pos.y), isClick: false);
	}

	public void HandleLoad_CardShopBlock(Vec2Seri pos)
	{
		AddCardShopBlock(new Vector2Int(pos.x, pos.y));
	}

	public void HandleLoad_EquipShopBlock(Vec2Seri pos)
	{
		AddEquipShopBlock(new Vector2Int(pos.x, pos.y));
	}

	public void HandleLoad_EventBlock(Vec2Seri pos)
	{
		AddRandomEventBlock_Load(new Vector2Int(pos.x, pos.y));
	}

	public void HandleLoad_BossBlock(Vec2Seri pos)
	{
		AddBossBlock(new Vector2Int(pos.x, pos.y));
	}

	public void HandleLoad_RoomPlotBlock(Vec2Seri pos)
	{
		AddRoomPlotBlock(new Vector2Int(pos.x, pos.y));
	}

	public void HandleLoad_HiddenBossBlock(Vec2Seri pos)
	{
		AddHiddenBossBlock(new Vector2Int(pos.x, pos.y));
	}

	private void InitEnemyHint()
	{
		enemyHintRoot = base.transform.Find("Root/RoomRoot/EnemyHintRoot");
	}

	public void ShowEnemyHint()
	{
		for (int i = 0; i < currentRoomInfo.room.GetLength(0); i++)
		{
			for (int j = 0; j < currentRoomInfo.room.GetLength(1); j++)
			{
				if ((currentRoomInfo.room[i, j] == 5 || currentRoomInfo.room[i, j] == 4) && allShowingRoomBlocks.ContainsKey(new Vector2Int(i, j)))
				{
					Image enemyHint = GetEnemyHint();
					enemyHint.transform.localPosition = new Vector3(175 * i, 175 * j, 0f);
					allShowingEnemyHints.Add(new Vector2Int(i, j), enemyHint);
					enemyHint.sprite = ((currentRoomInfo.room[i, j] == 4) ? EliteMonsterSpriteHint : NormalMonsterSpriteHint);
				}
			}
		}
	}

	public void HideEnemyHint(Vector2Int pos)
	{
		if (allShowingEnemyHints.TryGetValue(pos, out var hint))
		{
			hint.material.DOFloat(0f, "_ColorAlpha", 0.4f).OnComplete(delegate
			{
				hint.gameObject.SetActive(value: false);
			});
			allEnemyHintPool.Enqueue(hint);
		}
	}

	private Image GetEnemyHint()
	{
		if (allEnemyHintPool.Count > 0)
		{
			Image image = allEnemyHintPool.Dequeue();
			image.gameObject.SetActive(value: true);
			image.material.DOKill();
			image.material.SetFloat("_ColorAlpha", 1f);
			return image;
		}
		Image component = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("EnemyPositionHint", "Prefabs", enemyHintRoot).GetComponent<Image>();
		component.material = UnityEngine.Object.Instantiate(component.material);
		return component;
	}

	private void RecycleAllEnemyHints()
	{
		if (allShowingEnemyHints.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<Vector2Int, Image> allShowingEnemyHint in allShowingEnemyHints)
		{
			allShowingEnemyHint.Value.gameObject.SetActive(value: false);
			allEnemyHintPool.Enqueue(allShowingEnemyHint.Value);
		}
		allShowingEnemyHints.Clear();
	}

	public void AddRoomPlotBlock(Vector2Int pos)
	{
		RecycleRoomBlock(pos);
		string plotCode = currentRoomInfo.plotCode;
		RoomPlotBlock roomPlotBlock = GetRoomPlotBlock(roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
		roomPlotBlock.transform.localPosition = new Vector3(pos.x * 175, 0f, 0f);
		roomPlotBlock.transform.SetAsLastSibling();
		roomPlotBlock.SetBlockPlotCode(pos, plotCode);
		allShowingRoomPlotBlocks.Add(pos, roomPlotBlock);
		allShowingBaseBlocks[pos] = roomPlotBlock;
	}

	private RoomPlotBlock GetRoomPlotBlock(Transform root)
	{
		if (allRoomPlotBlockPools.Count > 0)
		{
			RoomPlotBlock roomPlotBlock = allRoomPlotBlockPools.Dequeue();
			roomPlotBlock.gameObject.SetActive(value: true);
			roomPlotBlock.transform.SetParent(root);
			return roomPlotBlock;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("RoomPlotBlock", "Prefabs/Room", root).GetComponent<RoomPlotBlock>();
	}

	public void RecycleRoomPlotBlock(Vector2Int pos)
	{
		RoomPlotBlock roomPlotBlock = allShowingRoomPlotBlocks[pos];
		roomPlotBlock.gameObject.SetActive(value: false);
		allRoomPlotBlockPools.Enqueue(roomPlotBlock);
		allShowingRoomPlotBlocks.Remove(pos);
	}

	private void RecycleAllRoomPlotBlocks()
	{
		if (allShowingRoomPlotBlocks.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<Vector2Int, RoomPlotBlock> allShowingRoomPlotBlock in allShowingRoomPlotBlocks)
		{
			allShowingRoomPlotBlock.Value.gameObject.SetActive(value: false);
			allRoomPlotBlockPools.Enqueue(allShowingRoomPlotBlock.Value);
		}
		allShowingRoomPlotBlocks.Clear();
	}

	public void AddHiddenBossBlock(Vector2Int pos)
	{
		if (hiddenBossBlock == null)
		{
			GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("HiddenBossBlock", "Prefabs/Room", roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
			hiddenBossBlock = gameObject.GetComponent<HiddenBossBlock>();
			hiddenBossBlock.Init();
		}
		else
		{
			hiddenBossBlock.gameObject.SetActive(value: true);
			hiddenBossBlock.transform.SetParent(roomBlockRoots[roomBlockRoots.Length - pos.y - 1]);
		}
		hiddenBossBlock.transform.localPosition = new Vector3(175 * pos.x, 0f, 0f);
		hiddenBossBlock.transform.SetAsLastSibling();
		allShowingBaseBlocks[pos] = hiddenBossBlock;
		hiddenBossBlock.ActiveHiddenBossBlock(currentRoomInfo.seeds[pos.x, pos.y]);
	}

	public void RecycleHiddenBossBlock()
	{
		if (hiddenBossBlock != null)
		{
			hiddenBossBlock.gameObject.SetActive(value: false);
		}
	}
}
