using UnityEngine;

[CreateAssetMenu(fileName = "Collectible", menuName = "Scriptable Objects/Collectible")]
public class CollectibleSO : ScriptableObject
{
    public Countables collectibleID;
    public ItemSO item;
}
