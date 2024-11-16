using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspectGuessUI : MonoBehaviour
{
	public GameObject suspectPanel;
	public GameObject weaponPanel;
	public GameObject spoonButton;
	public GameObject golfButton;
	public GameObject dartButton;

	public GameObject winPanel;
	public GameObject partialWinPanel;
	public GameObject partialLossPanel;
	public GameObject lossPanel;

	private int suspectGuess = 0;
	private int weaponGuess = 0;

	private List<int> foundWeapons;

	private void Start() {
		foundWeapons = new List<int>();
		suspectPanel.SetActive(true);
		suspectGuess = 0;
		weaponGuess = 0;

		weaponPanel.SetActive(true);
		spoonButton.SetActive(false);
		golfButton.SetActive(false);
		dartButton.SetActive(false);


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
				foundWeapons.Add(1);

			} 
			else if (w.weaponName.Equals("Poison Dart"))
			{
				dartButton.SetActive(true);
				foundWeapons.Add(2);
			} 
			else if (w.weaponName.Equals("Golf Club"))
			{
				golfButton.SetActive(true);
				foundWeapons.Add(3);
			}
			weaponPanel.SetActive(false);
		}
	}

	public void suspectChoice(int characterNum)
	{
		this.suspectGuess = characterNum;
	}

	public void continueToWeapon()
	{
		if (suspectGuess != 0)
		{
			suspectPanel.SetActive(false);
			weaponPanel.SetActive(true);
		}
	}

	public void weaponChoice(int characterNum)
	{
		this.weaponGuess = characterNum;
	}

	public void endScreen()
	{
			if (suspectGuess == weaponGuess)
			{
				winPanel.SetActive(true);
			}
			else if (foundWeapons.Contains(suspectGuess))
			{
				partialWinPanel.SetActive(true);
			}
			else
			{
				lossPanel.SetActive(true);
			}
	}
}
