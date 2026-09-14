using UnityEngine;

public static class ExpTable
{
    const int REQUIRE_EXP_AMOUNT = 4;
    const float EXP_MULTIPLIER = 1.2f;

    /// <summary>
    /// 経験値処理
    /// </summary>
    /// <param name="currentLevel"></param>
    /// <param name="expGain"></param>
    /// <returns></returns>
    public static (uint level, uint exp) Exp(uint currentLevel, uint expGain, uint maxLevel = uint.MaxValue)
    {
        while (currentLevel < maxLevel)
        {
            uint required = GeometricSequence(currentLevel);
            if (expGain < required) break;

            expGain -= required;
            currentLevel++;
        }
        return (currentLevel, expGain);
    }

    /// <summary>
    /// 指定レベルから次のレベルの必要な経験値量
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static uint RequiredExp(uint level) => GeometricSequence(level);

    /// <summary>
    /// 等比数列的に次レベルまでの必要経験値を計算
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    private static uint GeometricSequence(uint level)
    {
        float result = REQUIRE_EXP_AMOUNT * Exponent(EXP_MULTIPLIER, level);
        return (uint)result;
    }

    /// <summary>
    /// 累乗計算用(fをp乗する)
    /// </summary>
    /// <param name="f"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    private static float Exponent(float f, uint p)
    {
        float result = 1;
        for (int i = 0; i < p; i++)
        {
            result *= f;
        }
        return result;
    }
}
