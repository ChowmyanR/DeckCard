using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerState player1;
    public PlayerState player2;

    public int maxRounds = 6;
    private int round = 1;

    [Header("Turn Timer")]
    public float turnDuration = 30f;
    private float currentTimer;
    private bool timerRunning;

    private PlayerState currentTurn;
    

    void Awake()
    {
        Instance = this;
    }
    // WAIT FOR PLAYERS TO CONNECT BEFORE THE MATCH STARTS
    IEnumerator Start()
    {
        while (NetworkManager.Singleton == null ||
               NetworkManager.Singleton.ConnectedClientsIds.Count < 2)
        {
            yield return null;
        }

        AssignClientIds();
        StartMatch();
    }
    // ASSIGNING CLIENT IDS TO PLAYERS
    void AssignClientIds()
    {
        int i = 0;

        foreach (ulong id in
                 NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (i == 0) player1.clientId = id;
            else player2.clientId = id;
            i++;
        }
    }
    
    // CREATING DECKS, DRAWING CARDS, DECIDING THE FIRST TURN RANDOMLY
    void StartMatch()
    {
        player1.deck.CreateDeck();
        player2.deck.CreateDeck();

        for (int i = 0; i < 3; i++)
        {
            player1.deck.DrawCard();
            player2.deck.DrawCard();
        }

        currentTurn = Random.value > 0.5f ? player1 : player2;

        player1.StartTurn();
        player2.StartTurn();

        StartTurnTimer();
        SendTurnUpdate();
    }
    // STARTING THE TURN TIMER
    void StartTurnTimer()
    {
        if(!NetworkManager.Singleton.IsServer)
            return;
        currentTimer = turnDuration;
        timerRunning = true;

        StartCoroutine(TimerRoutine());
    }

    // TURN TIMER ROUTINE, IF THE TIMER EXCEEDS, THE TURN ENDS AUTOMATICALLY
    IEnumerator TimerRoutine()
    {
        while (timerRunning && currentTimer > 0f)
        {
            yield return new WaitForSeconds(1f);
            currentTimer--;

            // SEND TIMER UPDATE TO CLIENTS
            TimerMessage msg = new TimerMessage
            {
                action = "timer",
                timeLeft = (int)currentTimer
            };

                string json = JsonUtility.ToJson(msg);
                NetworkGameManager.Instance.Broadcast(json);
        }
        if (currentTimer <= 0f)
        {
            timerRunning = false;

                // CALLS AUTO END TURN
            EndTurn(currentTurn.clientId);
        }
    }

    // TURN INFO UPDATING
    void SendTurnUpdate()
    {
        string info =
            "Round: " + round +
            " | Turn: " +
            (currentTurn == player1 ?
            "Player 1" : "Player 2");

        TurnMessage msg = new TurnMessage
        {
            action = "turn",
            turnInfo = info
        };

        string json = JsonUtility.ToJson(msg);

        NetworkGameManager.Instance.Broadcast(json);
    }

    // RETURNING THE CURRENT TURN PLAYER
    public PlayerState CurrentTurn => currentTurn;
    // ENDING THE TURN AND CHECKING IF SENDER IS THE CURRENT PLAYER AND SERVER
    public void EndTurn(ulong senderId)       
    {
        if (!NetworkManager.Singleton.IsServer)  
            return;

        if (currentTurn.clientId != senderId)   
            return;

        timerRunning =false;


        if (currentTurn == player1)               
            currentTurn = player2;
        else
            ResolveRound();

        StartTurnTimer();
        SendTurnUpdate();
    }

    void ResolveRound()                                    // RESOLVING THE TURNS AND CALCULATING SCORES, THEN CHECKING THE GAME IS OVER OR NOT
                                                           // IF NOT, DRAWING NEW CARDS AND DECIDING THE TURN RANDOMLY AGAIN
    {
        var p1 = player1.GetStats();
        var p2 = player2.GetStats();
                                                            // CALCULATING THE SCORE THAT CALCULATED BY THE DIFF OF ATK AND DEF
        int d1 = Mathf.Max(0, p1.atk - p2.def);
        int d2 = Mathf.Max(0, p2.atk - p1.def);

        if (d1 > d2) player1.score++;
        else if (d2 > d1) player2.score++;

        player1.ResetTurn();                                // RESETTING THE TURN STATS FOR THE NEXT ROUND
        player2.ResetTurn();

        round++;

        if (round > maxRounds)                                 // GAME OVER SCENE WERE CALLED BY SCENE MANAGER IF THE TURN WERE EXCEED
        {
            NetworkManager.Singleton.SceneManager                                   
                .LoadScene("GameOverScene",
                           LoadSceneMode.Single);
            return;
        }

        player1.deck.DrawCard();
        player2.deck.DrawCard();

        currentTurn =
            Random.value > 0.5f ?
            player1 : player2;
        
        StartTurnTimer();
        SendTurnUpdate();
    }
}
