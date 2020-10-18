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
	[SerializeField] Sprite skey = null, sgreyKey = null, smatch = null, sgreyMatch = null, sward = null, sgreyWard = null;
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
		if (key != null) { key.sprite = (count > 0) ? skey : sgreyKey; }
	}

	public void SetNumMatches(int count)
	{
		SetTally(match, matchTally, count);
		if (match != null) { match.sprite = (count > 0) ? smatch : sgreyMatch; }
	}
	public void SetCanSprint(bool yes)
	{
		// Removed
	}

	#endregion

	#region monster

	[SerializeField] Image ward = null, wardTally = null;

	public void SetNumWards(int count)
	{
		SetTally(ward, wardTally, count);
		if (ward != null) { ward.sprite = (count > 0) ? sward : sgreyWard; }
	}

	#endregion
}