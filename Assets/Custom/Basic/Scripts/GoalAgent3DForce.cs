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
    
    
    
    public float forceMagnitude = 10f;
    public GameObject rootX;
    public GameObject rootZ;
    private Rigidbody rb;

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
        rootZ.transform.localScale = new Vector3(1,forceZ,1);

    }

    public override void OnEpisodeBegin()
    {
        ResetScene();
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
        transform.localPosition = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        ChangeBallPosition();
    }

    void ChangeBallPosition()
    {
        Vector2 center = new Vector2(0, 0); // Center of the circle
        float innerRadius = 1.0f; // Inner radius of the circle
        float outerRadius = 2.0f; // Outer radius of the circle

        Vector2 newPosition = Random.insideUnitCircle.normalized * Random.Range(innerRadius, outerRadius);
        targetTransform.localPosition  = new Vector3(newPosition.x,0,newPosition.y);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(1f);
            FloorMeshRenderer.material = winMaterial;
            EndEpisode();
            ResetScene();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            FloorMeshRenderer.material = loseMaterial;
            EndEpisode();
            ResetScene();
        }
        
        
    }
}
