using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class ToyViewPresenter :IViewPresenter
{
    //View
    private ToyViewUI ui;
    private ToyView toyView;
    //マウスポインタ
    private MousePointer mousePointer;

    //presenterの初期化
    public ToyViewPresenter()
    {
        //Viewの初期化
        toyView = new GameObject().AddComponent<ToyView>();
        toyView.BuryToys();
        ui = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ToyViewUI")).GetComponent<ToyViewUI>();
        ui.Init();
        ui.SetToyCount();

        //マウスポインタの初期化
        mousePointer = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/MousePointer")).GetComponent<MousePointer>();
        mousePointer.Init();
        mousePointer.OnClick.TakeUntilDestroy(mousePointer).Subscribe(list =>
        {
            if (list.Count > 0)
            {
                int count = list.Count;

                for (int i = 0; i < count; i++)
                {
                    var clickable = list[0];

                    list.RemoveAt(0);

                    clickable.OnClicked();
                }
            }

        });

        //シーン開始
        var task = OpenView();
    }

    //ToyViewを開始
    private async UniTask OpenView()
    {
        
        await BlackOut.Instance.FadeIn();
    }

    //ToyViewを破棄
    public void Dispose()
    {
        GameObject.Destroy(ui.gameObject);
        GameObject.Destroy(toyView.gameObject);
        GameObject.Destroy(mousePointer.gameObject);
    }
}
