using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;

public class PlayViewPresenter: IViewPresenter
{
    //Model
    private TimeModel time;
    private ScoreModel score;
    //View
    private PlayViewUI ui;
    private PlayView playView;
    //MousePointer
    private MousePointer mousePointer;
    //Subscription
    private SingleAssignmentDisposable timeSub;
    private SingleAssignmentDisposable scoreSub;
    private SingleAssignmentDisposable mouseSub;

    //presenterの初期化
    public PlayViewPresenter()
    {
        //モデルの初期化
        time = new TimeModel();
        score = new ScoreModel();

        //Viewの初期化
        ui = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PlayViewUI")).GetComponent<PlayViewUI>();
        ui.Init();
        score.score.Subscribe(score => ui.SetScore(score));
        score.highScore.Subscribe(highScore => ui.SetHighScore(highScore));
        playView = new GameObject().AddComponent<PlayView>();
        playView.Init();
        playView.OnScoreUp.Subscribe(score => this.score.PlusCount(score.GetScore()));

        //マウスポインタの初期化
        mousePointer = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/MousePointer")).GetComponent<MousePointer>();
        mousePointer.Init();

        var task = OpenView();

    }

    //クリックしたときの挙動
    public void SubscribeMousePoiner()
    {
        mouseSub = new SingleAssignmentDisposable();
        mouseSub.Disposable = mousePointer.OnClick.TakeUntilDestroy(mousePointer).Subscribe(list =>
        {
            if (list.Count > 0)
            {
                bool wrong = true;

                int count = list.Count;

                for (int i = 0; i < count; i++)
                {
                    var clickable = list[0];

                    list.RemoveAt(0);

                    if (clickable.clickableObject == ClickableObject.FourClover)
                    {
                        var answertask = Clear();

                        wrong = false;
                    }
                    else if (clickable.clickableObject == ClickableObject.Shirotsume)
                    {
                        wrong = false;
                    }
                    else if (clickable.clickableObject == ClickableObject.Toy)
                    {
                        var task = CameraShake.Instance.Dig();

                        clickable.OnClicked();

                        wrong = false;
                    }
                    else if (clickable.clickableObject == ClickableObject.Bee)
                    {
                        clickable.OnClicked();

                        wrong = false;
                    }
                }

                if (wrong)
                {
                    Wrong();
                }

            }
        });
    }

    //TitleViewを開始
    public async UniTask OpenView()
    {
        await BlackOut.Instance.FadeIn();

        await playView.Standby();

        ui.Play();

        time.Start();
        timeSub = new SingleAssignmentDisposable();
        timeSub.Disposable = time.time.Subscribe(time =>
        {

            ui.SetTimer(time);

            if (time % 2 == 1)
            {
                playView.SetBee();
            }

            if (time <= 0)
            {
                TimeUp();
            }

        });

        SubscribeMousePoiner();

        AudioManager.Instance.PlaySE("Start");
    }

    //間違えた場合の挙動
    public void Wrong()
    {
        time.PlusTime(-5);

        CameraShake.Instance.Shake(0.5f, 3);

        AudioManager.Instance.PlaySE("Wrong");
    }

    //四つ葉を見つけたあとの挙動
    public async UniTask Clear()
    {
        time.PlusTime(5);
        time.Stop();

        await UniTask.DelayFrame(1);

        mouseSub.Dispose();
        timeSub.Dispose();

        await playView.Answer();

        await BlackOut.Instance.FadeOut();

        playView.CleanUp();

        await OpenView();

    }

    //時間切れ
    public void TimeUp()
    {
        time.Stop();

        AudioManager.Instance.PlaySE("Finish");

        mouseSub.Dispose();
        timeSub.Dispose();
    }

    //TitleSceneへ移動
    public async UniTask MoveToTitleView()
    {
        mouseSub.Dispose();
        timeSub.Dispose();

        await BlackOut.Instance.FadeOut();

        GameController.Instance.ChangeState(SceneState.Title);
    }

    //PlayViewを破棄
    public void Dispose()
    {
        mouseSub.Dispose();
        timeSub.Dispose();

        GameObject.Destroy(ui.gameObject);
        GameObject.Destroy(playView.gameObject);
        GameObject.Destroy(mousePointer.gameObject);
    }
}
