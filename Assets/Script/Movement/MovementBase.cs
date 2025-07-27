using Pathfinding;
using UnityEngine;
using Game.Utils;
using System.Collections.Generic;
using Unity.VisualScripting;

public abstract class MovementBase : MonoBehaviour {
    // Needed for the MoveRandom function to work properly
    protected bool hasStarted;

    // Components needed for pathfinding
    protected AIPath agent;
    protected Seeker seeker;
    protected GridGraph grid;
    protected Rigidbody2D rigidBody;


    // Information about the current path
    protected Path currPath;
    protected float pathLength;

    // Final separation between from the target
    [SerializeField] protected float endSep = 0.1f;

    public bool canMove;
    public bool targetReached;

    /// <summary>Makes the object move randomly within a range</summary>>
    /// <param name="centre">Centre about which we want the object to move, set to tranform.position for free movement</param>
    /// <param name="radius">radius whithin which we want the target to move</param>
    /// <param name="waitTime">Time the object waits on completing a path</param>
    /// <param name="waitTimerName">Name of the waitTimer if you need more control</param>
    protected void MoveRandom(Vector3 centre, float radius, float waitTime, string waitTimerName = "WaitTimer") {
        if (agent.reachedEndOfPath) {
            agent.SetPath(null);
            agent.canMove = false;

            float time = Random.Range(waitTime, waitTime + 1);
            time = waitTime == 0 ? 0 : time;
            if (hasStarted) {
                time = 0;
                hasStarted = false;
            }

            FunctionTimer.CreateSceneTimer(() => {
                Vector3 target = GenerateRandomTarget(centre, radius);
                seeker.StartPath(transform.position, target, (Path p) => ValidatePathLength(p, radius * 1.2f));
            }, time, waitTimerName);
        }
    }

    /// <summary>To continuosly move to a target as it's position keeps changing</summary>
    ///<param name="target">Transform of the target</param>
    ///<param name="updateTime">recalculates path after every updateTime seconds</param>
    ///<param name="timer">Timer to control update rate</param>
    ///<param name="timerName">Nmae of the timer for more control ovet it</param>
    protected void MoveToTarget(Transform target, float updateTime, ref FunctionTimer timer, string timerName = "MoveTimer") {
        if (timer != null && timer.TimeLeft() > 0) return;

        timer = FunctionTimer.CreateSceneTimer(() => {
            Vector3 targetPos = target.position;
            seeker.StartPath(transform.position, targetPos, UpdatePathData);
        }, updateTime, timerName);

    }

    /// <summary>To force a new path to a target</summary>
    ///<param name="target">Transform of the target</param>
    protected void ForceNewPath(Transform target) {
        Vector3 targetPos = target.position;
        seeker.StartPath(transform.position, targetPos, UpdatePathData);
    }


    /// <summary>To get a random target within a radius around a centre point</summary>
    ///<param name="centre">Centre of the valid target zone</param>
    ///<param name="radius">Radius of the valid target zone</param>
    /// <returns>A random target in the specified range</returns>
    protected Vector3 GenerateRandomTarget(Vector3 centre, float radius) {
        Vector3 target = Random.insideUnitCircle * radius;
        target.z = 0;
        target = centre + target;
        float nsize = grid.nodeSize / 2;

        float xbound = grid.width * nsize;
        float ybound = grid.depth * nsize;
        if (target.x < -xbound)
            target.x = -xbound + nsize;
        if (target.x > xbound)
            target.x = xbound - nsize;
        if (target.y < -ybound)
            target.y = -ybound + nsize;
        if (target.y > ybound)
            target.y = ybound - nsize;

        return target;
    }


    /// <summary>To get target away from an different object</summary>
    ///<param name="start">position the object you want to run away</param>
    ///<param name="runAwayTarget">Position of target you want to run away form</param>
    ///<param name="spread">Angular spread of the valid runaway zone in degrees</param>
    ///<param name="radius">Radius of the valid run away zone</param>
    /// <returns>A run away target</returns>
    protected Vector3 GenerateRunAwayTarget(Vector3 start, Vector3 runAwayTarget, float spread, float radius) {
        Vector2 dir = (start - runAwayTarget).normalized;
        float r = Random.Range(radius, radius + 1);
        Vector3 target = start + VectorHandler.GenerateRandomDir(dir, spread) * r;

        float nsize = grid.nodeSize / 2;

        float xbound = grid.width * nsize;
        float ybound = grid.depth * nsize;
        if (target.x < -xbound)
            target.x = runAwayTarget.x + 3;
        if (target.x > xbound)
            target.x = (runAwayTarget.x - 3) > -xbound ? (runAwayTarget.x - 3) : -xbound + nsize;
        if (target.y < -ybound)
            target.y = runAwayTarget.y + 3;
        if (target.y > ybound)
            target.y = (runAwayTarget.y - 3) > -ybound ? (runAwayTarget.y - 3) : -ybound + nsize;

        return target;
    }


