public class SawtoothArrow : Arrow
{
	public override ArrowType MArrowType => ArrowType.Sawtooth;

	protected override string EffectConfigName => "SawtoothArrow_Effect_EffectConfig";

	protected override string AddArrowEffectConfigName => "SawtoothArrow_Add_EffectConfig";

	protected override void OnArrowEffect(EntityBase[] targets)
	{
		if (targets != null && targets.Length != 0)
		{
			for (int i = 0; i < targets.Length; i++)
			{
				targets[i].GetBuff(new Buff_Bleeding(targets[i], 2));
			}
		}
	}
}
