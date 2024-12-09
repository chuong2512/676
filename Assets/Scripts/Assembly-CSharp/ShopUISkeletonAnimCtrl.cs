using Spine;
using Spine.Unity;
using UnityEngine;

public class ShopUISkeletonAnimCtrl : MonoBehaviour
{
	[SpineAnimation("", "", true, false)]
	public string idleAnimName;

	[SpineAnimation("", "", true, false)]
	public string purchasedAnimName;

	private Spine.AnimationState state;

	private SkeletonGraphic skeletonGraphic;

	private void Awake()
	{
		skeletonGraphic = GetComponent<SkeletonGraphic>();
		state = skeletonGraphic.AnimationState;
	}

	public void PlayIdle()
	{
		state.SetAnimation(0, idleAnimName, loop: true);
	}

	public void PlayPurchasedAnim()
	{
		state.SetAnimation(0, purchasedAnimName, loop: false);
		state.AddAnimation(0, idleAnimName, loop: true, 0f);
	}
}
