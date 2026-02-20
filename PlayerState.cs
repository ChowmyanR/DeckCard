using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerState : MonoBehaviour
{
    public ulong clientId;
    public int score;

    public Deck deck;
    public List<CardData> playedCards = new();

    public bool IsLocalPlayer()
    {
        return clientId ==
               NetworkManager.Singleton.LocalClientId;
    }

    public void StartTurn()                     // STARTS THE TURN
    {
        playedCards.Clear();
    }

    public void PlayCards()                                         // PLAYS THE SELECTED CARDS TO THE PLAYAREA
    {
        playedCards = deck.GetSelectedCards();

        foreach (var ui in
                 deck.handArea.GetComponentsInChildren<CardUI>())
        {
            if (ui.IsSelected())
                ui.MoveTo(deck.playArea);
        }
    }

    public (int atk, int def) GetStats()                       // CALCULATES THE TOTAL ATK AND DEF FROM THE PLAYED CARDS
    {
        int atk = 0, def = 0;

        foreach (var c in playedCards)
        {
            atk += c.attack;
            def += c.defense;
        }

        return (atk, def);
    }
                                                            // RESETS THE TURN
    public void ResetTurn()
    {
        playedCards.Clear();
    }
}
