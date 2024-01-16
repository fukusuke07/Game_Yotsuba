using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ShirotsumeLeaf : MonoBehaviour,IClickable
{
    private Sprite open;

    private Sprite close;

    public ClickableObject clickableObject { get; set; } = ClickableObject.Shirotsume;

    public bool isGrow { get; private set; } = false;

    private Vector3 touchPosition;

    public IObservable<ShirotsumeLeaf> Touch
    {
        get { return touchSubject; }
    }
    private Subject<ShirotsumeLeaf> touchSubject = new Subject<ShirotsumeLeaf>();

    public IObservable<ShirotsumeLeaf> TouchKeep
    {
        get { return touchKeepSubject; }
    }
    private Subject<ShirotsumeLeaf> touchKeepSubject = new Subject<ShirotsumeLeaf>();

    public void TouchLeaf(Collider2D col)
    {

        var a = this.gameObject.transform.parent.GetComponent<IPopUppable>();

        a.PopUp();
    }

    public void TouchKeepLeaf(Collider2D col)
    {

    }

    void OnTriggerEnter2D(Collider2D col) {
        touchPosition = col.transform.position;

        touchSubject.OnNext(this);

    }
    void OnTriggerStay2D(Collider2D col) {
        touchPosition = col.transform.position;

        touchKeepSubject.OnNext(this);
    }
    void OnTriggerExit2D(Collider2D col) {
        //this.gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }

   IEnumerator Grow(int i) {

        for(int j =0;j< i; j++) {

            yield return null;

        }
        
        this.gameObject.GetComponent<SpriteRenderer>().sprite = open;

        isGrow = true;

        var a = this.gameObject.transform.parent.GetComponent<IPopUppable>();

        a.PopUp();
    }
    public void Open()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = open;

        isGrow = true;
    }

    public void Change() {
        var a = this.gameObject.transform.parent.GetComponent<IPopUppable>();

        a.PopUp();

        this.gameObject.GetComponent<SpriteRenderer>().sprite = close;

        StartCoroutine(Grow(40));
    }
    public IEnumerator ClearDeleat() {
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
        this.transform.localScale = new Vector3(1, 1, 1);
        Destroy(this.gameObject.transform.parent.gameObject);

    }

    public void GetPop() {
        var a = this.gameObject.transform.parent.GetComponent<IPopUppable>();
        a.PopUp();
    }

    public void SetSprite(Sprite _close,Sprite _open)
    {
        open = _open;

        close = _close;

        isGrow = false;

        transform.Find("NewSprite").GetComponent<SpriteRenderer>().sprite = close;

        this.gameObject.GetComponent<SpriteRenderer>().sprite = close;

    }

    public void OnClicked()
    {
        
    }
}
