using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ScoreModel 
{
    string key = "SavedScore";

    public ReactiveProperty<int> score = new ReactiveProperty<int>(0);

    public ReactiveProperty<int> highScore = new ReactiveProperty<int>(0);

    public ScoreModel()
    {
        score.Value = 0;

        highScore.Value = PlayerPrefs.GetInt(key, 1);
    }

    public void PlusCount(int count)
    {
        this.score.Value += count;

        CheckScore();
    }

    public void SaveLevel()
    {
        PlayerPrefs.SetInt(key, highScore.Value);
        PlayerPrefs.Save();
    }
    public void CheckScore()
    {
        if (score.Value > highScore.Value)
        {
            highScore.Value= score.Value;

            SaveLevel();
        }

    }
}
