using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public struct VehicleSteering
{
    public float steering;
    public float accel;
    public bool footBrake;
    public bool handBrake;

    public VehicleSteering(float steering,float accel,bool footBrake,bool handBrake)
    {
        this.steering = steering;
        this.accel = accel;
        this.footBrake = footBrake;
        this.handBrake = handBrake;
    }
}

public enum NavigationStyle
{
    NavigateByAmbientWayPoint,
    NavigateByNavAgent,
}

[RequireComponent(typeof(VehicleController))]
[RequireComponent(typeof(WaypointProgressTracker))]
[RequireComponent(typeof(WayPoint))]
[RequireComponent(typeof(Property))]
//[RequireComponent(typeof(NavMeshAgent))]         这个须在子物体中
//[RequireComponent(typeof(Animator))]
public class AIVehicleControl : MonoBehaviour {

    public VehicleController vehicle;
    public WeaponSystem weaponSystem;
    public WaypointProgressTracker waypointTracker;
    public NavMeshAgent agent;
    public Animator stateMachine;
    public Property property;

    public float steering;
    public float accel;
    public bool footBrake;
    public bool handBrake;

    [Header("Navgation")]
    public WayPoint wayPoint;           //存储agent的waypoint值
    public NavigationStyle navigationStyle = NavigationStyle.NavigateByNavAgent;
    public Vector3 destination;

    [Header("PathFollow")]
    public float steeringSensitivity = 0.05f;
    public float accleSensitivity = 0.2f;
    public float desireSpeed;
    public float angleToTurningDirection;
    public float turningCaution;
    public float turningCautionFactor = 0.1f;
    public float angularSpeedCaution;
    public float angularSpeedCautionFactor = 2.5f;
    public float currentSpeed;

    [Header("obstacle avoidence")]  
    public Transform[] sensors;
    public float obstacleCheckBaseDistance = 5;
    public float obstacleCheckSpeedFactor = 2;
    public float avoidenceSteeringFactor = 1f;
    public float avoidenceSlowAccelFactor = 1f;
    public float avoidenceCaution;

    [Header("CheckNeigbors")]
    //public float checkNeigborsDuration;
    public float checkNeigborsRadius = 10f;
    public List<Transform> neigbors = new List<Transform>();

    [Header("Separation")]
    public float separationAccelSensitivity = 0.2f;
    public float separationSteeringSenesitivity = 0.15f;
    public Vector3 separationDesiraPos;

    [Header("Reverse")]
    public float reverseDesire = 0;
    public float reverseDesireIncreseSpeed = 0.5f;
    public float reverseDesireDropSpeed = 0.5f;
    public float reverseTime;
    public float reverseDuration = 1.5f;

    [Header("State_Patrol")]
    public float patrolAlertnessIncressFactor = 1f;
    public float patrolAlertnessDropFactor = 1f;


    [Header("State_ChaseAndAttack")]
    public float battalAlertnessIncressFactor = 1f;
    public float battalAlertnessDropFactor = 1f;

    [Header("State_SerchingEnemy")]
    public float serchingAlertnessIncressFactor = 1f;
    public float serchingAlertnessDropFactor = 1f;
    public float predictEscapeDistanceFactor = 5f;
    public Vector3 predictPos;
    public float serchEnemyEndTime;
    public float serchiEnemyDuration = 5f;

    public Transform helper;
    public SMB_Patrol patrolState;
    public bool DisplaySteeringInfo = false;
    public List<string> steeringNames = new List<string>();
    public List<Vector2> steeringForces = new List<Vector2>();

    bool collided = false;
    float remainSteering;
    float remainAccel;
    float sensorCheckDistance;
    int lastWayPointCount;

    void Start () {
        wayPoint.loop = false;
        wayPoint.smooth = false;
	}

    void FixedUpdate()
    {
        agent.transform.localPosition = Vector3.zero;

        if (navigationStyle == NavigationStyle.NavigateByNavAgent)
            SyncNavAgentWayPoint();

        steering = accel = 0;
        remainAccel = remainSteering = 1;
        footBrake = false;
        handBrake = false;

        SteeringBehaviour_PathFollow();
        vehicle.Move(steering, accel, footBrake, handBrake);
    }

    public void SetDestination(Vector3 target)
    {
        destination = target;
        agent.SetDestination(destination);
        UpdateWayPoint();
    }

    void SyncNavAgentWayPoint()
    {
        if (agent.pathPending)
            return;

        if (lastWayPointCount != agent.path.corners.Length)
        {
            UpdateWayPoint();
        }
        lastWayPointCount = agent.path.corners.Length;
    }

    void UpdateWayPoint()
    {
        wayPoint.SetWayPoint(agent.path.corners);
        waypointTracker.ResetProgress();
    }

    public void ChangeNavigationToNavAgent()
    {
        if (navigationStyle == NavigationStyle.NavigateByNavAgent)
            return;

        navigationStyle = NavigationStyle.NavigateByNavAgent;
        agent.enabled = true;
        waypointTracker.wayPoint = wayPoint;
        UpdateWayPoint();
    }

