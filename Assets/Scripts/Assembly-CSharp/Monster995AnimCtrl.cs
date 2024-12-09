using Spine.Unity;

public class Monster995AnimCtrl : MonsterAnimCtrlBase
{
	[SpineAnimation("", "", true, false)]
	public string defenceIdleAnimName;

	[SpineAnimation("", "", true, false)]
	public string idleToDefenceAnimName;

	[SpineAnimation("", "", true, false)]
	public string defenceToIdleAnimName;

	public void DefenceIdleAnim()
	{
		base.State.SetAnimation(0, defenceIdleAnimName, loop: true);
	}

	public void IdleToDefenceAnim()
	{
		base.State.SetAnimation(0, idleToDefenceAnimName, loop: false);
	}

	public void DefenceToIdleAnim()
	{
		base.State.SetAnimation(0, defenceToIdleAnimName, loop: false);
	}
}
