using UnityEngine;

[CreateAssetMenu(fileName = "ItemListSO", menuName = "Scriptable Objects/ItemListSO")]
public class ItemListSO : ScriptableObject {
    public ItemSO HealPotion;
    public ItemSO DefencePotion;
    public ItemSO ExpPotion;
    public ItemSO SpeedPotion;
}
