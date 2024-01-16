using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    // 位置座標
    private Vector3 position;

    private List<IClickable> objects = new List<IClickable>();

    // スクリーン座標をワールド座標に変換した位置座標
    private Vector3 screenToWorldPointPosition;

    private Camera camera;

    public bool active;
    public IObservable<List<IClickable>> OnClick
    {
        get { return clickSubject; }
    }
    private Subject<List<IClickable>> clickSubject = new Subject<List<IClickable>>();

    public void Init() {

        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

    }

    void Update() {

        // Vector3でマウス位置座標を取得する

        position = Input.mousePosition;

        // Z軸修正

        position.z = 10f;

        // マウス位置座標をスクリーン座標からワールド座標に変換する

        screenToWorldPointPosition = camera.ScreenToWorldPoint(position);

        // ワールド座標に変換されたマウス座標を代入

        gameObject.transform.position = new Vector3 (screenToWorldPointPosition.x, screenToWorldPointPosition.y,0);
        if (Input.GetMouseButtonDown(0)) {
            clickSubject.OnNext(objects);
        }

    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.GetComponent<IClickable>()!=null)
        {
            if (!objects.Contains(col.gameObject.GetComponent<IClickable>()))
            {
                objects.Add(col.gameObject.GetComponent<IClickable>());
            }
        }
        
    }
    void OnTriggerStay2D(Collider2D col) {
        
    }
    void OnTriggerExit2D(Collider2D col) {
        if (objects.Contains(col.gameObject.GetComponent<IClickable>()))
        {
            objects.Remove(col.gameObject.GetComponent<IClickable>());
        }
    }

    public void ChangeActive(bool _active)
    {
        active = _active;
    }

}
