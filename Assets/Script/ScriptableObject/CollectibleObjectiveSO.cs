using UnityEngine;

[CreateAssetMenu(fileName = "CollObjSO", menuName = "Scriptable Objects/Objectives/CollectibleObjSO")]
public class CollectibleObjSO : ObjectiveSO
{
    public string collectibleID;
    public int desiredAmount;
}
