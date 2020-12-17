using System;

[Serializable]
public class PlayerHighScoreData
{
    public int highScore;

    public PlayerHighScoreData(int highScore)
    {
        this.highScore = highScore;
    }
}