using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class NetworkGameManager : MonoBehaviour
{
    public static NetworkGameManager Instance;
    private const string MESSAGE = "JSON";

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        NetworkManager.Singleton.CustomMessagingManager
            .RegisterNamedMessageHandler(
                MESSAGE, OnReceive);
    }

    public void SendToServer(string json)
    {
        var writer =
            new FastBufferWriter(1024,
                                 Allocator.Temp);

        writer.WriteValueSafe(json);

        NetworkManager.Singleton.CustomMessagingManager
            .SendNamedMessage(
                MESSAGE,
                NetworkManager.ServerClientId,
                writer);
    }

    public void Broadcast(string json)
    {
        foreach (ulong id in
                 NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (id == NetworkManager.ServerClientId)
                continue;

            var writer =
                new FastBufferWriter(1024,
                                     Allocator.Temp);

            writer.WriteValueSafe(json);

            NetworkManager.Singleton
                .CustomMessagingManager
                .SendNamedMessage(
                    MESSAGE, id, writer);
        }
    }

    void OnReceive(ulong sender,
                   FastBufferReader reader)
    {
        reader.ReadValueSafe(out string json);

        BaseMessage msg =
            JsonUtility.FromJson<BaseMessage>(json);

        if (msg.action == "endTurn")
            GameManager.Instance.EndTurn(sender);

        else if (msg.action == "turn")
        {
            TurnMessage t =
                JsonUtility.FromJson<TurnMessage>(json);

            GameUIController.Instance
               .UpdateTurnUI(t.turnInfo);
        }
    }
}