public class BluntArrow : Arrow
{
	public override ArrowType MArrowType => ArrowType.Blunt;

	protected override string EffectConfigName => "BluntArrow_Effect_EffectConfig";

	protected override string AddArrowEffectConfigName => "BluntArrow_Add_EffectConfig";

	protected override void OnArrowEffect(EntityBase[] targets)
	{
		if (targets != null && targets.Length != 0)
		{
			for (int i = 0; i < targets.Length; i++)
			{
				targets[i].GetBuff(new Buff_Shocked(targets[i], 2));
			}
		}
	}
}
