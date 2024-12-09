public abstract class SpecialUsualCard : UsualCard
{
	protected SpecialUsualCardAttr specialUsualCardAttr;

	protected BaseEffectConfig SupEffectConfig
	{
		get
		{
			if (!specialUsualCardAttr.EffectConfigSupName.IsNullOrEmpty())
			{
				return SingletonDontDestroy<ResourceManager>.Instance.LoadScriptObject<BaseEffectConfig>(specialUsualCardAttr.EffectConfigSupName, "EffectConfigScriObj");
			}
			return null;
		}
	}

	public override bool IsWillBreakDefence => true;

	protected SpecialUsualCard(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		specialUsualCardAttr = (SpecialUsualCardAttr)usualCardAttr;
	}

	protected override bool IsSatisfySpecialStatus(Player player)
	{
		return true;
	}
}
