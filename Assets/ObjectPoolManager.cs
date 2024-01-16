using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;

public class ObjectPoolManager : SingletonMonoBehaviour<ObjectPoolManager>
{
    [SerializeField]
    private Clover _cloverPrefab;

    [SerializeField]
    private Shirotsume _shirotsumePrefab;

    [SerializeField]
    private ToyGround _toyGroundPrefab;

    [SerializeField]
    private Toy _toyPrefab;

    [SerializeField]
    private Bee _beePrefab;

    [SerializeField]
    private PlusScore _plusScorePrefab;

    [SerializeField]
    private SoundCallBack _soundCallBackPrefab;

    [SerializeField]
    private Transform _hierarchyTransform; //追加

    public ModifiedObjectPool<Clover> _cloverPool; //追加

    public ModifiedObjectPool<ToyGround> _toyGroundPool; //追加

    public ModifiedObjectPool<Toy> _toyPool; //追加

    public ModifiedObjectPool<Shirotsume> _shirotsumePool; //追加

    public ModifiedObjectPool<Bee> _beePool; //追加

    public ModifiedObjectPool<PlusScore> _plusScorePool; //追加

    public ModifiedObjectPool<SoundCallBack> _soundCallBackPool; //追加

    public int count;

    public ScoreCounter score;

    public void Init()
    {
        //オブジェクトプールを生成
        _cloverPool = new ModifiedObjectPool<Clover>(_hierarchyTransform, _cloverPrefab);

        _toyGroundPool = new ModifiedObjectPool<ToyGround>(_hierarchyTransform, _toyGroundPrefab);

        _toyPool = new ModifiedObjectPool<Toy>(_hierarchyTransform, _toyPrefab);

        _shirotsumePool = new ModifiedObjectPool<Shirotsume>(_hierarchyTransform, _shirotsumePrefab);

        _beePool = new ModifiedObjectPool<Bee>(_hierarchyTransform, _beePrefab);

        _plusScorePool = new ModifiedObjectPool<PlusScore>(_hierarchyTransform, _plusScorePrefab);

        _soundCallBackPool = new ModifiedObjectPool<SoundCallBack>(_hierarchyTransform, _soundCallBackPrefab);

        //破棄されたときにPoolを解放する
        this.OnDestroyAsObservable().Subscribe(_ => DisposeAllPool());

    }

    public void DisposeAllPool()
    {
        _cloverPool.Dispose();

        _toyGroundPool.Dispose();

        _toyPool.Dispose();

        _beePool.Dispose();

        _plusScorePool.Dispose();
    }

    public Clover CreateClover()
    {
        var exp = _cloverPool.Rent();

        return exp;
    }

    public ToyGround CreateToyGround()
    {
        var exp = _toyGroundPool.Rent();

        return exp;
    }

    public Toy CreateToy()
    {
        var exp = _toyPool.Rent();

        return exp;
    }

    public Shirotsume CreateShirotsume()
    {
        var exp = _shirotsumePool.Rent();

        return exp;
    }

    public Bee CreateBee()
    {
        var exp = _beePool.Rent();

        return exp;
    }

    public PlusScore CreatePlusScore()
    {
        var exp = _plusScorePool.Rent();

        return exp;
    }

    public SoundCallBack CreateSoundCallBack()
    {
        var exp = _soundCallBackPool.Rent();

        return exp;
    }

}
