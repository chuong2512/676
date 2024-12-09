public class PoisonArrow : Arrow
{
	public override ArrowType MArrowType => ArrowType.Poison;

	protected override string EffectConfigName => "PoisonArrow_Effect_EffectConfig";

	protected override string AddArrowEffectConfigName => "PoisonArrow_Add_EffectConfig";

	protected override void OnArrowEffect(EntityBase[] targets)
	{
		if (targets != null && targets.Length != 0)
		{
			int num = 1;
			Singleton<GameManager>.Instance.Player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponArrowEffectTakeEffect, (BattleEffectData)new SimpleEffectData
			{
				intData = (int)MArrowType
			}, out int IntData);
			num += IntData;
			for (int i = 0; i < targets.Length; i++)
			{
				targets[i].GetBuff(new Buff_DeadPoison(targets[i], num));
			}
		}
	}
}
