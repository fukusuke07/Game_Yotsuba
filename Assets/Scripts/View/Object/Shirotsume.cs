using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Shirotsume : MonoBehaviour,IPopUppable,IScoreUppable
{
    private ShirotsumeLeaf leaf;

    public IObservable<Clover> OnScoreUp
    {
        get { return scoreUpSubject; }
    }
    private Subject<Clover> scoreUpSubject = new Subject<Clover>();

    private bool isPopping = false;

    private SpriteRenderer leafSprite;
    private SpriteRenderer shadowSprite;

    private SingleAssignmentDisposable bloomSub;

    private int order;

    private ShirotsumeColor color = ShirotsumeColor.White; 
    // Start is called before the first frame update

    public void Init(int _order)
    {
        if (!leafSprite) leafSprite = transform.Find("Leaf").GetComponent<SpriteRenderer>();
        if (!shadowSprite) shadowSprite = transform.Find("Leaf").Find("Shadow").GetComponent<SpriteRenderer>();

        if (!leaf)
        {
            leaf = transform.Find("Leaf").GetComponent<ShirotsumeLeaf>();

            leaf.Touch.Subscribe(_ =>
            {
                PopUp();

                Bloom();
            });
        }

        color = (ShirotsumeColor)UnityEngine.Random.Range(0, 2);

        Debug.Log("Shirotsume" + color.ToString());

        SetSprite(SpriteManager.Instance.shirotsumeDic["Shirotsume"+ color.ToString()]);

        order = _order;
        ChangeSortingOrder(order);

        isPopping = false;

        PopUp();

        Bloom();
    }

    public void ChangeSortingOrder(int _order)
    {
        leafSprite.sortingOrder = _order;
        shadowSprite.sortingOrder = _order - 1;
    }

    public void Bloom()
    {
        SetSprite(SpriteManager.Instance.shirotsumeDic["Shirotsume" + color.ToString()]);
        Debug.Log("bloom");
        if (bloomSub != null)
        {
            bloomSub.Dispose();
            bloomSub = null;
        }
        bloomSub = new SingleAssignmentDisposable();
        bloomSub.Disposable = Observable.Timer(TimeSpan.FromMilliseconds(UnityEngine.Random.Range(1000, 3000))).Subscribe(_ =>
        {
            SetSprite(SpriteManager.Instance.shirotsumeDic["Shirotsume" + color.ToString() + "Grow"]);
            AudioManager.Instance.PlaySEOnBigVolume("MItsubaGrowSE" + UnityEngine.Random.Range(0, 3));
            PopUp();
            bloomSub.Dispose();
            bloomSub = null;
        });
    }

    private void SetSprite(Sprite _sprite)
    {
        leafSprite.sprite = _sprite;
        shadowSprite.sprite = _sprite;
    }

    public void PopUp()
    {
        StartCoroutine(DoPopUp());
    }

    public IEnumerator DoPopUp() {

        isPopping = true;

        Vector3 moveDistance;            // 移動距離および方向

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

    public IEnumerator DoReturn()
    {
        yield return StartCoroutine(DoPopUp());

        ObjectPoolManager.Instance._shirotsumePool.Return(this);
    }

    public void Return()
    {
        if (bloomSub != null)
        {
            bloomSub.Dispose();
            bloomSub = null;
        }
        StartCoroutine(DoReturn());
    }

    public int GetScore()
    {
        return 100;
    }
}

public enum ShirotsumeColor
{
    White,
    Pink
}
