using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError($"Singleton{typeof(T)}のインスタンスが存在していません、シーンの配置及び初期化順序を確認");

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning($"Singleton{typeof(T)}が重複したため{gameObject.name}を破棄");
            Destroy(gameObject);
        }
    }
}
