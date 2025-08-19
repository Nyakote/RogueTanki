using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName = "Assset/Item")]

public class Item : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int itemCost;
    public Sprite itemSprite;
}
