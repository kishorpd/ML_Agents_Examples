using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Android;
using Random = UnityEngine.Random;

public class GoalAgent3DForce : Agent
{

    [SerializeField] private Transform targetTransform;

    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer FloorMeshRenderer;
    
    public enum Phases
    {
        StraightLine,
        StraightLineLittleRandom,
        StraightLineMoreRandom
    }

    public Phases CurrentPhase = Phases.StraightLine;
    
    public float forceMagnitude = 10f;
    public GameObject rootX;
    public GameObject rootZ;
    private Rigidbody rb;
    public GameObject FuelIndicator;
    public float DefaultFuelSize = 100;
    public float DefaultConsumptionRate = 0.4f;


    public ParticleSystem posBoosterX;
    public ParticleSystem negBoosterX;
    public ParticleSystem posBoosterZ;
    public ParticleSystem negBoosterZ;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InitBoosters();
        
        
        targetTransform.localPosition = new Vector3(Random.Range(-rangeH,rangeH), 0, -rangeV);
        
   //     ResetScene();
    }
    
    float forceX;
    float forceZ;
    public float debugForceMultiplier = 3;

    void FixedUpdate()
    {
        //KeyBased();
        ForceApplication();
    }
    
    void KeyBased()
    {
        forceX = Input.GetAxis("Horizontal");
        forceZ = Input.GetAxis("Vertical");
        
        ForceApplication();
    }

    void ForceApplication()
    {
        rb.AddForce(CalculateForce(135, forceX*forceMagnitude), ForceMode.Impulse);
        rb.AddForce(CalculateForce(45, forceZ*forceMagnitude), ForceMode.Impulse);

        rootX.transform.localScale = new Vector3(1,-forceX,1);
        rootZ.transform.localScale = new Vector3(1,-forceZ,1);
        FuelCalculations();
        UpdateBoosters();
    }

    Vector3 CalculateForce(float angle,float fMagnitude)
    {
        
        float xComponent = Mathf.Cos(angle * Mathf.Deg2Rad) * fMagnitude;
        float zComponent = Mathf.Sin(angle * Mathf.Deg2Rad) * fMagnitude;
        return new Vector3(xComponent, 0f, zComponent);

    }

    void UpdateBoosters()
    {
        UpdateBooster(forceZ, posBoosterZ);
        UpdateBooster(-forceZ, negBoosterZ);
        UpdateBooster(-forceX, posBoosterX);
        UpdateBooster(forceX, negBoosterX);
    }

    void InitBoosters()
    {
        posBoosterX.Play();
        negBoosterX.Play();
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
        ResetScene();
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
        //Total : 13
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = -Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");

        //UpdateBoosters();


//        Debug.LogError("X : " + continuousActions[0]) ;
//      Debug.LogError("Y : " + continuousActions[1]) ;

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
        
//        Debug.LogError("X : " + actions.ContinuousActions[0]) ;
  //      Debug.LogError("Y : " + actions.ContinuousActions[1]) ;

        forceX = -actions.ContinuousActions[0];
        forceZ = actions.ContinuousActions[1];
        
      //  ForceApplication();
    }

    private void ResetScene()
    {
        FuelRemaining = DefaultFuelSize;
        transform.localPosition = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }


    public float rangeH = 1;
    public float rangeV = 1;
    void ChangeRewardPosition()
    {

        /*
        ChangeRewardPositionRadial();
        */
        /*
    private int DefaultUpgradeCutoff = 10;
    private int UpgradeCount = 0;
        
        switch (CurrentPhase)
        {
            case Phases.StraightLine:
                    targetTransform.localPosition = new Vector3(targetTransform.localPosition.x + 0.1f, 0, 0);
                break;
            case Phases.StraightLineLittleRandom:
                    targetTransform.localPosition = new Vector3(targetTransform.localPosition.x + 0.1f, 0, Random.Range(-.5f,.5f));
                break;
            case Phases.StraightLineMoreRandom:
                    targetTransform.localPosition = new Vector3(targetTransform.localPosition.x + 0.1f, 0, Random.Range(-.7f,.7f));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        
        if (targetTransform.localPosition.x >= 8)
        {
            targetTransform.localPosition  = new Vector3(2,0,0);

            if (CurrentPhase != Phases.StraightLineMoreRandom)
            {
                UpgradeCount++;
                if (UpgradeCount == DefaultUpgradeCutoff)
                {
                    if (CurrentPhase == Phases.StraightLine) CurrentPhase = Phases.StraightLineLittleRandom;
                    else
                    if (CurrentPhase == Phases.StraightLineLittleRandom) CurrentPhase = Phases.StraightLineMoreRandom;
                    UpgradeCount = 0;
                }
            }

        }
        */
        targetTransform.localPosition = new Vector3(Random.Range(-4,4), 0, Random.Range(-4,0));
    }
    
    void ChangeRewardPositionRadial()
    {
        //TODO : Add or subtract an offset.
        Vector2 center = new Vector2(0, 0); // Center of the circle
        float innerRadius = 0.5f; // Inner radius of the circle
        float outerRadius = 1.2f; // Outer radius of the circle
        //float maxDist = 2.0f;

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

    void Penalize(float penalty = -1f)
    {
        SetReward(penalty);
        FloorMeshRenderer.material = loseMaterial;
        EndEpisode();
        ResetScene();
    }

    void GiveRewards(float reward = 1f)
    {
        SetReward(1f);
        FloorMeshRenderer.material = winMaterial;
        
        //Change position only when it succeeds
        ChangeRewardPosition();

        EndEpisode();
      //  ResetScene();
    }
}
