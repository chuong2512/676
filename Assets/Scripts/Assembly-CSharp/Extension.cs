using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class Extension
{
	public static bool CheckFileName(this string fileName, out string failResean)
	{
		bool flag = Regex.IsMatch(fileName, "(?!((^(con)$)|^(con)\\\\..*|(^(prn)$)|^(prn)\\\\..*|(^(aux)$)|^(aux)\\\\..*|(^(nul)$)|^(nul)\\\\..*|(^(com)[1-9]$)|^(com)[1-9]\\\\..*|(^(lpt)[1-9]$)|^(lpt)[1-9]\\\\..*)|^\\\\s+|.*\\\\s$)(^[^\\\\\\\\\\\\/\\\\:\\\\<\\\\>\\\\*\\\\?\\\\\\\\\\\\\"\\\\\\\\|]{1,255}$)");
		failResean = ((!flag) ? "FileNamingNotStandardized".LocalizeText() : string.Empty);
		return flag;
	}

	public static string LocalizeText(this string key)
	{
		return LocalizationManager.Instance.GetLocalizedValue(key.ToUpper());
	}

	public static Vector2 WithV2(this Vector2 origin, float? x = null, float? y = null)
	{
		return new Vector2(x ?? origin.x, y ?? origin.y);
	}

	public static Vector3 WithV3(this Vector3 origin, float? x = null, float? y = null, float? z = null)
	{
		return new Vector3(x ?? origin.x, y ?? origin.y, z ?? origin.z);
	}

	public static int Str2Int(this string str)
	{
		if (!int.TryParse(str, out var result))
		{
			Debug.LogError("Str can not translate to Int type!");
		}
		return result;
	}

	public static float Str2Float(this string str)
	{
		if (!float.TryParse(str, out var result))
		{
			Debug.Log("Str can not translate to Float type!");
		}
		return result;
	}

	public static bool IsNull(this object x)
	{
		return x == null;
	}

	public static bool IsNull<T>(this T? x) where T : struct
	{
		return !x.HasValue;
	}

	public static bool IsNullOrEmpty(this string str)
	{
		return string.IsNullOrEmpty(str);
	}

	public static Color ToColor(this string origin)
	{
		Color color = Color.white;
		ColorUtility.TryParseHtmlString(origin, out color);
		return color;
	}

	public static Color HexColorToColor(this string hexColorString)
	{
		if (string.IsNullOrEmpty(hexColorString))
		{
			return default(Color);
		}
		int num = int.Parse(hexColorString, NumberStyles.AllowHexSpecifier);
		float num2 = 255f;
		int num3 = 0xFF & num;
		int num4 = 0xFF00 & num;
		num4 >>= 8;
		int num5 = 0xFF0000 & num;
		num5 >>= 16;
		return new Color((float)((0xFF000000u & num) >> 24) / num2, (float)num5 / num2, (float)num4 / num2, (float)num3 / num2);
	}

	public static Tween TransformHint(this Transform trans)
	{
		trans.DOComplete();
		float x = trans.localScale.x;
		float endValue = x * 1.5f;
		Sequence sequence = DOTween.Sequence();
		sequence.Append(trans.DOScale(endValue, 0.1f)).Append(trans.DOScale(x, 0.4f));
		return sequence;
	}

	public static T[] RandomFromList<T>(this List<T> pool, int randomAmount, int randomSeed = -1)
	{
		if (pool == null)
		{
			throw new NullReferenceException("Pool cannot be null");
		}
		if (pool.Count < randomAmount)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (randomSeed >= 0)
		{
			UnityEngine.Random.InitState(randomSeed);
		}
		T[] array = new T[randomAmount];
		int num = 0;
		int min = 0;
		int num2 = pool.Count - randomAmount + 1;
		while (num < randomAmount)
		{
			int num3 = UnityEngine.Random.Range(min, num2);
			array[num] = pool[num3];
			num++;
			num2++;
			min = num3 + 1;
		}
		return array;
	}

	public static void TransformMoveByBezier(this Transform target, Vector3 startPoint, Vector3 middlePoint, Vector3 endPoint, float moveTime, Action endAction)
	{
		if (!(SingletonDontDestroy<Game>.Instance == null))
		{
			SingletonDontDestroy<Game>.Instance.StartCoroutine(TransformMoveByBezier_IE(target, startPoint, middlePoint, endPoint, moveTime, endAction));
		}
	}

	public static void TransformMoveByBezierWithoutGame(this MonoBehaviour mono, Transform target, Vector3 startPoint, Vector3 middlePoint, Vector3 endPoint, float moveTime, Action endAction)
	{
		mono.StartCoroutine(TransformMoveByBezier_IE(target, startPoint, middlePoint, endPoint, moveTime, endAction));
	}

	private static IEnumerator TransformMoveByBezier_IE(Transform target, Vector3 startPoint, Vector3 middlePoint, Vector3 endPoint, float moveTime, Action endAction)
	{
		float currentTime = 0f;
		while (currentTime < moveTime)
		{
			float t = currentTime / moveTime;
			Vector3 position = Bezier.GetV3Point(startPoint, middlePoint, endPoint, t);
			target.transform.position = position;
			currentTime += Time.deltaTime;
			yield return null;
		}
		endAction?.Invoke();
	}

	public static Tween ImageFlashHint(this Image targetImg, float flashTime = 0.2f)
	{
		targetImg.color = Color.clear;
		Sequence sequence = DOTween.Sequence();
		sequence.Append(targetImg.DOColor(Color.white, flashTime));
		sequence.Append(targetImg.DOColor(Color.clear, flashTime));
		return sequence;
	}

	public static bool IsPointerInputLegal(this int pointId)
	{
		if (pointId != -1)
		{
			return pointId == 0;
		}
		return true;
	}

	public static List<T> RandomListSort<T>(this List<T> list)
	{
		List<T> list2 = new List<T>(list.Count);
		foreach (T item in list)
		{
			list2.Insert(UnityEngine.Random.Range(0, list2.Count), item);
		}
		return list2;
	}

	public static Tween Fade(this Image image, float startAlpha, float endAlpha, float duration = 0.5f, Action completeAct = null)
	{
		image.color = image.color.WithCol(null, null, null, startAlpha);
		return image.DOFade(endAlpha, duration).OnComplete(delegate
		{
			completeAct?.Invoke();
		});
	}

	public static Tween Fade(this Text text, float startAlpha, float endAlpha, float duration = 0.5f, Action completeAct = null)
	{
		text.color = text.color.WithCol(null, null, null, startAlpha);
		return text.DOFade(endAlpha, duration).OnComplete(delegate
		{
			completeAct?.Invoke();
		});
	}

	public static Tween Fade(this CanvasGroup origin, float startAlpha, float endAlpha, float duration = 0.5f, Action completeAct = null)
	{
		origin.alpha = startAlpha;
		return origin.DOFade(endAlpha, duration).OnComplete(delegate
		{
			completeAct?.Invoke();
		});
	}

	public static Tween FadeCol(this Outline origin, Color startCol, Color endCol, float duration = 0.5f, Action completeAct = null)
	{
		origin.effectColor = startCol;
		return origin.DOColor(endCol, duration).OnComplete(delegate
		{
			completeAct?.Invoke();
		});
	}

	public static void KillTween(this Tween origin, bool isComplete = false)
	{
		if (origin != null && origin.IsActive())
		{
			origin.Kill(isComplete);
		}
	}

	public static Color WithCol(this Color origin, float? r = null, float? g = null, float? b = null, float? a = null)
	{
		return new Color(r ?? origin.r, g ?? origin.g, b ?? origin.b, a ?? origin.a);
	}

	public static void WithCol(this Text origin, float? a = null, float? r = null, float? g = null, float? b = null)
	{
		Color color = origin.color;
		color = (origin.color = new Color(r ?? color.r, g ?? color.g, b ?? color.b, a ?? color.a));
	}

	public static void WithCol(this Image origin, float? a = null, float? r = null, float? g = null, float? b = null)
	{
		Color color = origin.color;
		color = (origin.color = new Color(r ?? color.r, g ?? color.g, b ?? color.b, a ?? color.a));
	}

	public static void WithCol(this Outline origin, float? a = null, float? r = null, float? g = null, float? b = null)
	{
		Color color = origin.effectColor;
		color = (origin.effectColor = new Color(r ?? color.r, g ?? color.g, b ?? color.b, a ?? color.a));
	}

	public static string ToAllPlatformStreamingAssets(this string filePath)
	{
		return Path.Combine(Application.streamingAssetsPath, filePath);
	}

	public static string GetAllPlatformStreamingAssetsData(this string filePath)
	{
		_ = string.Empty;
		filePath = Path.Combine(Application.streamingAssetsPath, filePath);
		return File.ReadAllText(filePath);
	}
}
