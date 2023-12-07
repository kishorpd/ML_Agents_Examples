using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
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

    void Start()
    {
        rb = GetComponent<Rigidbody>();

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
        //Debug.LogError("X : " + forceX);
        //Debug.LogError("Y : " + forceZ);
        Vector3 force = new Vector3(forceX, 0f, forceZ).normalized * forceMagnitude;
        rb.AddForce(force, ForceMode.Impulse);

        rootX.transform.localScale = new Vector3(1,forceX,1);
        rootZ.transform.localScale = new Vector3(1,-forceZ,1);
        FuelCalculations();

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
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        sensor.AddObservation(Vector3.Distance(transform.localPosition,targetTransform.localPosition));
        sensor.AddObservation(FuelRemaining);
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
        
//        Debug.LogError("X : " + continuousActions[0]) ;
  //      Debug.LogError("Y : " + continuousActions[1]) ;
        
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        
//        Debug.LogError("X : " + actions.ContinuousActions[0]) ;
  //      Debug.LogError("Y : " + actions.ContinuousActions[1]) ;

        forceX = actions.ContinuousActions[0];
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

    private int DefaultUpgradeCutoff = 10;
    private int UpgradeCount = 0;
    
    
    void ChangeRewardPosition()
    {
        switch (CurrentPhase)
        {
            case Phases.StraightLine:
                    targetTransform.localPosition = new Vector3(targetTransform.localPosition.x + 0.1f, 0, 0);
                break;
            case Phases.StraightLineLittleRandom:
                    targetTransform.localPosition = new Vector3(targetTransform.localPosition.x + 0.1f, 0, Random.Range(-1,1));
                break;
            case Phases.StraightLineMoreRandom:
                    targetTransform.localPosition = new Vector3(targetTransform.localPosition.x + 0.1f, 0, Random.Range(-2,2));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (targetTransform.localPosition.x >= 12)
        {
            targetTransform.localPosition  = new Vector3(2,0,0);

            if (CurrentPhase != Phases.StraightLineMoreRandom)
            {
                UpgradeCount++;
                if (UpgradeCount == DefaultUpgradeCutoff)
                {
                    if (CurrentPhase == Phases.StraightLine) CurrentPhase = Phases.StraightLineLittleRandom;
                    if (CurrentPhase == Phases.StraightLineLittleRandom) CurrentPhase = Phases.StraightLineMoreRandom;
                }
            }

        }
        
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
        ResetScene();
    }
}
