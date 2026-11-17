using System;
using System.IO;
using UnityEngine;

public class SaveManager
{
    private const string FILE_NAME = "savedata.json";

    private readonly string _path;
    private readonly string _tempPath;
    private readonly string _backupPath;

    public SaveManager()
    {
        _path = Path.Combine(Application.persistentDataPath, FILE_NAME);
        _tempPath = _path + ".tmp";
        _backupPath = _path + ".bak";
    }

    /// <summary>
    /// セーブデータをロード。セーブデータがない、若しくは破損した場合は新規のデータを返す
    /// </summary>
    /// <returns></returns>
    public SaveData Load()
    {
        RecoverFromTemp();

        if (!File.Exists(_path))
            return new SaveData();

        try
        {
            string json = File.ReadAllText(_path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if (data == null)
                throw new Exception("Jsonの復元結果がnull");

            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager]セーブデータの読み込みに失敗 : {e.Message}");
            BackupBrokenFile();
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

        File.WriteAllText(_tempPath, json);

        if (File.Exists(_path))
            File.Replace(_tempPath, _path, null);
        else
            File.Move(_tempPath, _path);
    }

    /// <summary>
    /// 前回の保存が途中で中断された場合、.tmpからセーブデータを復元
    /// </summary>
    private void RecoverFromTemp()
    {
        if (!File.Exists(_tempPath)) return;

        //置き換えの途中でゲームが終了し、セーブデータが失われた場合
        if (!File.Exists(_path))
        {
            try
            {
                string json = File.ReadAllText(_tempPath);
                if (JsonUtility.FromJson<SaveData>(json) != null)
                {
                    File.Move(_tempPath, _path);
                    Debug.LogWarning("[SaveManager] .tmpからセーブデータを復元");
                    return;
                }
            }
            catch(Exception e)
            {
                Debug.LogError($"[SaveManager] .tmpからの復元に失敗 : {e.Message}");
            }
        }

        //セーブが保存されていて .tmpが不要な場合
        File.Delete(_tempPath);
    }

    private void BackupBrokenFile()
    {
        try
        {
            if (File.Exists(_backupPath)) 
                File.Delete(_backupPath);

            File.Move(_path, _backupPath);
            Debug.LogWarning($"[SaveManager]破損したファイルを移動 : {_backupPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager]破損ファイルの移動に失敗 : {e.Message}");
        }
    }
}
