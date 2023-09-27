using UnityEngine;

public static class PlayerWinLossTracker
{
    private const string WINS_NOTE = "PlayerWins";
    private const string LOSSES_NOTE = "PlayerLosses";

    public static int GetPlayerLosses()
    {
        if (PlayerPrefs.HasKey(LOSSES_NOTE))
            return PlayerPrefs.GetInt(LOSSES_NOTE);
        else return 0;
    }

    public static int GetPlayerWins()
    {
        if (PlayerPrefs.HasKey(WINS_NOTE))
            return PlayerPrefs.GetInt(WINS_NOTE);
        else return 0;
    }

    public static void AddWin()
    {
        if (PlayerPrefs.HasKey(WINS_NOTE))
            PlayerPrefs.SetInt(WINS_NOTE, PlayerPrefs.GetInt(WINS_NOTE) + 1);
        else
            PlayerPrefs.SetInt(WINS_NOTE, 0);
    }

    public static void AddLoss()
    {
        if (PlayerPrefs.HasKey(LOSSES_NOTE))
            PlayerPrefs.SetInt(LOSSES_NOTE, PlayerPrefs.GetInt(LOSSES_NOTE) + 1);
        else
            PlayerPrefs.SetInt(LOSSES_NOTE, 0);
    }
}
