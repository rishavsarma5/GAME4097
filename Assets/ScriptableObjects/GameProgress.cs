using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameProgress", menuName = "GameProgress")]
public class GameProgress : ScriptableObject
{
    public float startTime;
    public float timeElapsed;
    public int totalCluesCount;
    public int numCluesFound;
    public int totalWeaponsCount;
    public int numWeaponsFound;
    public int numTurnsPlayed;
    public int totalTurns;

    public Vector3 currentPlayerPosition;

    public bool gameStarted = false;

    public float GetRunTime()
    {
        timeElapsed = Time.time - startTime;
        Debug.Log("Total run time: " + startTime.ToString());
        return timeElapsed;
    }

    public void SaveGameProgress()
    {
        var didGameStart = gameStarted ? 1 : 0;
        PlayerPrefs.SetInt("GameStarted", didGameStart);
        PlayerPrefs.SetFloat("TimeStarted", startTime);
        PlayerPrefs.SetFloat("ElapsedTime", timeElapsed);
        
        SaveClues();
        SaveWeapons();
        SaveTurns();
    }

    public void ResetGame()
    {
        gameStarted = false;
        startTime = 0;
        timeElapsed = 0;
        totalCluesCount = 0;
        numCluesFound = 0;
        totalWeaponsCount = 0;
        numWeaponsFound = 0;
        numTurnsPlayed = 0;
        totalTurns = 0;

        PlayerPrefs.DeleteAll();
    }

    private void SaveClues()
    {
        PlayerPrefs.SetInt("CluesFound", numCluesFound);
        PlayerPrefs.SetInt("TotalClues", totalCluesCount);
    }

    private void SaveWeapons()
    {
        PlayerPrefs.SetInt("WeaponsFound", numWeaponsFound);
        PlayerPrefs.SetInt("TotalWeapons", totalWeaponsCount);
    }

    private void SaveTurns()
    {
        PlayerPrefs.SetInt("TurnsPlayed", numTurnsPlayed);
        PlayerPrefs.SetInt("TotalTurns", totalTurns);
    }
}
