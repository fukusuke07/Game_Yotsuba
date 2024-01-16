using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    private SoundCallBack bgm;

    private AudioSource se = new AudioSource();

    private Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>();

    public void Init()
    {
        audioDic.Add("MItsubaGrowSE0", Resources.Load<AudioClip>("Audio/MItsubaGrowSE0"));
        audioDic.Add("MItsubaGrowSE1", Resources.Load<AudioClip>("Audio/MItsubaGrowSE1"));
        audioDic.Add("MItsubaGrowSE2", Resources.Load<AudioClip>("Audio/MItsubaGrowSE2"));
        audioDic.Add("Answer", Resources.Load<AudioClip>("Audio/Answer"));
        audioDic.Add("Wrong", Resources.Load<AudioClip>("Audio/Wrong"));
        audioDic.Add("Start", Resources.Load<AudioClip>("Audio/Start"));
        audioDic.Add("Finish", Resources.Load<AudioClip>("Audio/Finish"));
        audioDic.Add("Dig", Resources.Load<AudioClip>("Audio/Dig"));
        audioDic.Add("Get", Resources.Load<AudioClip>("Audio/Get"));
        audioDic.Add("BGM", Resources.Load<AudioClip>("Audio/MitsubaBGM0"));

        PlayBGM("BGM");

    }
    public void PlaySE(string _name)
    {
        var audio = ObjectPoolManager.Instance.CreateSoundCallBack();

        audio.OnCallBack
            .TakeUntilDisable(audio.gameObject)
            .Subscribe( _ => ObjectPoolManager.Instance._soundCallBackPool.Return(audio));

        var task = audio.PlaySE(audioDic[_name]);
    }

    public void PlaySEOnBigVolume(string _name)
    {
        var audio = ObjectPoolManager.Instance.CreateSoundCallBack();

        audio.OnCallBack
            .TakeUntilDisable(audio.gameObject)
            .Subscribe(_ => ObjectPoolManager.Instance._soundCallBackPool.Return(audio));

        var task = audio.PlaySE(audioDic[_name]);
    }

    public void PlayBGM(string _name)
    {
        if (!bgm) bgm = ObjectPoolManager.Instance.CreateSoundCallBack();

        bgm.GetComponent<AudioSource>().loop = true;

        bgm.PlayBGM(audioDic[_name]);
    }

    public void StopBGM()
    {
        bgm.StopBGM();
    }

}
