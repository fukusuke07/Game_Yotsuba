using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TimeModel
{
    public ReactiveProperty<int> time = new ReactiveProperty<int>(0);
    private static readonly int Init_Time = 50;

    private int miliSecond;
    private static readonly int Init_MiliSecond = 120;

    private SingleAssignmentDisposable timeCount = new SingleAssignmentDisposable();

    public TimeModel()
    {
        time.Value = Init_Time;

        miliSecond = Init_MiliSecond;
    }

    public void TimeCount()
    {
        if(time.Value > 0)
        {
            miliSecond -= 1;

            if (miliSecond < 0)
            {
                time.Value -= 1;

                miliSecond = Init_MiliSecond;
            }
        }
    }

    public void PlusTime(int plus)
    {
        time.Value += plus;

        if (time.Value < 0) time.Value = 0;
    }

    public void Start()
    {
        timeCount = new SingleAssignmentDisposable();

        timeCount.Disposable = Observable.EveryUpdate()
     .Subscribe(_ => {
         TimeCount();
     });
    }

    public void Stop()
    {
        timeCount.Dispose();
    }
}
