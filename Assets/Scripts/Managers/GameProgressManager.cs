using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance;

    public GameProgress gameProgress;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // starts the game time
    void Start()
    {
        gameProgress.startTime = Time.time;
        gameProgress.SaveStartTime();
    }

    public void SaveTotalClues(int clueCount)
    {
        if (gameProgress.totalCluesCount != clueCount)
        {
            gameProgress.SaveTotalClues(clueCount);
        }
    }

    public void SaveFoundClues(int currFoundClues)
    {
        if (gameProgress.numCluesFound != currFoundClues)
        {
            gameProgress.SaveFoundClues(currFoundClues);
        }
    }

    public void SaveTotalWeapons(int weaponCount)
    {
        if (gameProgress.totalWeaponsCount != weaponCount)
        {
            gameProgress.SaveTotalWeapons(weaponCount);
        }
    }

    public void SaveFoundWeapons(int currFoundWeapons)
    {
        if (gameProgress.numWeaponsFound != currFoundWeapons)
        {
            gameProgress.SaveFoundWeapons(currFoundWeapons);
        }
    }

    public void SavePlayerPosition(Vector3 currPlayerPos)
    {
        if (gameProgress.currentPlayerPosition != currPlayerPos)
        {
            gameProgress.SavePlayerPosition(currPlayerPos);
        }
    }

    public void SaveTotalTurns(int turnCount)
    {
        if (gameProgress.totalTurns != turnCount)
        {
            gameProgress.SaveTotalTurns(turnCount);
        }
    }

    public void SaveTurnsPlayed(int numsTurnsPlayed)
    {
        if (gameProgress.numTurnsPlayed != numsTurnsPlayed)
        {
            gameProgress.SaveNumTurnsPlayed(numsTurnsPlayed);
        }
    }
}
