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
    }

    public void SetTotalCluesCount(int clueCount)
    {
        gameProgress.totalCluesCount = clueCount;
    }

    public void SetTotalCluesFound(int cluesFound)
    {
        gameProgress.numCluesFound = cluesFound;
    }

    public void SetTotalWeaponCount(int weaponCount)
    {
        gameProgress.totalWeaponsCount = weaponCount;
    }

    public void SetTotalWeaponsFound(int weaponsFound)
    {
        gameProgress.numWeaponsFound = weaponsFound;
    }

    public void SetTotalTurns(int turns)
    {
        gameProgress.totalTurns = turns;
    }

    public void SetNumTurnsPlayed(int turns)
    {
        gameProgress.numTurnsPlayed = turns;
    }
}
