
using UnityEngine;

public static class PlayerPreferenceManager
{
    public static int GetScore()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            return PlayerPrefs.GetInt("Score");
        }
        else
        {
            return 0;
        }
    }

    public static void SetScore(int score)
    {
        PlayerPrefs.SetInt("Score", score);
    }

    public static int GetHighscore()
    {
        if (PlayerPrefs.HasKey("Highscore"))
        {
            return PlayerPrefs.GetInt("Highscore");
        }
        else
        {
            return 0;
        }
    }

    public static void SetHighscore(int highscore)
    {
        PlayerPrefs.SetInt("Highscore", highscore);
    }

}
