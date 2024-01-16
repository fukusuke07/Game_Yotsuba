using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    string key = "SavedScore";

    public int score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    {
        score = PlayerPrefs.GetInt(key, 1);

        DontDestroyOnLoad(this.gameObject);
    }
    public void SaveLevel()
    {
        PlayerPrefs.SetInt(key, score);
        PlayerPrefs.Save();
    }
    public void CheckScore(int _score)
    {
        if (score < _score)
        {
            score = _score;

            SaveLevel();
        }

        
    }
}