using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TestPlay : MonoBehaviour
{
	public Image img;

	public ParticleSystem par;

	public void TestExhaust()
	{
		par.Play();
		img.material.SetFloat("_ThreshHold", 1f);
		img.material.DOFloat(0f, "_ThreshHold", 2f);
	}
}
