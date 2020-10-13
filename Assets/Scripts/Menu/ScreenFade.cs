using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
	[SerializeField] Image background;

	public void FadeFromDefault(float duration, Action onComplete)
	{
		Color col = background.color;
		col.a = 1;
		StartCoroutine(FadeFromColour(col, duration, onComplete));
	}
	public void FadeToDefault(float duration, Action onComplete)
	{
		Color col = background.color;
		col.a = 0;
		StartCoroutine(FadeToColour(col, duration, onComplete));
	}
	public void FadeToBlack(float duration, Action onComplete)
	{
		StartCoroutine(FadeToColour(new Color(0, 0, 0, 0), duration, onComplete));
	}
	public void FadeToWhite(float duration, Action onComplete)
	{
		StartCoroutine(FadeToColour(new Color(1, 1, 1, 0), duration, onComplete));
	}

	IEnumerator FadeFromColour(Color col, float duration, Action onComplete)
	{
		while(col.a > 0)
		{
			col.a = Mathf.Clamp01(col.a - (Time.deltaTime / duration));
			background.color = col;
			yield return true;
		}
		if (onComplete != null) { onComplete(); }
	}
	IEnumerator FadeToColour(Color col, float duration, Action onComplete)
	{
		while (col.a < 1)
		{
			col.a = Mathf.Clamp01(col.a + (Time.deltaTime / duration));
			background.color = col;
			yield return true;
		}
		if (onComplete != null) { onComplete(); }
	}
}
