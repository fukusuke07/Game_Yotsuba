using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using DG.Tweening;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

public class PlayView : MonoBehaviour
{
    //難易度
    private int level;
    //フレーム内で葉っぱに触っているか
    private bool touchLeaf = false;
    //Object
    private List<Clover> threeClovers = new List<Clover>();
    private Clover fourClover;
    private List<Shirotsume> shirotsumes = new List<Shirotsume>();
    private ToyGround toyGround;
    private List<Bee> bees = new List<Bee>();

    public IObservable<IScoreUppable> OnScoreUp
    {
        get { return scoreUpSubject; }
    }
    private Subject<IScoreUppable> scoreUpSubject = new Subject<IScoreUppable>();

    public IObservable<Unit> OnClear
    {
        get { return clearSubject; }
    }
    private Subject<Unit> clearSubject = new Subject<Unit>();

    // Start is called before the first frame update
    public void Init()
    {
        level = 0;
    }

    //Objectを配置
    public async UniTask Standby()
    {
        level += 1;

        await GrowClover();

        if (level >= 3) await GrowShirotsume();

        if (level >= 2) BuryToy();

    }
    void Update()
    {
        if (touchLeaf == true)
        {
            AudioManager.Instance.PlaySE("MItsubaGrowSE" + UnityEngine.Random.Range(0, 3).ToString());
        }
        touchLeaf = false;
    }

    //シーン移動前に各オブジェクトをPoolに戻す
    public void OnDestroy() {
        CleanUp();
    }

    //Cloverを生やす
    private async UniTask GrowClover()
    {
        if (level >= 4) {
            level = 4;
        }

        //配置するクローバーの総数
        var numberBox = new NumberBox(level * level * 9);
        //四葉のクローバーの位置を指定
        int fourCloverNum = UnityEngine.Random.Range(0, level * level * 9);

        while (numberBox.numbers.Count > 0) {

            int num = numberBox.ExtractNumber();

            int x = num % (level * 3);

            int y = num / (level * 3);

            if (num == fourCloverNum)
            {
                fourClover = CreateClover();
                fourClover.OnScoreUp.Subscribe(_ => scoreUpSubject.OnNext(fourClover));
                fourClover.PopUp();
                decimal g = level * 3 / 2;
                fourClover.transform.position = new Vector3((y - (float)Math.Floor(g)) * (20 + (4 - level)) + UnityEngine.Random.Range(-3, 3), (x - (float)Math.Floor(g)) * (17 + (4 - level)) - 5 + (level) + UnityEngine.Random.Range(-3, 3), 0);
                fourClover.Init(CloverType.FourClover, 108 - 3 * x);

                Debug.Log(fourClover.transform.localPosition);
            }
            else
            {
                Debug.Log(threeClovers.Count);
                var clover = CreateClover();
                clover.OnScoreUp.Subscribe(_ => scoreUpSubject.OnNext(clover));
                clover.PopUp();
                threeClovers.Add(clover);
                decimal g = level * 3 / 2;
                clover.transform.position = new Vector3((y - (float)Math.Floor(g)) * (20 + (4 - level)) + UnityEngine.Random.Range(-3, 3), (x - (float)Math.Floor(g)) * (17 + (4 - level)) - 5 + (level) + UnityEngine.Random.Range(-3, 3), 0);
                clover.Init(CloverType.ThreeClover, 108 - 3 * x);

                Debug.Log(clover.transform.localPosition);
            }

            AudioManager.Instance.PlaySE("MItsubaGrowSE" + UnityEngine.Random.Range(0, 3).ToString());

            await UniTask.DelayFrame(1);
            await UniTask.DelayFrame(1);
        }
    }

    //Shirotsumeを生やす
    public async UniTask GrowShirotsume() 
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        int total = level * level - 5;

