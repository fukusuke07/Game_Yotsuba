using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;

public class SoundCallBack : MonoBehaviour
{
    private AudioSource audio;

    public IObservable<SoundCallBack> OnCallBack
    {
        get { return callBackSubject; }
    }
    private Subject<SoundCallBack> callBackSubject = new Subject<SoundCallBack>();

    public async UniTask PlaySE(AudioClip audioClip)
    {
        if(!audio) audio = GetComponent<AudioSource>();
        audio.loop = false;
        audio.PlayOneShot(audioClip);

        await Checking();

        callBackSubject.OnNext(this);
    }

    IEnumerator Checking()
    {
        while (true)
        {
            yield return null;

            if (!audio.isPlaying)
            {

                break;
            }
        }
    }

    public void PlayBGM(AudioClip audioClip)
    {
        if (!audio) audio = GetComponent<AudioSource>();
        audio = GetComponent<AudioSource>();
        audio.loop = true;
        audio.clip = audioClip;
        audio.Play();
    }

    public void StopBGM()
    {
        audio.Stop();
    }
}
