using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private const float AUTO_SAVE_INTERVAL = 60f;
    public TimeManager TimeManager => _timeManager;
    private TimeManager _timeManager;
    public ExpManager ExpManager => _expManager;
    private ExpManager _expManager;

    public IdleRewardManager IdleRewardManager => _idleRewardManager;
    private IdleRewardManager _idleRewardManager;

    private SaveManager _saveManager;
    private SaveData _saveData;
    private float _autoSaveTimer;

    private ExpDataBase _expDataBase;

    protected override void Awake()
    {
        base.Awake();

        _saveManager = new SaveManager();
        _saveData = _saveManager.Load();

        _expManager = new ExpManager(_saveData.ExpDataBase);

        _timeManager = new TimeManager(tickInterval: 1f);

        _idleRewardManager = new IdleRewardManager(_timeManager, _expManager);

        _timeManager.Init();
    }

    private void Update()
    {
        _timeManager.Tick(Time.deltaTime);

        _autoSaveTimer += Time.deltaTime;
        if (_autoSaveTimer >= AUTO_SAVE_INTERVAL)
            Save();
    }

    /// <summary>
    /// セーブデータを保存
    /// </summary>
    public void Save()
    {
        _autoSaveTimer = 0;
        _saveManager.Save(_saveData);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            Save();

        _timeManager.HandleApplicationPause(pause);
    }

    private void OnApplicationQuit()
    {
        Save();
        _timeManager.HandleApplicationQuit();
    }
}