    /// <summary>To check is a target is present in a paricular direction and angular range</summary>
    ///<param name="start">position of the current object</param>
    ///<param name="radius">Radius of the search zone</param>
    ///<param name="searchLayer">Layer of the target</param>
    ///<param name="ostacleLayer">Layer of obstacles</param>
    ///<param name="dir">DIrection in which you want to check for the target</param>
    ///<param name="angularSpread">Angular spread of the valid runaway zone in degrees</param>
    /// <returns>Position of the target if present, null otherwise</returns>
    protected Vector2? CheckForTarget(Vector2 start, float radius, string searchLayer, string obstacleLayer = null, Vector2 dir = new Vector2(), float angularSpread = 360) {
        int player = LayerMask.GetMask(searchLayer);
        Collider2D hit = Physics2D.OverlapCircle(start, radius, player);

        if (hit == null) return null;

        Vector2 d = ((Vector2)hit.transform.position - start).normalized;
        float r = ((Vector2)hit.transform.position - start).magnitude;

        int layer = obstacleLayer != null ? LayerMask.GetMask(obstacleLayer) : 0;
        RaycastHit2D obstacle = Physics2D.Raycast(start, d, r, layer);
        if (obstacle.collider != null) return null;

        if (dir != Vector2.zero) {
            angularSpread = Mathf.Clamp(angularSpread, 0, 360);
            float angle = Vector2.Angle(dir, d);
            if (angle > angularSpread / 2) return null;
        }

        return hit.transform.position;
    }


    /// <summary>Called automatically while generating a random path, it trims the path to the specifies length</summary>
    ///<param name="p">The path to be checked, usually recieved from the seeker when the path is calculated</param>
    ///<param name="length">Max length of the path</param>
    protected void ValidatePathLength(Path p, float length) {
        if (!p.error) {
            List<Vector3> newPath = new List<Vector3>(p.vectorPath);
            float dist = p.GetTotalLength();

            for (int i = newPath.Count - 1; i > 0; i--) {
                if (dist < length) break;

                float d = (newPath[i] - newPath[i - 1]).magnitude;
                newPath.RemoveAt(i);
                dist -= d;
            }

            agent.destination = newPath[newPath.Count - 1];
            agent.canMove = true;
            seeker.StartPath(newPath[0], newPath[newPath.Count - 1], (Path p) => { agent.canMove = true; UpdatePathData(p); });
        }
    }


    /// <summary>A simple function to update the current path data</summary>
    ///<param name="p">The new path</param>
    protected void UpdatePathData(Path p) {
        currPath = p;
        pathLength = currPath.GetTotalLength();
    }


    /// <summary>To add and initialize the basic components</summary>
    protected void InitBasicComponents() {
        agent = gameObject.AddComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        grid = AstarPath.active.data.gridGraph;

        rigidBody = GetComponent<Rigidbody2D>();

        agent.maxAcceleration = float.PositiveInfinity;
        agent.slowdownDistance = endSep;
        agent.endReachedDistance = endSep;
        agent.radius = 0.5f;
        agent.enableRotation = false;
        agent.pickNextWaypointDist = 1;
        agent.orientation = OrientationMode.YAxisForward;
        agent.whenCloseToDestination = CloseToDestinationMode.Stop;
        agent.canMove = true;
        agent.canSearch = false;
        agent.autoRepath.mode = AutoRepathPolicy.Mode.Never;

        seeker.StartPath(transform.position, transform.position);
        hasStarted = true;
        canMove = true;
    }

    /// <summary>To get the current move direction of the object</summary>
    ///<returns>Current move direction of the object</returns>
    public Vector2 GetMoveDir() {
        if (agent != null && agent.hasPath) {
            Vector3 dir = agent.desiredVelocity;
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) dir.y = 0;
            else dir.x = 0;

            if (dir.magnitude < 0.3) return Vector2.zero;
            else return dir.normalized;
        }
        return Vector2.zero;
    }

    /// <summary>To disable movement</summary>
    public void DisableMovement() {
        canMove = false;
        agent.canMove = false;
    }

    /// <summary>To enable movement</summary>
    public void EnableMovement() {
        canMove = true;
        agent.canMove = true;
    }
}
