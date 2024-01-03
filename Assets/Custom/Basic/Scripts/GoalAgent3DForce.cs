using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using Random = UnityEngine.Random;

public class GoalAgent3DForce : Agent
{

    [SerializeField] private Transform targetTransform;

    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer FloorMeshRenderer;
    
    public float forceMagnitude = 10f;
    public GameObject rootX;
    public GameObject rootY;
    public GameObject rootZ;
    public GameObject rootTorqY;
    private Rigidbody rb;
    public GameObject FuelIndicator;
    public float DefaultFuelSize = 100;
    public float DefaultConsumptionRate = 0.4f;


    public ParticleSystem posBoosterX;
    public ParticleSystem negBoosterX;

    public ParticleSystem posBoosterY;
    public ParticleSystem negBoosterY;

    public ParticleSystem posBoosterZ;
    public ParticleSystem negBoosterZ;

    Vector3 startingPosition;
    Quaternion startingRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InitBoosters();
        startingPosition = transform.localPosition;
        startingRotation = transform.localRotation;

        ChangeRewardPosition();

   //     targetTransform.localPosition = new Vector3(Random.Range(-rangeH,rangeH), 0, -rangeV);
    }
    
    float forceX;
    float forceY;
    float forceZ;
    float torqueY;
    public float debugForceMultiplier = 3;

    void FixedUpdate()
    {
        ForceApplication();
    }



    public float angleBetween = 0;

   // float rewardValue = 0;

    private void OnDrawGizmos()
    {

        //Handles.Label(transform.localPosition, "angle : " + getTargetAngle() + " IsTargetInFront : " + IsTargetInFront(transform,targetTransform));

        if (bChangeRewardPos)
        {
            bChangeRewardPos = false;
            targetTransform.localPosition = new Vector3(-5,0,Random.Range(-5, 1.5f));
        }
    }

    float getTargetAngle()
    {
        Vector3 LookDirection = transform.forward;
        Vector3 relativeDirection = (targetTransform.position - transform.position).normalized;
        return 135 - Vector3.Angle(LookDirection, relativeDirection);
    }


    private bool IsTargetInFront(Transform playerT, Transform targetT)
    {
        // Get the vector from the car to the waypoint
        Vector3 toWaypoint = targetT.position - playerT.position;

        // Calculate the dot product of the car's forward vector and the vector to the waypoint
        float dotProduct = Vector3.Dot(-playerT.right, toWaypoint);


        // If the dot product is negative, the waypoint is on the back side of the car
        return dotProduct < 0f;
    }

    void KeyBased()
    {
        forceX = Input.GetAxis("Horizontal");
        forceZ = Input.GetAxis("Vertical");
        
        ForceApplication();
    }

    public RocketBooster rRocketBooster;
    public RocketBooster lRocketBooster;

    void ForceApplication()
    {
        rb.AddForce(CalculateForce(135, forceX*forceMagnitude), ForceMode.Impulse);
        rb.AddForce( new Vector3(0, forceY * forceMagnitude,0), ForceMode.Impulse);
        rb.AddForce(CalculateForce(45, forceZ*forceMagnitude), ForceMode.Impulse);

        

        if(torqueY > 0) { rRocketBooster.AddForceOrTorque(math.abs(torqueY)); }
        else { lRocketBooster.AddForceOrTorque(math.abs(torqueY)); }

        rootX.transform.localScale = new Vector3(1,-forceX,1);
        rootY.transform.localScale = new Vector3(1,-forceY,1);
        rootZ.transform.localScale = new Vector3(1,-forceZ,1);

        rootTorqY.transform.localScale = new Vector3(1,-torqueY,1);

        FuelCalculations();
        UpdateBoosters();
    }

    Vector3 CalculateForce(float angle,float fMagnitude)
    {
        
        float xComponent = Mathf.Cos(angle * Mathf.Deg2Rad) * fMagnitude;
        float yComponent = fMagnitude;
        float zComponent = Mathf.Sin(angle * Mathf.Deg2Rad) * fMagnitude;
        return new Vector3(xComponent, yComponent, zComponent);

    }
    

    void UpdateBoosters()
    {
        UpdateBooster(forceZ, posBoosterZ);
        UpdateBooster(-forceZ, negBoosterZ);
        
        UpdateBooster(-forceX, posBoosterX);
        UpdateBooster(forceX, negBoosterX);

        UpdateBooster(-forceY, posBoosterY);
        UpdateBooster(forceY, negBoosterY);
    }

    void InitBoosters()
    {
        posBoosterX.Play();
        negBoosterX.Play();
        
        posBoosterY.Play();
        negBoosterY.Play();

        posBoosterZ.Play();
        negBoosterZ.Play();

    }

    void FuelCalculations()
    {
        FuelRemaining -= DefaultConsumptionRate;
        if (FuelRemaining <= 0)
        {
            Penalize();
        }

        if (FuelIndicator)
        {
            FuelIndicator.transform.localScale = new Vector3(FuelRemaining, 1.5f, 2);
        }
    }

    public override void OnEpisodeBegin()
    {
        ResetTimer(); ;
    }

    public float FuelRemaining = 100;
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);//3
        sensor.AddObservation(targetTransform.localPosition);//3
        
        var direction = (transform.localPosition - targetTransform.localPosition).normalized;
        sensor.AddObservation(direction);//3
        
        sensor.AddObservation(Vector3.Distance(transform.localPosition,targetTransform.localPosition));//3
        sensor.AddObservation(FuelRemaining);//1
        sensor.AddObservation(getTargetAngle());//1

        sensor.AddObservation(IsTargetInFront(transform,targetTransform) );//1
        //Total : 15
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = -Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
        continuousActions[2] = (Input.GetKey(KeyCode.I) ? 1 : 0) + (Input.GetKey(KeyCode.K) ? -1 : 0);
        continuousActions[3] = (Input.GetKey(KeyCode.J) ? 1 : 0) + (Input.GetKey(KeyCode.L) ? -1 : 0);
    }

    public int EmmisionRateForParticles = 80;

    public void UpdateBooster(float boost, ParticleSystem boosterSys)
    {
        if(boost > 0)
        {
            boosterSys.Play();
            var emmision = boosterSys.emission;
            emmision.rateOverTime = boost * EmmisionRateForParticles;
        }
        else
        {
            boosterSys.Stop();
        }

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        forceX = -actions.ContinuousActions[0];
        forceZ = actions.ContinuousActions[1];
        forceY = actions.ContinuousActions[2];
        torqueY = actions.ContinuousActions[3];
    }

    void ResetTimer()
    {
        FuelRemaining = DefaultFuelSize;
    }

    private void ResetScene()
    {
        ResetTimer();
        //transform.localPosition = new Vector3(0,1,0);


        transform.localPosition = startingPosition;
        transform.localRotation = startingRotation;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }


    public float rangeH = 1;
    public float rangeReward = 1;
    public float rangeV = 1;

    public bool bChangeRewardPos = true;

    public bool forth = false;

    void ChangeRewardPosition()
    {

        //targetTransform.localPosition = new Vector3(Random.Range(-7,7), 3, Random.Range(-5,2f));

        targetTransform.localPosition = new Vector3(Random.Range(rangeReward, -rangeReward) * (forth ? 1 : -1), 1, Random.Range(-5, 1.5f));
      //  forth = !forth;
        //targetTransform.localPosition = new Vector3(Random.Range(-7,7), Random.Range(1, 4f), Random.Range(-5,2f));
    }
    
    void ChangeRewardPositionRadial()
    {
        //TODO : Add or subtract an offset.
        Vector2 center = new Vector2(0, 0); // Center of the circle
        float innerRadius = 0.5f; // Inner radius of the circle
        float outerRadius = 1.2f; // Outer radius of the circle

        Vector2 newPosition = Random.insideUnitCircle.normalized * Random.Range(innerRadius, outerRadius);
        targetTransform.localPosition  = new Vector3(newPosition.x,0,newPosition.y);
        //targetTransform.localPosition  = new Vector3(newPosition.x + Random.Range(2,maxDist),0,newPosition.y);
        //targetTransform.localPosition  = new Vector3(newPosition.x + Random.Range(-maxDist,maxDist),0,newPosition.y);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            GiveRewards();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            Penalize();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }

    void Penalize(float penalty = -1f)
    {
        SetReward(penalty);
        FloorMeshRenderer.material = loseMaterial;
        EndEpisode();
        ResetScene();
    }

    float maxAngle = 10;

    void GiveRewards(float reward = 1f)
    {
        float currentAngle = math.abs(getTargetAngle());
        float rewardValue = 
            ((currentAngle <= maxAngle) && IsTargetInFront(transform, targetTransform)) ? 
            (1 - (currentAngle/ maxAngle)) : 0;

        SetReward(rewardValue);
        FloorMeshRenderer.material = winMaterial;
        
        //Change position only when it succeeds
        ChangeRewardPosition();

        EndEpisode();
       // ResetScene();
    }
}
