using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class TextPicMix : Text
{
	private List<Image> m_ImagesPool = new List<Image>();

	private List<int> m_ImagesVertexIndex = new List<int>();

	private static readonly Regex s_Regex = new Regex("<quad name=(.+?)size=(\\d*\\.?\\d+%?)width=(\\d*\\.?\\d+%?)/>", RegexOptions.Singleline);

	public override void SetVerticesDirty()
	{
		base.SetVerticesDirty();
		UpdateQuadImage();
	}

	protected void UpdateQuadImage()
	{
		m_ImagesVertexIndex.Clear();
		int num = 0;
		int num2 = 0;
		foreach (Match item2 in s_Regex.Matches(text))
		{
			int num3 = item2.Index - num + num2;
			string input = text.Substring(0, item2.Index);
			int num4 = Regex.Matches(input, " ").Count - num2;
			int count = Regex.Matches(input, "\n").Count;
			int num5 = num3 - (num4 + count);
			num += item2.Length;
			num2++;
			int item = num5 * 4 + 3;
			m_ImagesVertexIndex.Add(item);
			m_ImagesPool.RemoveAll((Image image) => image == null);
			if (m_ImagesPool.Count == 0)
			{
				GetComponentsInChildren(m_ImagesPool);
			}
			if (m_ImagesVertexIndex.Count > m_ImagesPool.Count)
			{
				GameObject gameObject = DefaultControls.CreateImage(default(DefaultControls.Resources));
				gameObject.layer = base.gameObject.layer;
				RectTransform rectTransform = gameObject.transform as RectTransform;
				if ((bool)rectTransform)
				{
					rectTransform.SetParent(base.rectTransform);
					rectTransform.localPosition = Vector3.zero;
					rectTransform.localRotation = Quaternion.identity;
					rectTransform.localScale = Vector3.one;
				}
				m_ImagesPool.Add(gameObject.GetComponent<Image>());
			}
			string value = item2.Groups[1].Value;
			float num6 = float.Parse(item2.Groups[2].Value);
			Image image2 = m_ImagesPool[m_ImagesVertexIndex.Count - 1];
			if (image2.sprite == null || image2.sprite.name != value)
			{
				image2.sprite = Resources.Load<Sprite>("Sprites/TextPic/" + value);
			}
			image2.rectTransform.sizeDelta = new Vector2(num6, num6);
			image2.enabled = true;
		}
		for (int i = m_ImagesVertexIndex.Count; i < m_ImagesPool.Count; i++)
		{
			if ((bool)m_ImagesPool[i])
			{
				m_ImagesPool[i].enabled = false;
			}
		}
	}

	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		base.OnPopulateMesh(toFill);
		for (int i = 0; i < m_ImagesVertexIndex.Count; i++)
		{
			int num = m_ImagesVertexIndex[i];
			RectTransform obj = m_ImagesPool[i].rectTransform;
			Vector2 sizeDelta = obj.sizeDelta;
			UIVertex vertex = default(UIVertex);
			toFill.PopulateUIVertex(ref vertex, num);
			obj.anchoredPosition = new Vector2(vertex.position.x + sizeDelta.x / 2f, vertex.position.y + sizeDelta.y / 2f);
			int num2 = num;
			int num3 = num - 3;
			while (num2 > num3)
			{
				toFill.PopulateUIVertex(ref vertex, num3);
				toFill.SetUIVertex(vertex, num2);
				num2--;
			}
		}
	}
}
