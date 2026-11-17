using System;

public class IdleRewardManager
{
    private const double MAX_OFFLINE_SECONDS = 12 * 60 * 60;
    private const double MIN_OFFLINE_SECONDS = 5 * 60;
    private const uint EXP_PER_SECOND = 10;

    /// <summary>
    /// オフライン報酬付与時に発火、(反映秒数、獲得経験値量)
    /// </summary>
    public event Action<double, uint> OnOfflineRewardGranted;

    public double PendingOfflineSeconds { get; private set; }
    public uint PendingOfflineExp { get; private set; }

    private readonly ExpManager _expManager;

    public IdleRewardManager(TimeManager timeManager, ExpManager expManager)
    {
        _expManager = expManager;

        timeManager.OnTick += HandleTick;
        timeManager.OnOfflineTimeCalculated += HandleOfflineTime;
    }

    /// <summary>
    /// 起動時のUI表示のために取り出す
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="exp"></param>
    /// <returns></returns>
    public bool TryConsumePendingReward(out double seconds, out uint exp)
    {
        seconds = PendingOfflineSeconds;
        exp = PendingOfflineExp;

        bool hasReward = exp > 0;
        PendingOfflineSeconds = 0;
        PendingOfflineExp = 0;
        return hasReward;
    }

    /// <summary>
    /// オンライン中指定秒数毎に経験値を加算
    /// </summary>
    private void HandleTick()
    {
        _expManager.AddExp(ExpIds.Player, EXP_PER_SECOND);
    }


    private void HandleOfflineTime(double elapsedSeconds)
    {
        if (elapsedSeconds < MIN_OFFLINE_SECONDS)
            return;

        double cappedSeconds = Math.Min(elapsedSeconds, MAX_OFFLINE_SECONDS);
        uint exp = (uint)(cappedSeconds * EXP_PER_SECOND);

        _expManager.AddExp(ExpIds.Player, exp);

        PendingOfflineSeconds += cappedSeconds;
        PendingOfflineExp += exp;
        OnOfflineRewardGranted?.Invoke(cappedSeconds, exp);
    }
}
