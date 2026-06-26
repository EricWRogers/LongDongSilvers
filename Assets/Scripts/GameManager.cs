using UnityEngine;
using Unity.Netcode;  
public class GameManager : NetworkBehaviour
{
    //This whole thing may need to be a networkobj.

     public static GameManager Instance;

    public string JoinCode;

    public NetworkVariable<float> shiftTimer = new();

    public float maxShiftTime = 600f; //10 minutes. The huds know what to do with this. Should be 8pm is the end of shift.

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


    void Update()
    {
        if(!IsServer || !IsHost) return;

        shiftTimer.Value += Time.deltaTime;
    }
}
