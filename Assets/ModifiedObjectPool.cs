using System.Collections;
using System.Collections.Generic;
using UniRx.Toolkit;
using UnityEngine;

public class ModifiedObjectPool<T> : ObjectPool<T> where T : UnityEngine.Component
{
    private T _prefab;

    private readonly Transform _parenTransform;

    protected override T CreateInstance()
    {
        //新しく生成
        var e = GameObject.Instantiate(_prefab);

        //ヒエラルキーが散らからないように一箇所にまとめる
        e.transform.SetParent(_parenTransform);

        return e;
    }

    public ModifiedObjectPool(Transform parenTransform, T prefab)
    {
        _parenTransform = parenTransform;

        _prefab = prefab;
    }
}

