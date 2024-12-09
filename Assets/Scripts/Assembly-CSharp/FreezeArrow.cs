public class FreezeArrow : Arrow
{
	public override ArrowType MArrowType => ArrowType.Freeze;

	protected override string EffectConfigName => "FreezeArrow_Effect_EffectConfig";

	protected override string AddArrowEffectConfigName => "FreezeArrow_Add_EffectConfig";

	protected override void OnArrowEffect(EntityBase[] targets)
	{
		Singleton<GameManager>.Instance.Player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponArrowEffectTakeEffect, (BattleEffectData)new SimpleEffectData
		{
			intData = (int)MArrowType
		}, out int IntData);
		if (targets != null && targets.Length != 0)
		{
			for (int i = 0; i < targets.Length; i++)
			{
				targets[i].GetBuff(new Buff_Freeze(targets[i], 1 + IntData));
			}
		}
	}
}
