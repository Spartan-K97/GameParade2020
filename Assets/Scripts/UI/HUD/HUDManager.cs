using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the heads up display
/// </summary>
public class HUDManager : MonoBehaviour
{
	[SerializeField] List<Sprite> tallies = new List<Sprite>();
	[SerializeField] Color activeColour = new Color(1, 1, 1, 1), greyedColour = new Color(1, 1, 1, 0.5f);
	[SerializeField] Text interactionText = null;

	private void SetTally(Image icon, Image tally, int count)
	{
		if (icon != null)
		{
			if (count >= tallies.Count)
			{
				Debug.LogError("Too many items to tally.");
			}
			else
			{
				tally.sprite = tallies[count];
				icon.color = (count > 0) ? activeColour : greyedColour;
			}
		}
	}

	public void SetInteractionText(string text)
	{
		interactionText.text = text;
	}

	#region human

	[SerializeField] Image key = null, keyTally = null, match = null, matchTally = null, sprint = null;

	public void SetNumKeys(int count)
	{
		SetTally(key, keyTally, count);
	}

	public void SetNumMatches(int count)
	{
		SetTally(match, matchTally, count);
	}
	public void SetCanSprint(bool yes)
	{
		if (sprint != null)
		{
			sprint.color = (yes) ? activeColour : greyedColour;
		}

	}

	#endregion

	#region monster

	[SerializeField] Image ward = null, wardTally = null;

	public void SetNumWards(int count)
	{
		SetTally(ward, wardTally, count);
	}

	#endregion
}