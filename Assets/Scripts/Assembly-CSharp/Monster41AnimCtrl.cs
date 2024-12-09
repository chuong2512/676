using Spine.Unity;

public class Monster41AnimCtrl : MonsterAnimCtrlBase
{
	[SpineAnimation("", "", true, false)]
	public string getShieldAnimName;

	[SpineAnimation("", "", true, false)]
	public string idleWithShieldAnimName;

	[SpineAnimation("", "", true, false)]
	public string loseShieldAnimName;

	public void GetShieldAnim()
	{
		base.State.SetAnimation(0, getShieldAnimName, loop: false);
		base.State.AddAnimation(1, idleWithShieldAnimName, loop: true, 0f);
	}

	public void LoseShieldAnim()
	{
		base.State.SetAnimation(0, loseShieldAnimName, loop: false);
		base.State.SetEmptyAnimation(1, 0.3f);
	}
}
