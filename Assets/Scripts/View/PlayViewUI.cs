using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class PlayViewUI : MonoBehaviour
{
    //Object
    private ScoreCounter score;
    private ScoreCounter timer;
    private ScoreCounter highScore;
    private Button returnTitleButton;

    public void Init()
    {
        score = transform.Find("Panel/UIBar/Score").GetComponent<ScoreCounter>();
        score.Init();

        timer = transform.Find("Panel/UIBar/Time").GetComponent<ScoreCounter>();
        timer.Init();

        highScore = transform.Find("Panel/UIBar/HighScore").GetComponent<ScoreCounter>();
        highScore.Init();

        returnTitleButton = transform.Find("Panel/ReturnButton").GetComponent<Button>();
        returnTitleButton.onClick.AddListener(ReturnToTitleView);
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

    public void MinusTime(int minus)
    {

    }

    public void Play()
    {
        returnTitleButton.interactable = true;
    }

    public void ReturnToTitleView()
    {
        var task = DoReturnToTitleView();
    }

    public async UniTask DoReturnToTitleView()
    {
        returnTitleButton.interactable = false;

        await BlackOut.Instance.FadeOut();

        GameController.Instance.ChangeState(SceneState.Title);
    }

}
