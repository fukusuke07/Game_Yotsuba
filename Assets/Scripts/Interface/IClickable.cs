using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IClickable
{
    ClickableObject clickableObject { get; }
    void OnClicked();

}

public enum ClickableObject
{
    ThreeClover,
    FourClover,
    Shirotsume,
    Toy,
    Bee
}
