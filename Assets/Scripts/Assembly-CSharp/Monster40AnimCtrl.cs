using Spine.Unity;

public class Monster40AnimCtrl : MonsterAnimCtrlBase
{
	[SpineAnimation("", "", true, false)]
	public string idleAAnimName;

	[SpineAnimation("", "", true, false)]
	public string idleBAnimName;

	[SpineAnimation("", "", true, false)]
	public string AtoBAnimName;

	[SpineAnimation("", "", true, false)]
	public string BtoAAnimName;

	public void AtoBAnim()
	{
		base.State.SetAnimation(0, AtoBAnimName, loop: false);
		base.State.AddAnimation(0, idleBAnimName, loop: true, 0f);
	}

	public void BtoAAnim()
	{
		base.State.SetAnimation(0, BtoAAnimName, loop: false);
		base.State.AddAnimation(0, idleAAnimName, loop: true, 0f);
	}
}
