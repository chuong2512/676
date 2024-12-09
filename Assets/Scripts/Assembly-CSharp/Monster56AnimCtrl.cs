public class Monster56AnimCtrl : MonsterAnimCtrlBase
{
	public void PlaySingleIdle(int index)
	{
		base.State.SetAnimation(0, $"Idle{index}", loop: true);
	}
}
