using System.Collections;
using Spine.Unity;

public class HealthSpriteEventBlock : SpecialEventBlock
{
	[SpineAnimation("", "", true, false)]
	public string idleAnimName;

	private SkeletonGraphic skeleton;

	protected override void OnAwake()
	{
		base.OnAwake();
		skeleton = base.transform.Find("Anim").GetComponent<SkeletonGraphic>();
		skeleton.Initialize(overwrite: true);
	}

	private void OnEnable()
	{
		IdleAnim();
		StartCoroutine(SetSilbling_IE());
	}

	private IEnumerator SetSilbling_IE()
	{
		yield return null;
		RoomUI roomUI = SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI;
		base.transform.SetParent(roomUI.BossBlockRoot);
		base.transform.SetAsLastSibling();
	}

	public void IdleAnim()
	{
		if (skeleton.AnimationState == null)
		{
			skeleton.Initialize(overwrite: false);
		}
		skeleton.AnimationState.SetAnimation(0, idleAnimName, loop: true);
	}
}
