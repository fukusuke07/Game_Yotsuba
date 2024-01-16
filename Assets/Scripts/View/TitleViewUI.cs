using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class TitleViewUI : MonoBehaviour
{

    private ScoreCounter timer;

    private ScoreCounter score;

    private ScoreCounter highScore;

    private Button toyButton;

    public void Init()
    {
        timer = transform.Find("Panel/UIBar/Time").GetComponent<ScoreCounter>();
        timer.Init();

        score = transform.Find("Panel/UIBar/Score").GetComponent<ScoreCounter>();
        score.Init();

        highScore = transform.Find("Panel/UIBar/HighScore").GetComponent<ScoreCounter>();
        highScore.Init();

        toyButton = transform.Find("Panel/ToyButton").GetComponent<Button>();
        toyButton.onClick.AddListener(ChangeToToyView);
    }

    public void SetScore(int _score)
    {
        score.Calculate(_score);
    }

    public void SetHighScore(int _highScore)
    {
        highScore.Calculate(_highScore);
    }

    public void SetTimer(int _time)
    {
        timer.Calculate(_time);
    }


    public void ChangeToToyView()
    {
        var task = DoChangeToToyView();
    }

    public async UniTask DoChangeToToyView()
    {
        await BlackOut.Instance.FadeOut();

        GameController.Instance.ChangeState(SceneState.ToyCollection);
    }
}
