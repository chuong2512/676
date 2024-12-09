using System.Collections;
using UnityEngine;

public class BattleBg_6Ctrl : MonoBehaviour
{
	private SpriteRenderer eye01;

	private SpriteRenderer eye02;

	private SpriteRenderer lightBug01;

	private SpriteRenderer lightBug02;

	private SpriteRenderer magicLight;

	private float eyeCounter;

	private float eyeFlashTime;

	private float magicLightOffset;

	private float magicBug01Offset;

	private float magicBug02Offset;

	public float LightFunc01A;

	public float LightFunc01Fre;

	public float LightFunc02A;

	public float LightFunc02Fre;

	public float LightBaseFuncOffset;

	public float BugFunA;

	public float BugFre;

	public float BugOffset;

	private void Awake()
	{
		eye01 = base.transform.Find("Eyes_0").GetComponent<SpriteRenderer>();
		eye02 = base.transform.Find("Eyes_1").GetComponent<SpriteRenderer>();
		lightBug01 = base.transform.Find("LightBug_1").GetComponent<SpriteRenderer>();
		lightBug02 = base.transform.Find("LightBug_2").GetComponent<SpriteRenderer>();
		magicLight = base.transform.Find("MagicLight").GetComponent<SpriteRenderer>();
	}

	private void OnEnable()
	{
		ResetEyeCounter();
		magicLightOffset = Random.Range(0f, 6f);
		magicBug01Offset = Random.Range(0f, 6f);
		magicBug02Offset = Random.Range(0f, 6f);
	}

	private void Update()
	{
		eyeCounter += Time.deltaTime;
		if (eyeCounter >= eyeFlashTime)
		{
			FlashEye();
		}
		magicLight.color = new Color(1f, 1f, 1f, MagicLightAlphaFunction(Time.time, magicLightOffset));
		lightBug01.color = new Color(1f, 1f, 1f, BugLightAlphaFunction(Time.time, magicBug01Offset));
		lightBug02.color = new Color(1f, 1f, 1f, BugLightAlphaFunction(Time.time, magicBug02Offset));
	}

	private float BugLightAlphaFunction(float t, float offset)
	{
		return Mathf.Clamp01(BugFunA * Mathf.Sin(BugFre * t + offset) + BugOffset);
	}

	private float MagicLightAlphaFunction(float t, float offset)
	{
		return Mathf.Clamp01(LightFunc01A * Mathf.Sin(LightFunc01Fre * t + offset) + LightFunc02A * Mathf.Sin(LightFunc02Fre * t + offset) + LightBaseFuncOffset);
	}

	private void ResetEyeCounter()
	{
		eyeFlashTime = Random.Range(5f, 10f);
		eyeCounter = 0f;
		Color color2 = (eye01.color = (eye02.color = Color.clear));
	}

	private void FlashEye()
	{
		eyeFlashTime = 2.14748365E+09f;
		Color color2 = (eye01.color = (eye02.color = Color.white));
		StartCoroutine(FlashEye_IE());
	}

	private IEnumerator FlashEye_IE()
	{
		yield return new WaitForSeconds(2.5f);
		ResetEyeCounter();
	}
}
