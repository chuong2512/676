using UnityEngine;

public interface IBagCardDrag
{
	bool IsDragingCard { get; set; }

	void ReleaseCard(string cardCode, bool isMain);

	GameObject StartDragCard(string cardCode, bool isEquiped);

	bool RemoveCardFromInventory(string cardCode);

	void EndDragCard();

	void TryAddCardToInventory(string cardCode, bool isMain);

	void ForceAddCardToInventory(string cardCode, bool isMain);

	void TryEquipCard(string cardCode);

	void RemoveNewCard(string cardCode);
}
