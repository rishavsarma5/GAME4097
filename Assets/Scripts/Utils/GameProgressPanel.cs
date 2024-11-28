using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameProgressPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameProgressText;

    private void OnEnable()
    {
        var gameProgress = GameProgressManager.Instance.gameProgress;
        if (!gameProgress.gameStarted)
        {
            gameProgressText.text = "No Game Started.";
        } else
        {
            var timePlayed = FormatRuntime(gameProgress.GetRunTime());
            var currNumTurns = $"Turns Played: {gameProgress.numTurnsPlayed}/{gameProgress.totalTurns}";
            var currCluesFound = $"Clues Found: {gameProgress.numCluesFound}/{gameProgress.totalCluesCount}";
            var currWeaponsFound = $"Weapons Found: {gameProgress.numWeaponsFound}/{gameProgress.totalWeaponsCount}";

            gameProgressText.text = $"{timePlayed}\n{currNumTurns}\n{currCluesFound}\n{currWeaponsFound}";
        }
    }

    private string FormatRuntime(float timeElapsed)
    {
        int hours = Mathf.FloorToInt(timeElapsed / 3600);
        int minutes = Mathf.FloorToInt((timeElapsed % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);

        string formattedTime = string.Format("Time Played: {0:00}:{1:00}:{2:00}", hours, minutes, seconds);

        return formattedTime;
    }
}
