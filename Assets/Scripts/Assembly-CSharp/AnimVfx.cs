public class AnimVfx : VfxBase
{
	protected override void PlayVfx(bool isMute)
	{
	}

	public void OnAnimEnd()
	{
		Recycle();
	}

	protected override void OnRecycle()
	{
	}

	protected override void OnInit()
	{
	}
}
