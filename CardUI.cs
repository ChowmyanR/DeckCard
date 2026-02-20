using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text attackText;
    public TMP_Text defenseText;

    public Image cardImage;   // ← TO ADD SPRITE IMAGE FIELD

    public CardData data;
    private PlayerState owner;
    private bool selected;

    public void Initialize(CardData card, PlayerState player)
    {
        data = card;
        owner = player;

        nameText.text = card.cardName;
        attackText.text = "ATK: " + card.attack;
        defenseText.text = "DEF: " + card.defense;

        if (card.cardImage != null)
            cardImage.sprite = card.cardImage;   // ← Assign sprite
    }

    public void OnClick()
    {
        if (!owner.IsLocalPlayer())
            return;

        selected = !selected;

        transform.localScale =
            selected ? Vector3.one * 1.1f : Vector3.one;
    }

    public bool IsSelected()
    {
        return selected;
    }

    public void MoveTo(Transform target)
    {
        transform.SetParent(target);
        transform.localScale = Vector3.one;
    }
}
