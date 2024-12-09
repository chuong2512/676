using Spine.Unity;

public class Monster34AnimCtrl : MonsterAnimCtrlBase
{
	[SpineAnimation("", "", true, false)]
	public string defenceIdleAnimName;

	[SpineAnimation("", "", true, false)]
	public string idleToDefenceAnimName;

	[SpineAnimation("", "", true, false)]
	public string defenceToIdleAnimName;

	public void IdleToDefenceAnim()
	{
		base.State.SetAnimation(0, idleToDefenceAnimName, loop: false);
		base.State.AddAnimation(0, defenceIdleAnimName, loop: true, 0f);
	}

	public void DefenceToIdleAnim()
	{
		base.State.SetAnimation(0, defenceToIdleAnimName, loop: false);
		base.State.AddAnimation(0, idleAnimName, loop: true, 0f);
	}
}
