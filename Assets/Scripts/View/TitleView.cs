using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;

public class TitleView : MonoBehaviour
{
    //Object
    private List<Clover> threeClovers = new List<Clover>();
    private Clover fourClover;
    //Cloverにふれたフレームで音を鳴らす
    private bool touchLeaf = false;

    //Cloverを生やす
    public async UniTask GrowClover()
    {
        List<string> matrixList = new List<string>();

        //三つ葉のクローバーを生やす（３本）
        for (int i = 0; i < 3; i++)
        {
            var threeClover = CreateClover();
            var matrix = AsignMarix(matrixList);
            threeClover.transform.position = new Vector3(matrix.x * 20 + UnityEngine.Random.Range(-3, 3), matrix.y * 13 + UnityEngine.Random.Range(-3, 3), 0);
            threeClover.Init(CloverType.ThreeClover, 108 - 3 * matrix.y);
            AudioManager.Instance.PlaySE("MItsubaGrowSE" + UnityEngine.Random.Range(0, 3).ToString());
            threeClovers.Add(threeClover);
            await UniTask.DelayFrame(2);
        }

        //四つ葉のクローバーを生やす（１本）
        for (int i = 0; i < 1; i++)
        {
            fourClover = CreateClover();
            var matrix = AsignMarix(matrixList);
            fourClover.transform.position = new Vector3(matrix.x * 16 + UnityEngine.Random.Range(-3, 3), matrix.y * 13 + UnityEngine.Random.Range(-3, 3), 0);
            fourClover.Init(CloverType.FourClover, 108 - 3 * matrix.y);
            AudioManager.Instance.PlaySE("MItsubaGrowSE" + UnityEngine.Random.Range(0, 3).ToString());
            await UniTask.DelayFrame(2);
        }

        //三つ葉のクローバーを生やす（１６本）
        for (int i = 0; i < 16; i++)
        {
            var threeClover = CreateClover();
            var matrix = AsignMarix(matrixList);
            threeClover.transform.position = new Vector3(matrix.x * 20 + UnityEngine.Random.Range(-3, 3), matrix.y * 13 + UnityEngine.Random.Range(-3, 3), 0);
            threeClover.Init(CloverType.ThreeClover, 108 - 3 * matrix.y);
            AudioManager.Instance.PlaySE("MItsubaGrowSE" + UnityEngine.Random.Range(0, 3).ToString());
            threeClovers.Add(threeClover);
            await UniTask.DelayFrame(2);
        }

    }

    //クローバーを消す
    public IEnumerator DeleteClover()
    {
        //三つ葉のクローバーを消す
        int count = threeClovers.Count;
        for(int i= 0; i < count; i++)
        {
            var clover = threeClovers[0];
            clover.Return();
            threeClovers.RemoveAt(0);
            AudioManager.Instance.PlaySE("MItsubaGrowSE" + UnityEngine.Random.Range(0, 3).ToString());
            yield return null;
            yield return null;
            yield return null;
        }

        //待機
        yield return new WaitForSeconds(1.5f);

        //四つ葉のクローバーを消す
        fourClover.Return();
        fourClover = null;
        AudioManager.Instance.PlaySE("MItsubaGrowSE" + UnityEngine.Random.Range(0, 3).ToString());
    }

    private void Update()
    {
        //Cloverにふれたフレームで音を鳴らす
        if (touchLeaf == true)
        {
            AudioManager.Instance.PlaySE("MItsubaGrowSE" + UnityEngine.Random.Range(0, 3).ToString());
        }
        touchLeaf = false;
    }

    //Cloverを生成
    private Clover CreateClover()
    {
        //CloverをPoolからとってくる
        var clover = ObjectPoolManager.Instance.CreateClover();
        var leaf = clover.transform.Find("Leaf").GetComponent<Leaf>();

        //Subscribe（葉っぱに触れる）
        var OnTriggerEnterLeaf = leaf.OnTriggerEnter2DAsObservable()
        .Subscribe(col => { 
            leaf.TouchLeaf(col);
            touchLeaf = true;
        });
        var OnTriggerStayLeaf = leaf.OnTriggerStay2DAsObservable()
       .Subscribe(col => leaf.TouchKeepLeaf(col));
        var OnTriggerExitLeaf = leaf.OnTriggerExit2DAsObservable()
       .Subscribe(col => leaf.ReleaseLeaf(col));
        return clover;
    }

    //シーン移動前に各オブジェクトをPoolに戻す
    private void OnDestroy()
    {
        //三つ葉のクローバーを消す
        int g = threeClovers.Count;
        for (int i = 0; i < g; i++)
        {
            if (threeClovers[0] != null)
            {
                var d = threeClovers[0];
                ObjectPoolManager.Instance._cloverPool.Return(d);
            }
            threeClovers.RemoveAt(0);
        }
        //四つ葉のクローバーを消す
        var b = fourClover;
        if (b != null)
        {
            fourClover = null;
            ObjectPoolManager.Instance._cloverPool.Return(b);
        }
    }

    //Cloverを生やす位置を（重ならないように）指定
    private (int x, int y) AsignMarix(List<string> list)
    {
        (int x, int y) matrix = (UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-8, 8));

        var str = matrix.x.ToString() +":"+ matrix.y.ToString();

        if (list.Contains(str))
        {
            matrix = AsignMarix(list);
        }
        else
        {
            list.Add(str);
        }

        return matrix;
    }
}

