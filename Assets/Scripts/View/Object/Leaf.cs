using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Leaf : MonoBehaviour, IClickable
{
    public bool isAnswer { get; private set; } = false;

    public ClickableObject clickableObject { get; set; }

    private SpriteRenderer stemSprite;
    public IObservable<Leaf> Touch
    {
        get { return touchSubject; }
    }
    private Subject<Leaf> touchSubject = new Subject<Leaf>();

    public IObservable<Leaf> TouchKeep
    {
        get { return touchKeepSubject; }
    }
    private Subject<Leaf> touchKeepSubject = new Subject<Leaf>();

    public IObservable<Leaf> Release
    {
        get { return releaseSubject; }
    }
    private Subject<Leaf> releaseSubject = new Subject<Leaf>();

    public IObservable<Leaf> Answer
    {
        get { return answerSubject; }
    }
    private Subject<Leaf> answerSubject = new Subject<Leaf>();

    public void TouchLeaf(Collider2D col)
    {
        Distance(col.transform.position);

        touchSubject.OnNext(this);
    }

    public void TouchKeepLeaf(Collider2D col)
    {
        Distance(col.transform.position);

        touchKeepSubject.OnNext(this);
    }

    public void ReleaseLeaf(Collider2D col)
    {
        releaseSubject.OnNext(this);
    }

    //葉っぱがマウスから離れる
    void Distance(Vector3 pos) {

        var a = Vector3.Distance(this.gameObject.transform.parent.position, pos);
        float b;
        var c = Vector3.Distance(this.gameObject.transform.localPosition, new Vector3(0, 0, 0));
        if (c > 3f)
        {
            b = 3f;
        }
        else
        {
            b = 6 - c;
        }
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.parent.position.x - 1.5f * (pos.x - this.gameObject.transform.parent.position.x) / a * b, this.gameObject.transform.parent.position.y - (pos.y - this.gameObject.transform.parent.position.y) / a * b, 0);


        if (transform.localPosition.x >= 2)
        {
            if (!stemSprite) stemSprite = transform.parent.Find("Stem").GetComponent<SpriteRenderer>();

            stemSprite.sprite = SpriteManager.Instance.cloverSpriteDic["CloverStemR"];
        }
        else if (transform.localPosition.x <= -2)
        {
            if (!stemSprite) stemSprite = transform.parent.Find("Stem").GetComponent<SpriteRenderer>();

            stemSprite.sprite = SpriteManager.Instance.cloverSpriteDic["CloverStemL"];
        }
        else
        {
            if (!stemSprite) stemSprite = transform.parent.Find("Stem").GetComponent<SpriteRenderer>();

            stemSprite.sprite = SpriteManager.Instance.cloverSpriteDic["CloverStem"];
        }

    }

    public void OnClicked()
    {

    }
}
