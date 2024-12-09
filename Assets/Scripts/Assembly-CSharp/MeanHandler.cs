using UnityEngine;

public abstract class MeanHandler
{
	protected const string MeanIconPatn = "Sprites/Mean";

	protected const string AtkTrigger = "AtkTrigger";

	protected const string StunTrigger = "StunTrigger";

	protected const string BuffTrigger = "BuffTrigger";

	protected const string DebuffTrigger = "DebuffTrigger";

	protected const string SpecialTrigger = "SpecialTrigger";

	protected const string SpeAtkTrigger = "SpeAtkTrigger";

	public abstract string GetSimpleDes0Str();

	public abstract string GetSimpleDes1Str();

	public abstract string GetDetailDesStr();

	public abstract string GetMeanTrigger();

	public abstract Sprite GetMeanIcon();
}
