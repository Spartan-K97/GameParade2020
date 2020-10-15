using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the heads up display
/// </summary>
public class HUDManager : MonoBehaviour
{
	[SerializeField] List<Sprite> tallies;
	[SerializeField] Image key, keyTally, match, matchTally, sprint;
	[SerializeField] Color activeColour, greyedColour;
	[SerializeField] Text interactionText;
	private int numKeys;
	private int numMatches;
	private bool canSprint;

	public void AddKey()
	{
		++numKeys;
		SetTally(key, keyTally, numKeys);
	}
	public void RemoveKey()
	{
		--numKeys;
		SetTally(key, keyTally, numKeys);
	}

	public void AddMatch()
	{
		++numMatches;
		SetTally(match, matchTally, numMatches);
	}
	public void RemoveMatch()
	{
		--numMatches;
		SetTally(match, matchTally, numMatches);
	}

	public void SetCanSprint(bool yes)
	{
		canSprint = yes;
		sprint.color = (yes) ? activeColour : greyedColour;
	}

	public void SetInteractionText(string text)
	{
		interactionText.text = text;
	}

	private void SetTally(Image icon, Image tally, int count)
	{
		if (count >= tallies.Count)
		{
			Debug.LogError("Too many items to tally.");
		}
		else
		{
			tally.sprite = tallies[count];
			sprint.color = (count == 0) ? activeColour : greyedColour;
		}
	}
}