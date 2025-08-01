using UnityEngine;

[CreateAssetMenu(fileName = "CollObjSO", menuName = "Scriptable Objects/Objectives/CountableObjSO")]
public class CountableObjSO : ObjectiveSO
{
    public Countables CountableType;
    public int desiredAmount;
}
