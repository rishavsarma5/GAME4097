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
    public int turnsLeft;

    public Vector3 currentPlayerPosition;
    public Vector3 diceRollPlayerPosition;
    public int lastDiceRoll;

    public bool gameStarted = false;
    public bool continueGame = false;

    public float GetRunTime()
    {
        timeElapsed = Time.time - startTime;
        Debug.Log("Total run time: " + startTime.ToString());
        return timeElapsed;
    }

    public void SaveStartTime()
    {
        PlayerPrefs.SetFloat("TimeStarted", startTime);
    }

    
    /*
    public void SaveAllGameProgress()
    {
        var didGameStart = gameStarted ? 1 : 0;
        PlayerPrefs.SetInt("GameStarted", didGameStart);
        PlayerPrefs.SetFloat("TimeStarted", startTime);
        PlayerPrefs.SetFloat("ElapsedTime", timeElapsed);
        
        SaveClues();
        SaveWeapons();
        SaveTurns();
    }
    */
    

    public void SavePlayerPosition(Vector3 playerPosition)
    {
        diceRollPlayerPosition = playerPosition;
        PlayerPrefs.SetFloat("PlayerPositionX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerPositionY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", playerPosition.z);
    }

    public void SaveDiceRollPosition(Vector3 playerPosition)
    {
        currentPlayerPosition = playerPosition;
        PlayerPrefs.SetFloat("DiceRollPositionX", playerPosition.x);
        PlayerPrefs.SetFloat("DiceRollPositionY", playerPosition.y);
        PlayerPrefs.SetFloat("DiceRollPositionZ", playerPosition.z);
    }

    public void LoadGameProgress()
    {
        gameStarted = PlayerPrefs.GetInt("GameStarted", 0) == 1;
        startTime = PlayerPrefs.GetFloat("TimeStarted", 0);
        timeElapsed = PlayerPrefs.GetFloat("ElapsedTime", 0);

        numCluesFound = PlayerPrefs.GetInt("CluesFound", 0);
        totalCluesCount = PlayerPrefs.GetInt("TotalClues", 0);

        numWeaponsFound = PlayerPrefs.GetInt("WeaponsFound", 0);
        totalWeaponsCount = PlayerPrefs.GetInt("TotalWeapons", 0);

        numTurnsPlayed = PlayerPrefs.GetInt("TurnsPlayed", 0);
        totalTurns = PlayerPrefs.GetInt("TotalTurns", 0);
        turnsLeft = PlayerPrefs.GetInt("TurnsLeft", 0);
        lastDiceRoll = PlayerPrefs.GetInt("LastDiceRoll", 1);

        // Set default or saved player position
        currentPlayerPosition = PlayerPrefs.HasKey("PlayerPositionX")
            ? new Vector3(
                PlayerPrefs.GetFloat("PlayerPositionX"),
                PlayerPrefs.GetFloat("PlayerPositionY"),
                PlayerPrefs.GetFloat("PlayerPositionZ"))
            : Vector3.zero;

        // Set default or saved player position
        diceRollPlayerPosition = PlayerPrefs.HasKey("DiceRollPositionX")
            ? new Vector3(
                PlayerPrefs.GetFloat("DiceRollPositionX"),
                PlayerPrefs.GetFloat("DiceRollPositionY"),
                PlayerPrefs.GetFloat("DiceRollPositionZ"))
            : Vector3.zero;
    }

    public void ResetGame()
    {
        gameStarted = false;
        continueGame = false;
        startTime = 0;
        timeElapsed = 0;
        totalCluesCount = 0;
        numCluesFound = 0;
        totalWeaponsCount = 0;
        numWeaponsFound = 0;
        numTurnsPlayed = 0;
        totalTurns = 0;
        lastDiceRoll = 0;
        currentPlayerPosition = Vector3.zero;
        diceRollPlayerPosition = Vector3.zero;

        PlayerPrefs.DeleteAll();
    }

    public void SaveFoundClues(int currFoundClues)
    {
        numCluesFound = currFoundClues;
        PlayerPrefs.SetInt("CluesFound", numCluesFound);
    }

    public void SaveTotalClues(int totalClues)
    {
        totalCluesCount = totalClues;
        PlayerPrefs.SetInt("TotalClues", totalCluesCount);
    }

    public void SaveTotalWeapons(int totalWeapons)
    {
        totalWeaponsCount = totalWeapons;
        PlayerPrefs.SetInt("TotalWeapons", totalWeaponsCount);
    }

    public void SaveFoundWeapons(int currFoundWeapons)
    {
        numWeaponsFound = currFoundWeapons;
        PlayerPrefs.SetInt("WeaponsFound", numWeaponsFound);
    }

    public void SaveNumTurnsPlayed(int currNumTurns)
    {
        numTurnsPlayed = currNumTurns;
        PlayerPrefs.SetInt("TurnsPlayed", numTurnsPlayed);
    }

    public void SaveTotalTurns(int turnCount)
    {
        totalTurns = turnCount;
        PlayerPrefs.SetInt("TotalTurns", totalTurns);
    }

    public void SaveTurnsLeft(int turnCount)
    {
        turnsLeft = turnCount;
        PlayerPrefs.SetInt("TurnsLeft", turnsLeft);
    }

    public void SaveLastDiceRoll(int diceValue)
    {
        lastDiceRoll = diceValue;
        PlayerPrefs.SetInt("LastDiceRoll", lastDiceRoll);
    }
}
