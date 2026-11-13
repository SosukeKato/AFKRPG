using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private const float AUTO_SAVE_INTERVAL = 60f;
    public TimeManager TimeManager => _timeManager;
    private TimeManager _timeManager;
    public ExpManager ExpManager => _expManager;
    private ExpManager _expManager;

    private SaveManager _saveManager;
    private SaveData _saveData;
    private float _autoSaveTimer;

    private ExpDataBase _expDataBase;

    protected override void Awake()
    {
        base.Awake();

        _saveManager = new SaveManager();
        _saveData = _saveManager.Load();

        _timeManager = new(tickInterval: 1f);
        _timeManager.Init();

        _expManager = new ExpManager(_saveData.ExpDataBase);
    }

    private void Update()
    {
        _timeManager.Tick(Time.deltaTime);

        _autoSaveTimer += Time.deltaTime;
        if(_autoSaveTimer >= AUTO_SAVE_INTERVAL)
        {
            _autoSaveTimer = 0;
            _saveManager.Save(_saveData);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            _saveManager.Save(_saveData);

        _timeManager.HandleApplicationPause(pause);
    }

    private void OnApplicationQuit()
    {
        _saveManager.Save(_saveData);
        _timeManager.HandleApplicationQuit();
    }
}
