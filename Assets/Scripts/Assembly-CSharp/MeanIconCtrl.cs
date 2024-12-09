using UnityEngine;
using UnityEngine.UI;

public class MeanIconCtrl : MonoBehaviour
{
	private Text meanDes0Text;

	private Text meanDes1Text;

	private MeanHandler meanHandler;

	private Animator m_Animator;

	private void Awake()
	{
		meanDes0Text = base.transform.Find("Des_0").GetComponent<Text>();
		meanDes1Text = base.transform.Find("Des_1").GetComponent<Text>();
		m_Animator = GetComponent<Animator>();
	}

	public void SetMean(MeanHandler handler)
	{
		meanHandler = handler;
		meanDes0Text.text = meanHandler.GetSimpleDes0Str();
		meanDes1Text.text = meanHandler.GetSimpleDes1Str();
		m_Animator.SetTrigger(handler.GetMeanTrigger());
	}
}
