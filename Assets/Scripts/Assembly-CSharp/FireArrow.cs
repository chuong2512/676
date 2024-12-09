public class FireArrow : Arrow
{
	public override ArrowType MArrowType => ArrowType.Fire;

	protected override string EffectConfigName => "FireArrow_Effect_EffectConfig";

	protected override string AddArrowEffectConfigName => "FireArrow_Add_EffectConfig";

	protected override void OnArrowEffect(EntityBase[] targets)
	{
		int num = 0;
		Buff_ShootStrengthen buff_ShootStrengthen;
		if ((buff_ShootStrengthen = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(Singleton<GameManager>.Instance.Player, BuffType.Buff_ShootStrengthen) as Buff_ShootStrengthen) != null)
		{
			num = buff_ShootStrengthen.StrengthenAmount;
		}
		Singleton<GameManager>.Instance.Player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponArrowEffectTakeEffect, (BattleEffectData)new SimpleEffectData
		{
			intData = 2
		}, out int IntData);
		if (targets != null && targets.Length != 0)
		{
			for (int i = 0; i < targets.Length; i++)
			{
				targets[i].TakeDamage(3 + num + IntData, Singleton<GameManager>.Instance.Player, isAbsDmg: true);
			}
		}
	}
}
