using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Save、Load機能の実装
/// </summary>
public static class JsonDataManager
{
    /// <summary>
    /// パスを取得 & セーブファイル名記録
    /// </summary>
    private static string getFilePath() { return Application.persistentDataPath+ "/savedata.json"; }

    /// <summary>
    /// 書き込み機能
    /// </summary>
    /// <param name="paintDataWrapper">シリアライズするデータ</param>
    public static void Save(ItemPossessionList paintDataWrapper)
    {
        //シリアライズ実行
        string jsonSerializedData = JsonUtility.ToJson(paintDataWrapper);
        Debug.Log(jsonSerializedData);

        //実際にファイル作って書き込む
        using (var sw = new StreamWriter(getFilePath(), false))
        {
            try
            {
                //ファイルに書き込む
                sw.Write(jsonSerializedData);
            }
            catch (Exception e) //失敗した時の処理
            {
                Debug.Log(e);
            }
        }
    }

    /// <summary>
    /// 読み込み機能
    /// </summary>
    /// <returns>デシリアライズした構造体</returns>
    public static ItemPossessionList Load()
    {
        ItemPossessionList itemPossessionList = new ItemPossessionList();

        try
        {
            //ファイルを読み込む
            using (FileStream fs = new FileStream(getFilePath(), FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                string result = sr.ReadToEnd();
                Debug.Log(result);

                //読み込んだJsonを構造体にぶちこむ
                itemPossessionList = JsonUtility.FromJson<ItemPossessionList>(result);
            }
        }
        catch (Exception e) //失敗した時の処理
        {
            Debug.Log(e);
        }

        //デシリアライズした構造体を返す
        return itemPossessionList;
    }
}