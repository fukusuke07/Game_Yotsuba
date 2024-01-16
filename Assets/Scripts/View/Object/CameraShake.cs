using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;


public class CameraShake : SingletonMonoBehaviour<CameraShake>
{
    private float shake_X;

    private float shake_Y;

    private float shakeVertically;

    private Transform canvas;

    private Vector2 vector2 = new Vector2();

    private Vector3 newtralPosition;
    public void Init()
    {

        newtralPosition = transform.position;
    }
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    public void ShakeVertically(float duration, float magnitude)
    {
        StartCoroutine(DoShakeVertically(duration, magnitude));
    }

    public void ShakeCanvas(float duration, float magnitude)
    {
        StartCoroutine(DoShakeCanvas(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        var pos = newtralPosition;

        var elapsed = 0f;

        while (elapsed < duration)
        {
            shake_X = pos.x + Random.Range(-1f, 1f) * magnitude;
            shake_Y = pos.y + Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(shake_X, shake_Y, -10);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = pos;
    }
    private IEnumerator DoShakeVertically(float duration, float magnitude)
    {
        var pos = transform.localPosition;

        var elapsed = 0f;

        shakeVertically = 0;

        while (elapsed < duration)
        {

            shakeVertically = shakeVertically - Time.deltaTime * magnitude;

            elapsed += Time.deltaTime;

            yield return null;
        }

        elapsed = 0f;

        while (elapsed < duration)
        {

            shakeVertically = shakeVertically + Time.deltaTime * magnitude;

            elapsed += Time.deltaTime;

            yield return null;
        }

        shakeVertically = 0;
    }

    private IEnumerator DoShakeCanvas(float duration, float magnitude)
    {
        var pos = new Vector3(0, 0, 0);

        var elapsed = 0f;

        while (elapsed < duration)
        {
            var x = Random.Range(-1f, 1f) * magnitude;
            var y = Random.Range(-1f, 1f) * magnitude;

            vector2 = new Vector2(x, y);

            elapsed += Time.deltaTime;

            canvas.transform.localPosition = vector2;

            yield return null;
        }

        canvas.transform.localPosition = pos;
    }

    //Toyを掘り出す時の振動
    public async UniTask Dig()
    {
        int count = 0;
        await UniTask.DelayFrame(1);
        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        while (count < 1)
        {
            var sca = camera.transform.position;
            //sca.x += 1f;
            sca.y += 1f;
            camera.transform.position = sca;
            count += 1;
            await UniTask.DelayFrame(1);       // 1フレーム後、再開
        }
        count = 0;
        while (count < 1)
        {
            var sca = camera.transform.position;
            //sca.x -= 2f;
            sca.y -= 2f;
            camera.transform.position = sca;
            count += 1;
            await UniTask.DelayFrame(1);       // 1フレーム後、再開
        }
        count = 0;

        while (count < 1)
        {
            var sca = camera.transform.position;
            //sca.x += 2f;
            sca.y += 2f;
            camera.transform.position = sca;
            count += 1;
            await UniTask.DelayFrame(1);      // 1フレーム後、再開
        }
        count = 0;
        while (count < 1)
        {
            var sca = camera.transform.position;
            //sca.x -= 2f;
            sca.y -= 2f;
            camera.transform.position = sca;
            count += 1;
            await UniTask.DelayFrame(1);     // 1フレーム後、再開
        }
        count = 0;

        while (count < 1)
        {
            var sca = camera.transform.position;
            //sca.x += 2f;
            sca.y += 2f;
            camera.transform.position = sca;
            count += 1;
            await UniTask.DelayFrame(1);      // 1フレーム後、再開
        }
        count = 0;
        while (count < 1)
        {
            var sca = camera.transform.position;
            //sca.x -= 2f;
            sca.y -= 2f;
            camera.transform.position = sca;
            count += 1;
            await UniTask.DelayFrame(1);       // 1フレーム後、再開
        }
        count = 0;
        camera.transform.position = new Vector3(0, 0, -10);
    }
}
