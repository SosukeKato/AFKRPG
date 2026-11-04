using System.IO;
using UnityEngine;

public class SaveManager
{
    private const string FILE_NAME = "savedata.json";

    private string _path;

    public SaveManager()
    {
        _path = Path.Combine(Application.persistentDataPath, FILE_NAME);
    }

    /// <summary>
    /// セーブデータをロード。セーブデータがない、若しくは破損した場合は新規のデータを返す
    /// </summary>
    /// <returns></returns>
    public SaveData Load()
    {
        if (!File.Exists(_path))
            return new SaveData();

        try
        {
            string json = File.ReadAllText(_path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data ?? new SaveData();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveManager]セーブデータの読み込みに失敗{e.Message}");
            return new SaveData();
        }
    }

    /// <summary>
    /// セーブデータを保存
    /// </summary>
    /// <param name="data"></param>
    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);

        string tempPath = _path + ".tmp";
        File.WriteAllText(tempPath, json);

        if (File.Exists(_path))
            File.Delete(_path);
        File.Move(tempPath, _path);
    }
}