    public void ChangeNavigationToWayPoint(WayPoint wayPoint)
    {
        navigationStyle = NavigationStyle.NavigateByAmbientWayPoint;
        agent.enabled = false;
        waypointTracker.wayPoint = wayPoint;
        waypointTracker.ResetProgress();
    }

    public void CheckNeigbors()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkNeigborsRadius, property.team.ToLayerMask());

        neigbors.Clear();
        for (int i = 0; i < colliders.Length; i++)
        {
            Transform rootTransform = Utility.GetRootTransform(colliders[i].transform);

            if (DisplaySteeringInfo)
                    print(rootTransform.name);


            if (rootTransform == transform)
                continue;

            neigbors.Add(rootTransform);
        }
    }

    public Vector2 PathFollow()
    {
        currentSpeed = vehicle.rigid.velocity.magnitude;

        float _steering;
        float _accel;
        desireSpeed = vehicle.maxSpeed;

        angleToTurningDirection = Vector3.Angle(transform.forward, waypointTracker.directionPoint.direction);
        turningCaution = angleToTurningDirection * turningCautionFactor;

        angularSpeedCaution = vehicle.rigid.angularVelocity.magnitude * angularSpeedCautionFactor;

        desireSpeed -= turningCaution;
        desireSpeed -= angularSpeedCaution;

        _accel = (desireSpeed - vehicle.relativeVelocity.z) * accleSensitivity;
        _accel = Mathf.Clamp(_accel, -1, 1);

        Vector3 relativeTarget = transform.InverseTransformPoint(waypointTracker.targetPoint.position);
        float angleToTarget = Mathf.Atan2(relativeTarget.x, relativeTarget.z) * Mathf.Rad2Deg;
        _steering = Mathf.Clamp(angleToTarget * steeringSensitivity, -1, 1) * Mathf.Sign(vehicle.relativeVelocity.z);
        _steering = Mathf.Clamp(_steering, -1, 1);

        return new Vector2(_steering, _accel);
    }

    public Vector2 ObstacleAvoidence()
    {
        RaycastHit nearestHitInfo = new RaycastHit();
        int nearestObstacleIndex = -1;
        float nearestObstacleDist = float.MaxValue;

        float sensorCheckDist = obstacleCheckBaseDistance + vehicle.rigid.velocity.magnitude * obstacleCheckSpeedFactor;
        sensorCheckDistance = sensorCheckDist;
        for (int i = 0; i < sensors.Length; i++)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(sensors[i].position, sensors[i].forward, out hitInfo, sensorCheckDist, Utility.ObstacleLayer))
            {
                if (hitInfo.distance < nearestObstacleDist)
                {
                    nearestObstacleIndex = i;
                    nearestObstacleDist = hitInfo.distance;
                    nearestHitInfo = hitInfo;
                }
            }
        }

        if (nearestObstacleIndex == -1)
            return Vector2.zero;

        Vector3 relativeHitPos = transform.InverseTransformPoint(nearestHitInfo.point);
        float overshoot = sensorCheckDist - nearestObstacleDist;
        float _steering = -1/ relativeHitPos.x * overshoot*avoidenceSteeringFactor*Mathf.Sign(vehicle.relativeVelocity.z);
        _steering = Mathf.Clamp(_steering, -1, 1);

         avoidenceCaution = avoidenceSlowAccelFactor / nearestObstacleDist;
        avoidenceCaution = Mathf.Clamp(avoidenceCaution, 0, vehicle.maxSpeed);
        float desireSpeed = vehicle.maxSpeed - avoidenceCaution;
        float _accel = Mathf.Clamp( (desireSpeed - vehicle.rigid.velocity.magnitude),-1,0);

        return new Vector2(_steering, _accel);
    }

    public Vector2 Separation()
    {
        Vector3 desirePosition = Vector3.zero;
        for (int i = 0; i < neigbors.Count; i++)
        {
            Vector3 toThis = transform.position - neigbors[i].transform.position;

            desirePosition += (toThis.normalized  / toThis.magnitude)*5;
        }
        separationDesiraPos = desirePosition;
        desirePosition = transform.InverseTransformPoint(desirePosition + transform.position);

        float angleToTarget;
        if (Utility.SnapToZero(desirePosition) == Vector3.zero)
            angleToTarget = 0;
         else
            angleToTarget = Mathf.Asin(desirePosition.x / desirePosition.magnitude);

        float steering = Mathf.Clamp(angleToTarget * separationSteeringSenesitivity, -1, 1);
        float accel = Mathf.Clamp( desirePosition.z * separationAccelSensitivity * vehicle.rigid.velocity.magnitude, -1,1);

        return new Vector2(steering, accel);
    }

    public Vector2 Reverse()
    {
        if (collided&&vehicle.rigid.velocity.sqrMagnitude < 1)
        {
            reverseDesire += reverseDesireIncreseSpeed * Time.deltaTime;
            if (reverseDesire >= 1)
            {
                reverseTime = Time.time + reverseDuration;
                reverseDesire = 0;
            }
        }
        else
        {
            reverseDesire -= reverseDesireDropSpeed * Time.deltaTime;
        }

        reverseDesire = Mathf.Clamp01(reverseDesire);
        
        collided = false;

        if (reverseTime < Time.time)
            return Vector2.zero;
        else
            return Vector2.down;
    }

    public bool AddSteering(Vector2 steeringVec)
    {
        float steeringToAdd = Mathf.Clamp(steeringVec.x, -remainSteering, remainSteering);
        steering += steeringToAdd;
        remainSteering -= Mathf.Abs(steeringToAdd);


        float accelToAdd = Mathf.Clamp(steeringVec.y, -remainAccel, remainAccel);
        accel += accelToAdd;
        remainAccel -= Mathf.Abs(accelToAdd);

        steeringForces.Add(new Vector2(steeringToAdd, accelToAdd));

        return !(remainSteering.IsApproch(0) && remainAccel.IsApproch(0));
    }

    public bool AddSteering(VehicleSteering vehicleSteering)
    {
        float steeringToAdd = Mathf.Clamp(vehicleSteering.steering, -remainSteering, remainSteering);
        steering += steeringToAdd;
        remainSteering -= Mathf.Abs(steeringToAdd);

        float accelToAdd = Mathf.Clamp(vehicleSteering.accel, -remainAccel, remainAccel);
        accel += accelToAdd;
        remainAccel -= Mathf.Abs(accelToAdd);

        return !(remainSteering.IsApproch(0) && remainAccel.IsApproch(0));
    }
    #region StateMachineBehaviour
    void SteeringBehaviour_PathFollow()
    {
        steeringNames.Clear();
        steeringForces.Clear();
        CheckNeigbors();

        steeringNames.Add("Reverse");
        if (!AddSteering(Reverse()))
            return;

        steeringNames.Add("ObstacleAvoidence");
        if (!AddSteering(ObstacleAvoidence()))
            return;

        steeringNames.Add("Separation");
        if (!AddSteering(Separation()))
            return;

        steeringNames.Add("PathFollow");
        if (!AddSteering(PathFollow()))
            return;
    }


    public void StateUpdate_Patrol()
    {
        SteeringBehaviour_PathFollow();
        vehicle.Move(steering, accel, false, false);
        //GeneralVisualPerception();

        //stateMachine.SetFloat(AnimPara.AnimPara_Alertness, alertness);
    }


    public void StateEnter_ChaseAndAttack()
    {
        //alertnessIncressFactor = battalAlertnessIncressFactor;
        //alertnessDropFactor = battalAlertnessDropFactor;
        ChangeNavigationToNavAgent();
    }

    public void StateUpdate_ChaseAndAttack()
    {
        //SetDestination(enemyLastSpottedPos);
        SteeringBehaviour_PathFollow();
        vehicle.Move(steering, accel, false, false);
        //GeneralVisualPerception();
        //weaponSystem.SetTarget(enemyLastSpottedPos);
        //if(targetedEnemy != null)
            weaponSystem.Trigger();
         
        //stateMachine.SetFloat(AnimPara.AnimPara_Alertness, alertness);
    }

    public void StateEnter_SerchingEnemy()
    {
        //alertnessIncressFactor = serchingAlertnessIncressFactor;
        //alertnessDropFactor = serchingAlertnessDropFactor;
        serchEnemyEndTime = Time.time+serchiEnemyDuration;
        ChangeNavigationToNavAgent();

        //Vector3 _predictPos = enemyLastSpottedPos + enemyLastSpottedVelocity * predictEscapeDistanceFactor;
        NavMeshHit hitInfo;

        if (NavMesh.SamplePosition(predictPos, out hitInfo, 5f, NavMesh.AllAreas))
        {
            predictPos = hitInfo.position;
        }
        else
        {
            //predictPos = enemyLastSpottedPos;
        }
    }

    public void StateUpdate_SerchingEnemy()
    {
        SteeringBehaviour_PathFollow();
        agent.SetDestination(predictPos);

        if (Time.time > serchEnemyEndTime)
        {
            stateMachine.SetTrigger(AnimPara.EndSerchEnemy);
        }
    }


    #endregion StateMachineBehaviour
    private void OnCollisionStay(Collision collision)
    {

        if (Vector3.Dot(transform.forward, collision.contacts[0].point - transform.position) > 0)
        {
            collided = true;

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        foreach (var go in sensors)
        {
            Gizmos.DrawLine(go.position, go.position + go.forward * sensorCheckDistance);
        }

        //UnityEditor.Handles.color = Color.red;

        Gizmos.color = Color.yellow;
        if (DisplaySteeringInfo)
            Gizmos.DrawWireSphere(separationDesiraPos + transform.position, 4);
    }


    private void OnGUI()
    {
        if (!DisplaySteeringInfo)
            return;
        for (int i = 0; i < steeringNames.Count; i++)
        {
            GUI.TextArea(new Rect(0, i * 40, 200, 40),steeringNames[i] + " steer :" + steeringForces[i].x + " accel:" + steeringForces[i].y);
        }
    }


}
