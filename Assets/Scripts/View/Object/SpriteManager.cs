using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager :SingletonMonoBehaviour<SpriteManager>
{
    public Dictionary<string, Sprite> numDic = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> miniNumDic = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> cloverSpriteDic = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> ToyDic = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> ToyUpDic = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> shirotsumeDic = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> beeDic = new Dictionary<string, Sprite>();
    public void Init()
    {
        numDic.Add("0", Resources.Load<Sprite>("Sprites/NumberSprite/0"));
        numDic.Add("1", Resources.Load<Sprite>("Sprites/NumberSprite/1"));
        numDic.Add("2", Resources.Load<Sprite>("Sprites/NumberSprite/2"));
        numDic.Add("3", Resources.Load<Sprite>("Sprites/NumberSprite/3"));
        numDic.Add("4", Resources.Load<Sprite>("Sprites/NumberSprite/4"));
        numDic.Add("5", Resources.Load<Sprite>("Sprites/NumberSprite/5"));
        numDic.Add("6", Resources.Load<Sprite>("Sprites/NumberSprite/6"));
        numDic.Add("7", Resources.Load<Sprite>("Sprites/NumberSprite/7"));
        numDic.Add("8", Resources.Load<Sprite>("Sprites/NumberSprite/8"));
        numDic.Add("9", Resources.Load<Sprite>("Sprites/NumberSprite/9"));

        cloverSpriteDic.Add("Fourclover0", Resources.Load<Sprite>("Sprites/CloverSprite/FourClover0"));
        cloverSpriteDic.Add("Fourclover1", Resources.Load<Sprite>("Sprites/CloverSprite/FourClover1"));
        cloverSpriteDic.Add("Fourclover2", Resources.Load<Sprite>("Sprites/CloverSprite/FourClover2"));
        cloverSpriteDic.Add("Fourclover3", Resources.Load<Sprite>("Sprites/CloverSprite/FourClover2"));
        cloverSpriteDic.Add("Threeclover0", Resources.Load<Sprite>("Sprites/CloverSprite/ThreeClover0"));
        cloverSpriteDic.Add("Threeclover1", Resources.Load<Sprite>("Sprites/CloverSprite/ThreeClover1"));
        cloverSpriteDic.Add("Threeclover2", Resources.Load<Sprite>("Sprites/CloverSprite/ThreeClover2"));
        cloverSpriteDic.Add("Threeclover3", Resources.Load<Sprite>("Sprites/CloverSprite/ThreeClover3"));
        cloverSpriteDic.Add("CloverStemL", Resources.Load<Sprite>("Sprites/CloverSprite/CloverStemL"));
        cloverSpriteDic.Add("CloverStem", Resources.Load<Sprite>("Sprites/CloverSprite/CloverStem"));
        cloverSpriteDic.Add("CloverStemR", Resources.Load<Sprite>("Sprites/CloverSprite/CloverStemR"));

        for (int i = 0; i < 16; i++)
        {
            ToyDic.Add("Toy" + i, Resources.Load<Sprite>("Sprites/ToySprite/Toy" + i));
        }
        for (int i = 0; i < 16; i++)
        {
            ToyUpDic.Add("ToyUp" + i, Resources.Load<Sprite>("Sprites/ToySprite/ToyUp" + i));
        }

        shirotsumeDic.Add("ShirotsumeWhite", Resources.Load<Sprite>("Sprites/CloverSprite/Shirotsume0"));
        shirotsumeDic.Add("ShirotsumeWhiteGrow", Resources.Load<Sprite>("Sprites/CloverSprite/Shirotsume0Grow"));
        shirotsumeDic.Add("ShirotsumePink", Resources.Load<Sprite>("Sprites/CloverSprite/Shirotsume1"));
        shirotsumeDic.Add("ShirotsumePinkGrow", Resources.Load<Sprite>("Sprites/CloverSprite/Shirotsume1Grow"));

        beeDic.Add("Bee0", Resources.Load<Sprite>("Sprites/BeeSprite/Bee0"));
        beeDic.Add("Bee1", Resources.Load<Sprite>("Sprites/BeeSprite/Bee1"));
    }
}
