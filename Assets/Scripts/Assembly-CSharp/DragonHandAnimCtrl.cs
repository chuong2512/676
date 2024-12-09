using System.Collections.Generic;
using Spine.Unity;

public class DragonHandAnimCtrl : MonsterAnimCtrlBase
{
	private Monster50AnimCtrl _monster50AnimCtrl;

	private bool isLeft;

	[SpineAnimation("", "", true, false)]
	public string action4AnimName;

	public override void Init()
	{
	}

	public void SetDragonHeadAnimCtrl(Monster50AnimCtrl monster50AnimCtrl, bool isLeft)
	{
		_monster50AnimCtrl = monster50AnimCtrl;
		this.isLeft = isLeft;
	}

	public override void PlaySkeletonAnim(List<EnemySkeletonAnimEffectStep.EnemySkeletonEffect> animList)
	{
		_monster50AnimCtrl.PlaySkeletonAnim(animList);
	}

	public override void PlaySkeletonAnim(EnemySkeletonAnimEffectStep.EnemySkeletonEffect effect)
	{
		_monster50AnimCtrl.PlaySkeletonAnim(effect);
	}

	public override void ChangeSkin(string skinName)
	{
		_monster50AnimCtrl.ChangeSkin(skinName);
	}

	public override void FlashWhite()
	{
		if (isLeft)
		{
			_monster50AnimCtrl.FlashWhite_LeftHand();
		}
		else
		{
			_monster50AnimCtrl.FlashWhite_RightHand();
		}
	}

	public void Dead()
	{
		if (isLeft)
		{
			_monster50AnimCtrl.LeftHandDead();
		}
		else
		{
			_monster50AnimCtrl.RightHandDead();
		}
	}
}
