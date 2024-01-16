using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : MonoBehaviour
{
    //番号
    public int number;
    // Start is called before the first frame update

    //発見したToyを記録
    public void UpdateItemPossessionList()
    {
        ItemPossessionList itemPossessionList = JsonDataManager.Load();

        itemPossessionList.itemPossessonList[number] = true;

        JsonDataManager.Save(itemPossessionList);
    }
}
