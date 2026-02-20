using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections;

public class GameUIController : MonoBehaviour                   // THIS SCRIPT WILL CONTROLS THE GAME UI, THAT INCLUDES PLAY AND END TURN BUTTONS
{
    public Button playButton;
    public Button endButton;

    private PlayerState localPlayer;

    IEnumerator Start()                                         // WAITING FOR THE LOCAL PLAYER TO BE ASSIGNED TO USE THE UI
    {
        while (!NetworkManager.Singleton.IsConnectedClient)
            yield return null;

        foreach (var p in
                 FindObjectsOfType<PlayerState>())
        {
            if (p.clientId ==
                NetworkManager.Singleton.LocalClientId)
            {
                localPlayer = p;
                break;
            }
        }
    }

    public void OnPlay()                                        // PLAY BUTTON USED TO PLAY THE SELECTED CARDS
    {
        if (localPlayer == null) return;
        if (GameManager.Instance.CurrentTurn != localPlayer) return;

        localPlayer.PlayCards();
    }

    public void OnEnd()                                          // END TURN WILL ENDS THE TURN AND GOES TO THE OTHER PLAYER
    {
        if (localPlayer == null) return;

        EndTurnMessage msg = new EndTurnMessage
        {
            action = "endTurn",
            clientId = localPlayer.clientId
        };

        string json = JsonUtility.ToJson(msg);

        NetworkGameManager.Instance.SendToServer(json);
    }
}
