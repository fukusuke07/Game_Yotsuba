using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BlackOut : SingletonMonoBehaviour<BlackOut>
{
    private Image image;

    public void Init()
    {
        image = GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0f);
    }

    //暗転
    public IEnumerator FadeOut()
    {
        image.color = new Color(0, 0, 0, 0.05f);

        for (int i = 0; i < 90; i++)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a*1.1f);     

            yield return null;
        }

        image.color = new Color(0, 0, 0, 1);
    }

    //暗転解除
    public IEnumerator FadeIn()
    {
        while(true)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a * 0.95f);

            yield return null;

            if (image.color.a <= 0.1f) break;
        }

        image.color = new Color(0, 0, 0, 0f);
    }
}
