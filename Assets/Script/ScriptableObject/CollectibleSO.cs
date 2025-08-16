using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleSO", menuName = "Scriptable Objects/CollectibleSO")]
public class CollectibleSO : ScriptableObject
{
    public Countables collectibleID;
    public ItemSO item;
}
