using System;

public class ExpManager
{
    public event Action<string, uint, uint> OnLevelUp;
    public event Action<string, uint, uint> OnExpChanged;

    private readonly ExpDataBase _dataBase;

    public ExpManager(ExpDataBase dataBase)
    {
        _dataBase = dataBase;
    }

    public void AddExp(string id, uint amount, uint maxLevel = uint.MaxValue)
    {
        ExpData data = _dataBase.GetExpData(id);
        uint oldLevel = data.Level;

        (uint nextLevel, uint remainingExp) = ExpTable.Exp(data.Level, data.Exp + amount, maxLevel);

        data.Level = nextLevel;
        data.Exp = remainingExp;

        if (nextLevel > oldLevel)
            OnLevelUp?.Invoke(id, oldLevel, nextLevel);

        OnExpChanged?.Invoke(id, data.Exp, ExpTable.RequiredExp(data.Level));
    }

    public bool Exists(string id) => _dataBase.Exists(id);

    public uint GetLevel(string id) => _dataBase.GetExpData(id).Level;

    public uint GetExp(string id) => _dataBase.GetExpData(id).Exp;
}
