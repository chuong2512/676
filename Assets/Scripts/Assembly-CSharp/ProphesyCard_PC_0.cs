public class ProphesyCard_PC_0 : ProphesyCard
{
	public override string ProphesyCode => "PC_0";

	public override void Active(bool isLoad)
	{
		if (Singleton<GameManager>.Instance.CurrentMapLayer == 1 && Singleton<GameManager>.Instance.CurrentMapLevel == 1)
		{
			(SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI).ShowEnemyHint();
		}
	}
}
