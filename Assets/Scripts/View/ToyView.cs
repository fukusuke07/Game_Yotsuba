using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class ToyView : MonoBehaviour
{
    //Object
    private List<ToyGround> toyGrounds = new List<ToyGround>();
    private List<Toy> toys = new List<Toy>();

    //Toyを地面に埋める
    public void BuryToys()
    {
        //セーブデータから見つけたToyを取得
        ItemPossessionList itemPossessionList = JsonDataManager.Load();

        //発見したToyを地面に埋める
        for (int i = 0; i < 16; i++)
        {
            if (itemPossessionList.itemPossessonList[i] == true)
            {
                var toyGround = ObjectPoolManager.Instance.CreateToyGround();

                toyGrounds.Add(toyGround);

                toyGround.number = i;

                toyGround.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.ToyDic["Toy" + i.ToString()];

                toyGround.transform.position = new Vector3(-140 + 40 * (i % 8), 42 - 96 * (i / 8), 0);

                Debug.Log(toyGround.transform.position);

                toyGround.OnDig.TakeUntilDisable(toyGround).Subscribe(_=>
                {
                    Debug.Log(toyGround.number);
                    AudioManager.Instance.PlaySE("Get");
                    AudioManager.Instance.PlaySEOnBigVolume("Dig");
                    var b = ObjectPoolManager.Instance.CreateToy();
                    toys.Add(b);
                    var shadow = b.transform.Find("Shadow");
                    b.number = toyGround.number;
                    b.UpdateItemPossessionList();
                    b.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.ToyUpDic["ToyUp" + toyGround.number];
                    b.transform.Find("Shadow").GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.ToyUpDic["ToyUp" + toyGround.number];
                    b.gameObject.transform.position = new Vector3(toyGround.gameObject.transform.position.x, toyGround.gameObject.transform.position.y+12, 0);
                    b.gameObject.transform.DOMove(new Vector3(toyGround.gameObject.transform.position.x, toyGround.gameObject.transform.position.y + 32, 0), 0.3f).SetEase(Ease.OutBack);
                    shadow.transform.localScale = new Vector3(0f, 0f, 0f);
                    shadow.DOScale(new Vector3(0.8f,0.8f,0.8f), 0.3f).SetEase(Ease.OutBack);
                    toyGrounds.Remove(toyGround);
                    ObjectPoolManager.Instance._toyGroundPool.Return(toyGround);
                });
            }
        }
    }

    //シーン移動前に各オブジェクトをPoolに戻す
    public void OnDestroy()
    {
        int g = toyGrounds.Count;
        for (int i = 0; i < g; i++)
        {
            if (toyGrounds[0])
            {
                Debug.Log("aaaa");
                var d = toyGrounds[0];
                ObjectPoolManager.Instance._toyGroundPool.Return(d);
            }
            toyGrounds.RemoveAt(0);
        }

        g = toys.Count;
        for (int i = 0; i < g; i++)
        {
            if (toys[0])
            {
                var d = toys[0];
                ObjectPoolManager.Instance._toyPool.Return(d);
            }
            toys.RemoveAt(0);
        }
    }

}
