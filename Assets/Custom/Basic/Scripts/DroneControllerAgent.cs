using Drone.Scripts.GamePlay;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.Sentis.Layers;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneControllerAgent : Agent
{
    [SerializeField] private InputManager droneInputManager;

    [SerializeField] private Transform targetTransform;

    Goal localGoal;

    [SerializeField] private GameObject DebugInCubeElem;

    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private MeshRenderer FloorMeshRenderer;

    bool bIsStillInTrigger = false;

    void Start()
    {
        localGoal = targetTransform.GetComponent<Goal>();
        defaultMaterial = FloorMeshRenderer.material;
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = Vector3.one;
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        sensor.AddObservation(bIsStillInTrigger);
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector2 cyclic = new Vector2(
            actions.ContinuousActions[0],
            actions.ContinuousActions[1]);

        float pedals = actions.ContinuousActions[2];
        float throttle = actions.ContinuousActions[3];

        droneInputManager.Inputs(cyclic, pedals, throttle);
    }



    private void OnTriggerEnter(Collider other)
    {

        if (GameObject.ReferenceEquals(localGoal.gameObject, other.gameObject))
        {
            GiveReward();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            Penalize();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.LogError("EXIT DETECTED!");
        bIsStillInTrigger = false;
        DebugInCubeElem.SetActive(bIsStillInTrigger);
        if (GameObject.ReferenceEquals(localGoal.gameObject, other.gameObject))
        {
            Penalize();
        }
    }


    public float rangeH = 1;
    public float rangeV = 1;
    void ChangeRewardPosition()
    {
        targetTransform.localPosition = new Vector3(Random.Range(-5, 5), Random.Range(1, 3f), Random.Range(-3, 2f));
        // targetTransform.localPosition = new Vector3(Random.Range(-7, 7), Random.Range(1, 4f), Random.Range(-5, 2f));
    }

    void GiveReward()
    {
        SetReward(1f);
        localGoal.Won();
        ChangeRewardPosition();
        //Debug.LogError("WON!");

        FloorMeshRenderer.material = winMaterial;
        DelayedEndEpisode();
    }

    void Penalize()
    {
        SetReward(-1f);
        transform.localPosition = Vector3.zero;
        //  Debug.LogError("Penalized");
        localGoal.Lost();
        FloorMeshRenderer.material = loseMaterial;
        EndEpisode();
    }
    void DelayedEndEpisode()
    {

        Invoke("EndEpisode", 6);
        Invoke("DelayedReward", 6);
    }

    void DelayedReward()
    {
        if (bIsStillInTrigger)
        {
            AddReward(3f);
            ChangeRewardPosition();
            bIsStillInTrigger = false;
            DebugInCubeElem.SetActive(bIsStillInTrigger);
        }
    }

    void ResetMeshRenderer()
    {

        FloorMeshRenderer.material = defaultMaterial;
    }

    void OnTriggerStay(Collider other)
    {
        if(!bIsStillInTrigger)
        {
            bIsStillInTrigger = true;
            DebugInCubeElem.SetActive(bIsStillInTrigger);
        }
        AddReward(0.1f);
    }

}
