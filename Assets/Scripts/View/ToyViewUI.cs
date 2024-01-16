using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class ToyViewUI : MonoBehaviour
{
    //Object
    private ScoreCounter toyCount;
    private Button returnTitleButton;

    public void Init()
    {
        toyCount = transform.Find("Panel/UIBar/Collection").GetComponent<ScoreCounter>();
        toyCount.Init();

        returnTitleButton = transform.Find("Panel/ReturnButton").GetComponent<Button>();
        returnTitleButton.onClick.AddListener(ReturnToTitleView);
    }

    public void SetToyCount()
    {
        int count = 0;
        ItemPossessionList itemPossessionList = JsonDataManager.Load();
        for(int i = 0; i < itemPossessionList.itemPossessonList.Length; i++)
        {
            if(itemPossessionList.itemPossessonList[i]) count += 1;
        }
        toyCount.Calculate(count);
    }

    public void ReturnToTitleView()
    {
        var task = DoReturnToTitleView();
    }

    public async UniTask DoReturnToTitleView()
    {
        await BlackOut.Instance.FadeOut();

        GameController.Instance.ChangeState(SceneState.Title);
    }
}
