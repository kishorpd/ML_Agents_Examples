using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.Sentis.Layers;
using UnityEngine;

public class DroneAgent : Agent
{

    [SerializeField] private Transform targetTransform;

    [SerializeField] Goal localGoal;

    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer FloorMeshRenderer;
    public List<RocketBooster> boosters;


    public override void OnEpisodeBegin()
    {
        // transform.localPosition = Vector3.zero;
        localGoal.Default();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        for (int i = 0; i < boosters.Count; i++)
        {
            boosters[i].localThrust = actions.ContinuousActions[i];
        }
   }

    private void OnTriggerEnter(Collider other)
    {

        if (GameObject.ReferenceEquals(localGoal.gameObject, other.gameObject))
        {
            SetReward(1f);
            localGoal.Won();
            FloorMeshRenderer.material = winMaterial;
            DelayedEndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            localGoal.Lost();
            FloorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
    }

    void DelayedEndEpisode()
    {
        Invoke("EndEpisode", 1);
    }

    void OnTriggerStay(Collider other)
    {
        AddReward(0.01f);
    }
}
