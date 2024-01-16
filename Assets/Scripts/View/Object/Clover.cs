using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Clover : MonoBehaviour,IPopUppable,IScoreUppable
{
    private Leaf leaf;

    public CloverType cloverType { get; private set; }

    public IObservable<Clover> OnScoreUp
    {
        get { return scoreUpSubject; }
    }
    private Subject<Clover> scoreUpSubject = new Subject<Clover>();

    private bool isPopping = false;

    private SpriteRenderer leafSprite;
    private SpriteRenderer shadowSprite;
    private SpriteRenderer stemSprite;

    private int order;

    public void Init(CloverType cloverType, int _order)
    {
        this.cloverType = cloverType;

        if (!leafSprite) leafSprite = transform.Find("Leaf").GetComponent<SpriteRenderer>();
        if (!shadowSprite) shadowSprite = transform.Find("Leaf").Find("Shadow").GetComponent<SpriteRenderer>();
        if (!stemSprite) stemSprite = transform.Find("Stem").GetComponent<SpriteRenderer>();

        if (!leaf)
        {
            leaf = transform.Find("Leaf").GetComponent<Leaf>();

            leaf.Touch.Subscribe(_ =>
            {
                PopUp();
                ChangeSortingOrder(10);
            });

            leaf.TouchKeep.Subscribe(_ =>
            {
                //if(!isPopping) this.transform.localScale = new Vector3(0.9f, 0.9f, 1);
            });

            leaf.Release.Subscribe(_ => 
            {
                ChangeSortingOrder(order);
                //this.transform.localScale = new Vector3(1f, 1f, 1);
            });
        }


        if (cloverType == CloverType.FourClover)
        {
            SetSprite(SpriteManager.Instance.cloverSpriteDic["Fourclover" + UnityEngine.Random.Range(0, 4)]);

            leaf.clickableObject = ClickableObject.FourClover;
        }
        else
        {
            SetSprite(SpriteManager.Instance.cloverSpriteDic["Threeclover" + UnityEngine.Random.Range(0, 3)]);

            leaf.clickableObject = ClickableObject.ThreeClover;
        }

        order = _order;
        ChangeSortingOrder(order);

        isPopping = false;

        PopUp();
    }
    private void SetSprite(Sprite _sprite)
    {
        leafSprite.sprite = _sprite;
        shadowSprite.sprite = _sprite;
    }

    public void ChangeSortingOrder(int _order)
    {
        leafSprite.sortingOrder = _order;
        shadowSprite.sortingOrder = _order-1;
        if (stemSprite.sortingOrder - 50 >= 0)
        {
            stemSprite.sortingOrder = _order - 50;
        }
    }
    public void PopUp()
    {
        if(!isPopping) StartCoroutine(DoPopUp());
    }
    private IEnumerator DoPopUp() {

        isPopping = true;

        int count = 0;

        while (count < 1) {
            var sca = this.transform.localScale;
            sca.x += count * 0.1f;
            sca.y += count * 0.1f;
            this.transform.localScale = sca;
            count += 1;
            yield return 0;        // 1フレーム後、再開
        }
        count = 0;
        while (count < 1) {
            var sca = this.transform.localScale;
            sca.x -= 0.1f;
            sca.y -= 0.1f;
            this.transform.localScale = sca;
            count += 1;
            yield return 0;        // 1フレーム後、再開
        }
        count = 0;

        while (count < 1) {
            var sca = this.transform.localScale;
            sca.x += count * 0.1f;
            sca.y += count * 0.1f;
            this.transform.localScale = sca;
            count += 1;
            yield return 0;        // 1フレーム後、再開
        }
        count = 0;
        while (count < 1) {
            var sca = this.transform.localScale;
            sca.x -= 0.1f;
            sca.y -= 0.1f;
            this.transform.localScale = sca;
            count += 1;
            yield return 0;        // 1フレーム後、再開
        }
        count = 0;

        while (count < 1) {
            var sca = this.transform.localScale;
            sca.x += count * 0.1f;
            sca.y += count * 0.1f;
            this.transform.localScale = sca;
            count += 1;
            yield return 0;        // 1フレーム後、再開
        }
        count = 0;
        while (count < 1) {
            var sca = this.transform.localScale;
            sca.x -= 0.1f;
            sca.y -= 0.1f;
            this.transform.localScale = sca;
            count += 1;
            yield return 0;        // 1フレーム後、再開
        }

        isPopping = false;

        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public int GetScore()
    {
        return 100;
    }

    private IEnumerator DoReturn()
    {
        yield return StartCoroutine(DoPopUp());

        ObjectPoolManager.Instance._cloverPool.Return(this);
    }

    public void Return()
    {
        StartCoroutine(DoReturn());
    }

}

public enum CloverType
{
    FourClover,
    ThreeClover
}
