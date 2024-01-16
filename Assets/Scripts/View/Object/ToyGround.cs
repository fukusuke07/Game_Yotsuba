using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ToyGround : MonoBehaviour,IClickable
{
    public ClickableObject clickableObject { get; set; } = ClickableObject.Toy;

    public int number;

    public IObservable<ToyGround> OnDig
    {
        get { return digSubject; }
    }
    private Subject<ToyGround> digSubject = new Subject<ToyGround>();

    public void OnClicked()
    {
        digSubject.OnNext(this);
    }
}
