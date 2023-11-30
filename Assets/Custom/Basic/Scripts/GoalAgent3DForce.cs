using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class GoalAgent3DForce : Agent
{

    [SerializeField] private Transform targetTransform;

    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer FloorMeshRenderer;
    
    
    
    public float forceMagnitude = 10f;
    public ParticleSystem particleS;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        //Lets continue tomorrow
    }
    
    float forceX;
    float forceZ;

    void FixedUpdate()
    {
        forceX = Input.GetAxis("Horizontal");
        forceZ = Input.GetAxis("Vertical");
        
        Debug.LogError("X : " + forceX);
        Debug.LogError("Y : " + forceZ);
        Vector3 force = new Vector3(forceX, 0f, forceZ).normalized * forceMagnitude;
        rb.AddForce(force, ForceMode.Impulse);

        ParticleSystem.ForceOverLifetimeModule forceOverLifetime = particleS.forceOverLifetime;
        forceOverLifetime.x = force.x;
        forceOverLifetime.z = force.z;

    }
    

    public override void OnEpisodeBegin()
    {
        transform.localPosition = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
        
        Debug.LogError("X : " + continuousActions[0]) ;
        Debug.LogError("Y : " + continuousActions[1]) ;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        
        Debug.LogError("X : " + actions.ContinuousActions[0]) ;
        Debug.LogError("Y : " + actions.ContinuousActions[1]) ;

        forceX = 1;// actions.ContinuousActions[0];
        forceZ = 1;//actions.ContinuousActions[1];
        
        Vector3 force = new Vector3(forceX, 0f, forceZ).normalized * forceMagnitude;
        rb.AddForce(force, ForceMode.Impulse);

        ParticleSystem.ForceOverLifetimeModule forceOverLifetime = particleS.forceOverLifetime;
        forceOverLifetime.x = force.x;
        forceOverLifetime.z = force.z;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(1f);
            FloorMeshRenderer.material = winMaterial;
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            FloorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
        
        
    }
}
