using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class NetworkGameManager : MonoBehaviour
{
    public static NetworkGameManager Instance;          // COMMUNICATION OF CLIENT AND SERVER
    private const string MESSAGE = "JSON";

    void Awake()                                        
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()                                // RECEIVES MSF FROM CLIENT
    {
        NetworkManager.Singleton.CustomMessagingManager
            .RegisterNamedMessageHandler(MESSAGE, OnReceive);
    }

    public void SendToServer(string json)          
    {
        var writer =
            new FastBufferWriter(1024, Allocator.Temp);

        writer.WriteValueSafe(json);

        NetworkManager.Singleton.CustomMessagingManager
            .SendNamedMessage(MESSAGE,
                              NetworkManager.ServerClientId,
                              writer);
    }

    void OnReceive(ulong sender,
                   FastBufferReader reader)
    {
        reader.ReadValueSafe(out string json);

        BaseMessage msg =
            JsonUtility.FromJson<BaseMessage>(json);

        if (msg.action == "endTurn")
            GameManager.Instance.EndTurn(sender);
    }
}
