using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public TimeManager TimeManager => _timeManager;
    private TimeManager _timeManager;

    protected override void Awake()
    {
        base.Awake();

        _timeManager = new(tickInterval: 1f);
        _timeManager.Init();
    }

    private void Update()
    {
        _timeManager.Tick(Time.deltaTime);
    }

    private void OnApplicationPause(bool pause)
    {
        _timeManager.HandleApplicationPause(pause);
    }

    private void OnApplicationQuit()
    {
        _timeManager.HandleApplicationQuit();
    }
}
