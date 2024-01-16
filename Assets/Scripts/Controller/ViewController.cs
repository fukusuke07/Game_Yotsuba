using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ViewController : SingletonMonoBehaviour<ViewController>
{
    private IViewPresenter presenter;

    public void Init(GameController gameController)
    {
        gameController.state.Subscribe(state => ChangeView(gameController.state.Value));
    }

    public void ChangeView(SceneState state)
    {
        if (presenter != null)
        {
            CloseView();
        }
        switch (state)
        {
            case SceneState.Title:
                BuildTitleView();
                break;
            case SceneState.Play:
                BuildPlayView();
                break;
            case SceneState.ToyCollection:
                BuildToyCollectionView();
                break;
        }
    }

    void BuildTitleView()
    {
        presenter = new TitleViewPresenter();
    }

    void BuildPlayView()
    {
        presenter = new PlayViewPresenter();
    }

    void BuildToyCollectionView()
    {
        presenter = new ToyViewPresenter();
    }

    void CloseView()
    {
        if (presenter != null) presenter.Dispose();

        presenter = null;
    }

}

