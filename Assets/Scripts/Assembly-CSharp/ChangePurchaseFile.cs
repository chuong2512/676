using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using UnityEngine;

public class ChangePurchaseFile : MonoBehaviour
{
	private Dictionary<string, int> priceList = new Dictionary<string, int>();

	private Dictionary<string, int> stageLimitList = new Dictionary<string, int>();

	private Dictionary<string, int> sourceLimitList = new Dictionary<string, int>();

	private Dictionary<string, int> shopLimitList = new Dictionary<string, int>();

	private AllArmor allArmor;

	private AllTrinket _allTrinket;

	private AllHead _allHead;

	private AllShoes _allShoes;

	private AllMainHand _allMainHand;

	private AllOffHand _allOffHand;

	private AllHands _allHands;

	public void ChangePrice()
	{
		priceList.Clear();
		stageLimitList.Clear();
		sourceLimitList.Clear();
		shopLimitList.Clear();
		using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(Application.dataPath + "/价值系数表 - 公式版.xlsx")))
		{
			ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[2];
			Debug.Log(excelWorksheet.Cells[2, 8].Text);
			for (int i = 2; !excelWorksheet.Cells[i, 1].Text.IsNullOrEmpty(); i++)
			{
				Debug.Log(i);
				Debug.Log(excelWorksheet.Cells[i, 1].Text);
				Debug.Log(excelWorksheet.Cells[i, 8].Text);
				priceList.Add(excelWorksheet.Cells[i, 1].Text, int.Parse(excelWorksheet.Cells[i, 8].Text));
				stageLimitList.Add(excelWorksheet.Cells[i, 1].Text, int.Parse(excelWorksheet.Cells[i, 9].Text));
				sourceLimitList.Add(excelWorksheet.Cells[i, 1].Text, int.Parse(excelWorksheet.Cells[i, 10].Text));
				shopLimitList.Add(excelWorksheet.Cells[i, 1].Text, int.Parse(excelWorksheet.Cells[i, 11].Text));
			}
		}
		_allTrinket = JsonUtility.FromJson<AllTrinket>("Equipment_TrinketData.json".GetAllPlatformStreamingAssetsData());
		_allHead = JsonUtility.FromJson<AllHead>("Equipment_HeadData.json".GetAllPlatformStreamingAssetsData());
		allArmor = JsonUtility.FromJson<AllArmor>("Equipment_ArmorData.json".GetAllPlatformStreamingAssetsData());
		_allHands = JsonUtility.FromJson<AllHands>("Equipment_HandsData.json".GetAllPlatformStreamingAssetsData());
		_allShoes = JsonUtility.FromJson<AllShoes>("Equipment_ShoesData.json".GetAllPlatformStreamingAssetsData());
		_allMainHand = JsonUtility.FromJson<AllMainHand>("Equipment_MainHandData.json".GetAllPlatformStreamingAssetsData());
		_allOffHand = JsonUtility.FromJson<AllOffHand>("Equipment_OffHandData.json".GetAllPlatformStreamingAssetsData());
		foreach (E_Armor_CardAttr allArmorAttr in allArmor.AllArmorAttrs)
		{
			if (priceList.ContainsKey(allArmorAttr.CardCode))
			{
				allArmorAttr.Price = priceList[allArmorAttr.CardCode];
				allArmorAttr.StageLimit = stageLimitList[allArmorAttr.CardCode];
				allArmorAttr.SourceLimit = sourceLimitList[allArmorAttr.CardCode];
				allArmorAttr.ShopLimit = shopLimitList[allArmorAttr.CardCode];
			}
		}
		foreach (EquipmentCardAttr allTrinketAttr in _allTrinket.AllTrinketAttrs)
		{
			if (priceList.ContainsKey(allTrinketAttr.CardCode))
			{
				allTrinketAttr.Price = priceList[allTrinketAttr.CardCode];
				allTrinketAttr.StageLimit = stageLimitList[allTrinketAttr.CardCode];
				allTrinketAttr.SourceLimit = sourceLimitList[allTrinketAttr.CardCode];
				allTrinketAttr.ShopLimit = shopLimitList[allTrinketAttr.CardCode];
			}
		}
		foreach (E_Head_CardAttr allHeadAttr in _allHead.AllHeadAttrs)
		{
			if (priceList.ContainsKey(allHeadAttr.CardCode))
			{
				allHeadAttr.Price = priceList[allHeadAttr.CardCode];
				allHeadAttr.StageLimit = stageLimitList[allHeadAttr.CardCode];
				allHeadAttr.SourceLimit = sourceLimitList[allHeadAttr.CardCode];
				allHeadAttr.ShopLimit = shopLimitList[allHeadAttr.CardCode];
			}
		}
		foreach (E_Shoes_CardAttr allShoesAttr in _allShoes.AllShoesAttrs)
		{
			if (priceList.ContainsKey(allShoesAttr.CardCode))
			{
				allShoesAttr.Price = priceList[allShoesAttr.CardCode];
				allShoesAttr.StageLimit = stageLimitList[allShoesAttr.CardCode];
				allShoesAttr.SourceLimit = sourceLimitList[allShoesAttr.CardCode];
				allShoesAttr.ShopLimit = shopLimitList[allShoesAttr.CardCode];
			}
		}
		foreach (E_Mainhand_CardAttr allMainHandAttr in _allMainHand.AllMainHandAttrs)
		{
			if (priceList.ContainsKey(allMainHandAttr.CardCode))
			{
				allMainHandAttr.Price = priceList[allMainHandAttr.CardCode];
				allMainHandAttr.StageLimit = stageLimitList[allMainHandAttr.CardCode];
				allMainHandAttr.SourceLimit = sourceLimitList[allMainHandAttr.CardCode];
				allMainHandAttr.ShopLimit = shopLimitList[allMainHandAttr.CardCode];
			}
		}
		foreach (E_Offhand_CardAttr allOffHandAttr in _allOffHand.AllOffHandAttrs)
		{
			if (priceList.ContainsKey(allOffHandAttr.CardCode))
			{
				allOffHandAttr.Price = priceList[allOffHandAttr.CardCode];
				allOffHandAttr.StageLimit = stageLimitList[allOffHandAttr.CardCode];
				allOffHandAttr.SourceLimit = sourceLimitList[allOffHandAttr.CardCode];
				allOffHandAttr.ShopLimit = shopLimitList[allOffHandAttr.CardCode];
			}
		}
		foreach (E_Hands_CardAttr allHandsAttr in _allHands.AllHandsAttrs)
		{
			if (priceList.ContainsKey(allHandsAttr.CardCode))
			{
				allHandsAttr.Price = priceList[allHandsAttr.CardCode];
				allHandsAttr.StageLimit = stageLimitList[allHandsAttr.CardCode];
				allHandsAttr.SourceLimit = sourceLimitList[allHandsAttr.CardCode];
				allHandsAttr.ShopLimit = shopLimitList[allHandsAttr.CardCode];
			}
		}
		File.WriteAllText(Application.streamingAssetsPath + "/Equipment_ArmorData.json", JsonUtility.ToJson(allArmor, prettyPrint: true));
		File.WriteAllText(Application.streamingAssetsPath + "/Equipment_HeadData.json", JsonUtility.ToJson(_allHead, prettyPrint: true));
		File.WriteAllText(Application.streamingAssetsPath + "/Equipment_HandsData.json", JsonUtility.ToJson(_allHands, prettyPrint: true));
		File.WriteAllText(Application.streamingAssetsPath + "/Equipment_MainHandData.json", JsonUtility.ToJson(_allMainHand, prettyPrint: true));
		File.WriteAllText(Application.streamingAssetsPath + "/Equipment_OffHandData.json", JsonUtility.ToJson(_allOffHand, prettyPrint: true));
		File.WriteAllText(Application.streamingAssetsPath + "/Equipment_ShoesData.json", JsonUtility.ToJson(_allShoes, prettyPrint: true));
		File.WriteAllText(Application.streamingAssetsPath + "/Equipment_TrinketData.json", JsonUtility.ToJson(_allTrinket, prettyPrint: true));
	}
}
