using System;
using System.Collections.Generic;

[Serializable]
public class ExpEntry
{
    public string Id;
    public ExpData Data;
}

[Serializable]
public class ExpDataBase
{
    //セーブデータ機能に使用
    public List<ExpEntry> Entries = new();

    [NonSerialized]
    private Dictionary<string, ExpData> _dict;

    /// <summary>
    /// ListからDictionaryを作成
    /// </summary>
    private void BuildDict()
    {
        _dict = new Dictionary<string, ExpData>();
        foreach(ExpEntry exp in Entries)
            _dict[exp.Id] = exp.Data;
    }

    /// <summary>
    /// 指定IDのデータを取得、なければ作成する
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ExpData GetExpData(string id)
    {
        if (_dict == null)
            BuildDict();

        if (!_dict.TryGetValue(id,out ExpData data))
        {
            data = new();
            _dict[id] = data;
            Entries.Add(new ExpEntry { Id = id, Data = data });
        }
        return data;
    }

    /// <summary>
    /// 指定IDのデータが存在するか確認
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool Exists(string id)
    {
        if (_dict == null)
            BuildDict();

        return _dict.ContainsKey(id);
    }
}
