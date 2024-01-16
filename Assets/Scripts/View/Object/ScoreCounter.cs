using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public float  count;
    public Dictionary<string, Image> dic = new Dictionary<string, Image>();
    private SpriteManager spriteManager;
    public GameObject number;
    public void Init() {

        spriteManager = GameObject.FindGameObjectWithTag("SpriteManager").GetComponent<SpriteManager>();
        number = Resources.Load<GameObject>("Prefabs/ScoreNumber");
        dic.Add("1", Instantiate(number).GetComponent<Image>());
        dic["1"].gameObject.transform.SetParent(this.gameObject.transform);
        Calculate(count);
        Order(count);
    }
    public void Calculate(float score) {
        bool next = true;
        float a = 1;
        float x = score;
        count = score;
        if (score == 0)
        {
            if (dic.ContainsKey(a.ToString()) == false)
            {
                dic.Add(a.ToString(), Instantiate(number).GetComponent<Image>());
                dic[a.ToString()].gameObject.transform.SetParent(this.gameObject.transform);
                Order(score);

            }

            dic[a.ToString()].sprite = spriteManager.numDic[(x % 10).ToString()];
            a *= 10;
            decimal g = (decimal)(x / 10);
            x = ((float)Math.Floor(g));
        }
        else
        {
            while (next == true)
            {

                if (score >= a)
                {
                    if (dic.ContainsKey(a.ToString()) == false)
                    {
                        dic.Add(a.ToString(), Instantiate(number).GetComponent<Image>());
                        dic[a.ToString()].gameObject.transform.SetParent(this.gameObject.transform);
                        Order(score);

                    }

                    dic[a.ToString()].sprite = spriteManager.numDic[(x % 10).ToString()];
                    a *= 10;
                    decimal g = (decimal)(x / 10);
                    x = ((float)Math.Floor(g));
                }
                else
                {
                    next = false;
                }
            }
        }

        CheckDigit();
        }
    public void Order(float score) {
        float a = 1;
        for(int i = 0; i < score.ToString().Length; i++) {
            if (dic.ContainsKey(a.ToString()) == false) {
                dic.Add(a.ToString(), Instantiate(number).GetComponent<Image>());
                dic[a.ToString()].gameObject.transform.SetParent(this.gameObject.transform);
            }
            dic[a.ToString()].gameObject.transform.localPosition = new Vector3(27 * (score.ToString().Length - i-1), 0, 0);
            a *= 10;
        }
    }

    public void CheckDigit()
    {
        int digit = 1;
        while (true)
        {
            if (dic.ContainsKey(digit.ToString()))
            {
                if(count < digit)
                {
                    if(digit==1&&count == 0)
                    {
                        var b = dic[digit.ToString()].color;

                        b.a =1;

                        dic[digit.ToString()].color = b;
                    }
                    else
                    {
                        var a = dic[digit.ToString()].color;

                        a.a = 0;

                        dic[digit.ToString()].color = a;
                    }
                    
                }
                else
                {
                    var a = dic[digit.ToString()].color;

                    a.a = 1;

                    dic[digit.ToString()].color = a;
                }
            }
            else
            {
                break;
            }
            digit *= 10;
        }
    }
}
public static class DictionaryExtensions {
    /// <summary>
    /// 値を取得、keyがなければデフォルト値を設定し、デフォルト値を取得
    /// </summary>
    public static TV GetOrDefault<TK, TV>(this Dictionary<TK, TV> dic, TK key, TV defaultValue = default(TV)) {
        TV result;
        return dic.TryGetValue(key, out result) ? result : defaultValue;
    }
}
