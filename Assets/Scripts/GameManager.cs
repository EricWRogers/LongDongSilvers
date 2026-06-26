using UnityEngine;

public class GameManager : MonoBehaviour
{
    //This whole thing may need to be a networkobj.

     public static GameManager Instance;

    public string JoinCode;

    public bool IsHost;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetJoinCode(string code)
    {
        JoinCode = code;
    }

    public void SetHost(bool value)
    {
        IsHost = value;
    }
}