        while (total > 0) {

            for (int y = 1; y < level * 3 - 1; y++) {

                int num = UnityEngine.Random.Range(1, level * 3);
                var shirotsume = CreateShirotsume();
                shirotsume.Init(108 - 3 * y + 6);
                shirotsume.OnScoreUp.Subscribe(_ => scoreUpSubject.OnNext(shirotsume));
                touchLeaf = true;
                shirotsumes.Add(shirotsume);
                total -= 1;
                decimal g = level * 3 / 2;
                shirotsume.transform.position = new Vector3((num - (float)Math.Floor(g)) * 21 - 10, (y - (float)Math.Floor(g)) * 21 - 13 + (level) + UnityEngine.Random.Range(0, 1), 0);
            }

            await UniTask.DelayFrame(1);
        }

    }

    //Toyを埋める
    public void BuryToy() {
        int numX = UnityEngine.Random.Range(1, level * 3 - 2);
        int numY = UnityEngine.Random.Range(1, level * 3 - 2);
        var toy = ObjectPoolManager.Instance.CreateToyGround().gameObject;
        decimal g = level * 3 / 2;
        toyGround = toy.GetComponent<ToyGround>();
        if(level <= 2)
        {
            toyGround.number = UnityEngine.Random.Range(0, 8);
        }
        else
        {
            toyGround.number = UnityEngine.Random.Range(0, 16);
        }
        toy.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.ToyDic["Toy" + toyGround.number];
        toyGround.OnDig.TakeUntilDisable(toyGround).Subscribe(_toyG =>
        {
            AudioManager.Instance.PlaySE("Get");
            AudioManager.Instance.PlaySEOnBigVolume("Dig");
            var b = ObjectPoolManager.Instance.CreateToy();
            b.number = _toyG.number;
            b.UpdateItemPossessionList();
            b.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.ToyUpDic["ToyUp" + _toyG.number];
            b.gameObject.transform.position = new Vector3(_toyG.gameObject.transform.position.x, _toyG.gameObject.transform.position.y+20, 0);
            b.gameObject.transform.DOMove(new Vector3(_toyG.gameObject.transform.position.x, _toyG.gameObject.transform.position.y + 40, 0), 1f).SetEase(Ease.OutBack).OnComplete(() => ObjectPoolManager.Instance._toyPool.Return(b));
            toyGround = null;
            ObjectPoolManager.Instance._toyGroundPool.Return(_toyG);
        });
        
        toy.transform.position = new Vector3((numX - (float)Math.Floor(g)) * 21 - 5 + UnityEngine.Random.Range(0, 3), (numY - (float)Math.Floor(g)) * 21 - 13 + level + UnityEngine.Random.Range(0, 1), 0);
    }

    //ミツバチを配置
    public void SetBee() {

        var bee = ObjectPoolManager.Instance.CreateBee();
        bee.OnDispose.TakeUntilDisable(bee).Subscribe(_ => ObjectPoolManager.Instance._beePool.Return(bee));
        bee.Init();
        var OnTriggerEnterLeaf = bee.OnTriggerEnter2DAsObservable().Subscribe(col => {
            bee.TouchBee();
         });
        bee.OnClick.TakeUntilDisable(bee).Subscribe(_ =>
        {
            AudioManager.Instance.PlaySE("Get");
            scoreUpSubject.OnNext(bee);
            var _plusScore = ObjectPoolManager.Instance.CreatePlusScore();
            _plusScore.transform.position = new Vector3(bee.transform.position.x, bee.transform.position.y + 20, 0);
            _plusScore.transform.DOMove(new Vector3(_plusScore.transform.position.x, _plusScore.transform.position.y + 20, 0), 0.5f).SetEase(Ease.OutBack).OnComplete(() => ObjectPoolManager.Instance._plusScorePool.Return(_plusScore));
        });
        bees.Add(bee.GetComponent<Bee>());

    }


    public async UniTask Answer() 
    {

        AudioManager.Instance.PlaySE("Answer");

        //シロツメクサを消す
        int g = shirotsumes.Count;
        for (int i = 0; i < g; i++) {

            var shirotsume = shirotsumes[0];

            //スコア加算
            scoreUpSubject.OnNext(shirotsume);
            touchLeaf = true;
            var plusScore = ObjectPoolManager.Instance.CreatePlusScore();
            plusScore.transform.position = new Vector3(shirotsume.transform.position.x, shirotsume.transform.position.y + 20, 0);
            plusScore.transform.DOMove(new Vector3(plusScore.transform.position.x, plusScore.transform.position.y + 20, 0), 0.5f).SetEase(Ease.OutBack).OnComplete(() => ObjectPoolManager.Instance._plusScorePool.Return(plusScore));

            shirotsume.Return();
            shirotsumes.RemoveAt(0);

            await UniTask.DelayFrame(1);
            await UniTask.DelayFrame(1);

        }

        //三つ葉のクローバーを消す
        g = threeClovers.Count;
        for (int i = 0; i < g; i++) {
            
            var threeClover = threeClovers[0];

            //スコア加算
            scoreUpSubject.OnNext(threeClovers[0]);
            AudioManager.Instance.PlaySEOnBigVolume("MItsubaGrowSE" + UnityEngine.Random.Range(0, 3));
            var _plusScore = ObjectPoolManager.Instance.CreatePlusScore();
            _plusScore.transform.position = new Vector3(threeClover.transform.position.x, threeClover.transform.position.y + 20, 0);
            _plusScore.transform.DOMove(new Vector3(_plusScore.transform.position.x, _plusScore.transform.position.y + 20, 0), 0.5f).SetEase(Ease.OutBack).OnComplete(() => ObjectPoolManager.Instance._plusScorePool.Return(_plusScore));
            threeClover.Return();
            threeClovers.RemoveAt(0);

            await UniTask.DelayFrame(1);
            await UniTask.DelayFrame(1);
        }

        await UniTask.Delay(TimeSpan.FromSeconds(1.5f));

        //スコア加算
        scoreUpSubject.OnNext(fourClover);
        AudioManager.Instance.PlaySEOnBigVolume("MItsubaGrowSE" + UnityEngine.Random.Range(0, 1));
        var plus = ObjectPoolManager.Instance.CreatePlusScore();
        plus.transform.position = new Vector3(fourClover.transform.position.x, fourClover.transform.position.y + 20, 0);
        plus.transform.DOMove(new Vector3(plus.transform.position.x, plus.transform.position.y + 20, 0), 0.5f).SetEase(Ease.OutBack).OnComplete(() => ObjectPoolManager.Instance._plusScorePool.Return(plus));

        //四つ葉のクローバーを消す
        fourClover.Return();
        fourClover = null;

        await UniTask.Delay(TimeSpan.FromSeconds(1.5f));

    }

    //Shirotsumeを生成
    private Shirotsume CreateShirotsume()
    {
        var shirotsume = ObjectPoolManager.Instance.CreateShirotsume();

        var leaf = shirotsume.transform.Find("Leaf").GetComponent<ShirotsumeLeaf>();

        var OnTriggerEnterLeaf = leaf.OnTriggerEnter2DAsObservable()
            .TakeUntilDisable(shirotsume)
            .Subscribe(col => leaf.TouchLeaf(col));

        var OnTriggerStayLeaf = leaf.OnTriggerStay2DAsObservable()
            .TakeUntilDisable(shirotsume)
            .Subscribe(col => leaf.TouchKeepLeaf(col));

        return shirotsume;
    }

    //Cloverを生成
    private Clover CreateClover()
    {
        var clover = ObjectPoolManager.Instance.CreateClover();

        var leaf = clover.transform.Find("Leaf").GetComponent<Leaf>();

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

    //暗転時にオブジェクトをPoolに戻す
    public void CleanUp()
    {
        //三つ葉のクローバーを消す
        int g = threeClovers.Count;
        for (int i = 0; i < g; i++)
        {
            if (threeClovers[0]!=null)
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
        //シロツメクサを消す
        g = shirotsumes.Count;
        for (int i = 0; i < g; i++)
        {
            if (shirotsumes[0] != null)
            {
                var d = shirotsumes[0];
                shirotsumes.RemoveAt(0);
                ObjectPoolManager.Instance._shirotsumePool.Return(d);
            }
        }
        //ハチを消す
        g = bees.Count;
        for (int i = 0; i < g; i++)
        {
            if (bees[0])
            {
                var d = bees[0];
                ObjectPoolManager.Instance._beePool.Return(d);
            }
            bees.RemoveAt(0);
        }
        //おもちゃを消す
        if (toyGround != null)
        {
            ObjectPoolManager.Instance._toyGroundPool.Return(toyGround);
            toyGround = null;
        }
    }

}

//Cloverの配置場所の抽選
public class NumberBox
{
    public List<int> numbers;

    public NumberBox(int num)
    {
        numbers = new List<int>();

        for(int i = 0; i < num; i++)
        {
            numbers.Add(i);
         }
    }

    public int ExtractNumber()
    {
        int order = UnityEngine.Random.Range(0, numbers.Count);

        int num = numbers[order];

        numbers.RemoveAt(order);

        return num;
    }
}


