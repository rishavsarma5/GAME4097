using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuspectGuessUI : MonoBehaviour
{
	public WinManager winManager;
	public GuessSuspectSpace[] suspectSpaces;
	public GuessWeaponSpace[] weaponSpaces;

	public TMP_Text suspectBlank;
	public TMP_Text weaponBlank;

	private List<Weapon> foundWeapons;
	private List<string> suspectOrder;

	private Weapon weaponGuess;
	private string suspectGuess;

	private bool lockedIn;

	private void Start()
	{
		lockedIn = false;
		foundWeapons = ClueGameManager.Instance.foundWeapons;
		suspectOrder = new List<string>();

		int i = 0;
		foreach(Weapon weapon in foundWeapons)
		{
			weaponSpaces[i].Init(i, weapon.icon);
			i++;
		}
		foreach(GuessSuspectSpace space in suspectSpaces)
		{
			suspectOrder.Add(space.subject);
		}

	}

	public void HandleGuess(int index)
	{
		if (weaponGuess != null)
		{
			weaponSpaces[foundWeapons.IndexOf(weaponGuess)].Deselect();
		}
		weaponGuess = foundWeapons[index];
		weaponBlank.text = weaponGuess.weaponName;
		weaponSpaces[index].SetSelected();
	}

	public void HandleGuess(string subject)
	{
		if(suspectGuess != null)
		{
			suspectSpaces[suspectOrder.IndexOf(suspectGuess)].Deselect();
		}
		suspectGuess = subject;
		suspectBlank.text = suspectGuess;
		suspectSpaces[suspectOrder.IndexOf(subject)].SetSelected();
	}

	public void FinalGuess()
	{
		if (!lockedIn)
		{
			lockedIn = true;
			if (weaponGuess.relatedNPC.npcName == suspectGuess)
			{
				winManager.WinGame();
			}
			else
			{
				winManager.LoseGame();
			}
		}
	}
	


}
