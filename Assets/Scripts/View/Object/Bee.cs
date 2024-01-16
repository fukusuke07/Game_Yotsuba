using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Bee : MonoBehaviour,IClickable,IPopUppable,IScoreUppable
{
    int count = 1600;

    private bool isClicked = false;

    private SpriteRenderer sprite;

    public IObservable<Bee> OnDispose
    {
        get { return disposeSubject; }
    }
    private Subject<Bee> disposeSubject = new Subject<Bee>();

    public IObservable<Bee> OnClick
    {
        get { return clickSubject; }
    }
    private Subject<Bee> clickSubject = new Subject<Bee>();

    public IObservable<Bee> OnScoreUp
    {
        get { return scoreUpSubject; }
    }
    private Subject<Bee> scoreUpSubject = new Subject<Bee>();

    public ClickableObject clickableObject { get; set; } = ClickableObject.Bee;
    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position += new Vector3(-1*this.transform.localScale.x, 0, 0);

        count -= 1;

        if(count <= 0)
        {
            count = 1600;

            disposeSubject.OnNext(this);
        }
    }

    public void Init()
    {
        count = 1600;

        isClicked = false;

        if (UnityEngine.Random.Range(0, 2) > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        transform.position = new Vector3(360 * transform.localScale.x, UnityEngine.Random.Range(-70, 100), 0);

        if (!sprite) sprite = GetComponent<SpriteRenderer>();

        sprite.color = new Color(1f, 1f, 1f, 1f);
    }

    public void Turn() {
        var s = this.gameObject.transform.localScale;
        s.x *= -1;
        this.gameObject.transform.localScale = s;
    }

    public void TouchBee()
    {
        PopUp();
    }

    public void PopUp()
    {
        StartCoroutine(DoPopUp());
    }
    private IEnumerator DoPopUp()
    {
        int count = 0;

        while (count < 1)
        {
            var sca = this.transform.localScale;
            sca.x += count * 0.1f;
            sca.y += count * 0.1f;
            this.transform.localScale = sca;
            count += 1;
            yield return 0;        // 1フレーム後、再開
        }
        count = 0;
        while (count < 1)
        {
            var sca = this.transform.localScale;
            sca.x -= 0.1f;
            sca.y -= 0.1f;
            this.transform.localScale = sca;
            count += 1;
            yield return 0;        // 1フレーム後、再開
        }
        count = 0;

        while (count < 1)
        {
            var sca = this.transform.localScale;
            sca.x += count * 0.1f;
            sca.y += count * 0.1f;
            this.transform.localScale = sca;
            count += 1;
            yield return 0;        // 1フレーム後、再開
        }
        count = 0;
        while (count < 1)
        {
            var sca = this.transform.localScale;
            sca.x -= 0.1f;
            sca.y -= 0.1f;
            this.transform.localScale = sca;
            count += 1;
            yield return 0;        // 1フレーム後、再開
        }
        count = 0;

        while (count < 1)
        {
            var sca = this.transform.localScale;
            sca.x += count * 0.1f;
            sca.y += count * 0.1f;
            this.transform.localScale = sca;
            count += 1;
            yield return 0;        // 1フレーム後、再開
        }
        count = 0;
        while (count < 1)
        {
            var sca = this.transform.localScale;
            sca.x -= 0.1f;
            sca.y -= 0.1f;
            this.transform.localScale = sca;
            count += 1;
            yield return 0;        // 1フレーム後、再開
        }
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnClicked()
    {
        if (!isClicked)
        {
            isClicked = true;
            Turn();
            sprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
        clickSubject.OnNext(this);
    }

    public int GetScore()
    {
        return 100;
    }
}
