using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspectGuessUI : MonoBehaviour
{
	public GameObject suspectPanel;
	public GameObject weaponPanel;
	public GameObject spoonButton;
	public GameObject fireButton;

	public GameObject winPanel;
	public GameObject partialWinPanel;
	public GameObject partialLossPanel;
	public GameObject lossPanel;

	private int suspectGuess;
	private int weaponGuess;

	private void Start() {
		suspectPanel.SetActive(true);
		suspectGuess = 0;
		weaponGuess = 0;

		weaponPanel.SetActive(true);
		spoonButton.SetActive(false);
		fireButton.SetActive(false);


		winPanel.SetActive(false);
		partialWinPanel.SetActive(false);
		partialLossPanel.SetActive(false);
		lossPanel.SetActive(false);
	}

	public void setUp(List<Weapon> weaponsFound)
	{
		foreach (Weapon w in weaponsFound)
		{
			if (w.weaponName.Equals("Bar Spoon"))
			{
				spoonButton.SetActive(true);
			} else if (w.weaponName.Equals("Fire Extinguisher"))
			{
				fireButton.SetActive(true);
			}
			weaponPanel.SetActive(false);
		}
	}

	public void suspectChoice(Boolean correct)
	{
		if (correct)
		{
			this.suspectGuess = 1;
		} else
		{
			this.suspectGuess = -1;
		}
	}

	public void continueToWeapon()
	{
		if (suspectGuess != 0)
		{
			suspectPanel.SetActive(false);
			weaponPanel.SetActive(true);
		}
	}

	public void weaponChoice(Boolean correct)
	{
		if (correct)
		{
			this.weaponGuess = 1;
		}
		else
		{
			this.weaponGuess = -1;
		}
	}

	public void endScreen()
	{
		if (suspectGuess == 1 &&  weaponGuess == 1)
		{
			winPanel.SetActive(true);
		}
		if (suspectGuess == 1 && weaponGuess == -1) { 
			partialWinPanel.SetActive(true);
		}
		if (suspectGuess == -1 && weaponGuess == 1)
		{
			partialLossPanel.SetActive(true);
		}
		if (suspectGuess == -1 && weaponGuess == -1)
		{
			lossPanel.SetActive(true);
		}
	}
}
