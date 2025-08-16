using UnityEngine;

[CreateAssetMenu(fileName = "ObjectiveSO", menuName = "Scriptable Objects/Objectives/ObjectiveSO")]
public class ObjectiveSO : ScriptableObject
{
    public string objectiveDesc;
    public string FullLengthObjectiveDesc;
    public NpcID targetNpcID;
}
