using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class GameController : SingletonMonoBehaviour<GameController>
{
    public ReactiveProperty<SceneState> state { get; private set; } = new ReactiveProperty<SceneState>(SceneState.Title);

    // Start is called before the first frame update

    override protected void Awake()
    {
        base.Awake();
        var poolManager = GameObject.FindGameObjectWithTag("ObjectPoolManager").GetComponent<ObjectPoolManager>();
        poolManager.Init();
    }

    void Start()
    {
        var audioManager = Instantiate(Resources.Load<GameObject>("Prefabs/AudioManager")).GetComponent<AudioManager>();
        audioManager.Init();

        var spriteManager = Instantiate(Resources.Load<GameObject>("Prefabs/SpriteManager")).GetComponent<SpriteManager>();
        spriteManager.Init();

        var blackOut = Instantiate(Resources.Load<GameObject>("Prefabs/BlackOut")).transform.Find("Panel").GetComponent<BlackOut>();
        blackOut.Init();

        var cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        cameraShake.Init();

        var viewController = Instantiate(Resources.Load<GameObject>("Prefabs/ViewController")).GetComponent<ViewController>();
        viewController.Init(this);

    }

public void ChangeState(SceneState state)
    {
        this.state.Value = state; 
    }

}

public enum SceneState
{
    Title,
    ToyCollection,
    Play
}
