using UnityEngine;

public enum CardType
{
    Attack,
    Defense,
    Balanced
}

[CreateAssetMenu(fileName = "NewCard",
                 menuName = "Card Game/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    public CardType cardType;

    public int attack;
    public int defense;

    public Sprite cardImage;
}
