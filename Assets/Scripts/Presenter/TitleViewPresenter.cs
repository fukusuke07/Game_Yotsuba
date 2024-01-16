using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class TitleViewPresenter :IViewPresenter
{
    //Model
    private TimeModel time;
    private ScoreModel score;
    //View
    private TitleViewUI ui;
    private TitleView titleView;
    //マウスポインタ
    private MousePointer mousePointer;

    //presenterの初期化
    public TitleViewPresenter()
    {
        time = new TimeModel();
        score = new ScoreModel();

        //Viewの初期化
        titleView = new GameObject().AddComponent<TitleView>();
        ui = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/TitleViewUI")).GetComponent<TitleViewUI>();
        ui.Init();
        time.time.Subscribe(time => ui.SetTimer(time));
        score.score.Subscribe(score => ui.SetScore(score));
        score.highScore.Subscribe(highScore => ui.SetHighScore(highScore));

        //マウスポインタの初期化
        mousePointer = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/MousePointer")).GetComponent<MousePointer>();
        mousePointer.Init();
        mousePointer.OnClick.TakeUntilDestroy(mousePointer).Subscribe(list =>
        {
            if (list.Count > 0)
            {

                foreach (IClickable clickable in list)
                {
                    if(clickable.clickableObject == ClickableObject.FourClover)
                    {
                        var task = MoveToPlayScene();
                    }
                }
            }

        });

        //シーン開始
        var open = OpenView();
    }

    //TitleViewを開始
    private async UniTask OpenView()
    {
        await BlackOut.Instance.FadeIn();
        await titleView.GrowClover();
    }

    //PlaySceneへ移動
    private async UniTask MoveToPlayScene()
    {
        await titleView.DeleteClover();
        await BlackOut.Instance.FadeOut();
        GameController.Instance.ChangeState(SceneState.Play);
    }

    //TitleViewを破棄
    public void Dispose()
    {
        GameObject.Destroy(ui.gameObject);
        GameObject.Destroy(titleView.gameObject);
        GameObject.Destroy(mousePointer.gameObject);
    }
}
