using System;
using UnityEngine;

public class TimeManager
{
    public event Action OnTick;
    public event Action<double> OnOfflineTimeCalculated;

    private const string LAST_ACTIVE_TIME_KEY = "LastActiveTime";
    private float _tickInterval;
    private float _tickTimer;
    public TimeManager(float tickInterval)
    {
        _tickInterval = tickInterval;
    }

    public void Init()
    {
        CalculateOfflineTime();
    }

    public void Tick(float deltaTime)
    {
        _tickTimer += deltaTime;
        if (_tickTimer >= _tickInterval)
        {
            _tickTimer -= _tickInterval;
            OnTick?.Invoke();
        }
    }

    public void HandleApplicationPause(bool isPause)
    {
        if (isPause)
            SaveLastActiveTime();
        else
            CalculateOfflineTime();
    }

    public void HandleApplicationQuit()
    {
        SaveLastActiveTime();
    }

    private void SaveLastActiveTime()
    {
        PlayerPrefs.SetString(LAST_ACTIVE_TIME_KEY, DateTime.UtcNow.ToString("O"));
        PlayerPrefs.Save();
    }

    private void CalculateOfflineTime()
    {
        if (!PlayerPrefs.HasKey(LAST_ACTIVE_TIME_KEY)) return;

        string savedTimeStr = PlayerPrefs.GetString(LAST_ACTIVE_TIME_KEY);

        if (!DateTime.TryParse(savedTimeStr,null,System.Globalization.DateTimeStyles.RoundtripKind,out DateTime lastActiveTime))
        {
            Debug.LogWarning("保存された文字列から時刻への変換が失敗しました");
            return;
        }

        double elapsedSeconds = (DateTime.UtcNow - lastActiveTime).TotalSeconds;

        if (elapsedSeconds < 0)
        {
            Debug.LogWarning("経過時間がマイナス、時間設定が変更されている可能性があります");
            elapsedSeconds = 0;
        }

        OnOfflineTimeCalculated?.Invoke(elapsedSeconds);
    }
}
