using UnityEngine;
using States;

public class Dash : PlayerState
{
    private float latestDashTime;
    public float dashTimeWindow => Time.time - latestDashTime;
    public override void Enter()
    {
        latestDashTime = Time.time;
        Debug.Log("Entered Dash state");
    }
}

