using Spine.Unity;

public class Monster65AnimCtrl : MonsterAnimCtrlBase
{
	[SpineAnimation("", "", true, false)]
	public string canGetHurtIdleAnim;

	public void ClearStat()
	{
		base.State.SetEmptyAnimation(1, 0.3f);
	}

	public void SetCanGetHurtStat()
	{
		base.State.SetAnimation(1, canGetHurtIdleAnim, loop: true);
	}
}
