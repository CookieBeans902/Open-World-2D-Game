using UnityEngine;

[CreateAssetMenu(fileName = "CollObjSO", menuName = "Scriptable Objects/Objectives/CountableObjSO")]
public class CountableObjSO : ObjectiveSO
{
    public string collectibleID;
    public int desiredAmount;
}
