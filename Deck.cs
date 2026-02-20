using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public PlayerState owner;
    public List<CardData> cardPool;

    public GameObject cardPrefab;
    public Transform handArea;
    public Transform playArea;

    private List<CardData> deck = new();
    private int index;
    // CREATING A DECK OF 12 CARDS RANDOMLY FROM THE CARD POOL
    public void CreateDeck()
    {
        deck.Clear();
        index = 0;

        for (int i = 0; i < 12; i++)
            deck.Add(cardPool[Random.Range(0, cardPool.Count)]);

        Shuffle();
    }
    // SHUFFLE THE DECK
    void Shuffle()
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            (deck[i], deck[r]) = (deck[r], deck[i]);
        }
    }
    // DRAW A CARD FROM THE DECK TO THE HAND
    public void DrawCard()
    {
        if (index >= deck.Count) return;

        var data = deck[index++];

        var obj = Instantiate(cardPrefab, handArea);
        obj.GetComponent<CardUI>()
           .Initialize(data, owner);
    }
    // PLAY A CARD TO THE PLAY AREA
    public List<CardData> GetSelectedCards()
    {
        List<CardData> selected = new();

        foreach (var ui in
                 handArea.GetComponentsInChildren<CardUI>())
        {
            if (ui.IsSelected())
                selected.Add(ui.data);
        }

        return selected;
    }
}
