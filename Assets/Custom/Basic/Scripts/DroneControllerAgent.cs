using Drone.Scripts.GamePlay;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
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

    float timeSpentInTrigger = 1;
    float timeSpentAfterBeingInTrigger = 1;
    bool bEntryRewardGiven = false;


    [Unity.Collections.ReadOnly]
    [SerializeField]
    private float TIME_TO_STAY_IN_TRIGGER = 6;
    [Unity.Collections.ReadOnly]
    [SerializeField]
    private float TIME_TO_START_FROM= 1;

    void Init()
    {
        if(localGoal == null)
        {
            localGoal = targetTransform.GetComponent<Goal>();
            Debug.LogError(" localGoal  :" + (localGoal == null));
            defaultMaterial = FloorMeshRenderer.material;
        }
    }

    public override void OnEpisodeBegin()
    {
        Init();
        timeSpentInTrigger = TIME_TO_START_FROM;
        timeSpentAfterBeingInTrigger = 0;
        ChangeRewardPosition();
        DebugInCubeElem.transform.localScale = new Vector3 (1, 1, 0);
        transform.localPosition = Vector3.one;
        bEntryRewardGiven = false;
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        sensor.AddObservation(timeSpentInTrigger);
    }


    Vector2 cyclic = new Vector2(0,0);
    public override void OnActionReceived(ActionBuffers actions)
    {
        // Vector2 cyclic = new Vector2(
        //     actions.ContinuousActions[0],
        //     actions.ContinuousActions[1]);


        float pedals = actions.ContinuousActions[0];
        float throttle = actions.ContinuousActions[1];

        droneInputManager.Inputs(cyclic, pedals, throttle);
    }



    private void OnTriggerEnter(Collider other)
    {
        //if (GameObject.ReferenceEquals(localGoal.gameObject, other.gameObject))
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            if (!bEntryRewardGiven) GiveReward();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
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
        AddReward(1f);
        bEntryRewardGiven = true;
        localGoal.Won();
        //Debug.LogError("WON!");

        FloorMeshRenderer.material = winMaterial;
   //     DelayedEndEpisode();
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
  //      Invoke("EndEpisode", TIME_TO_STAY_IN_TRIGGER);
   //     Invoke("DelayedReward", 5.5f);
    }

    private void Update()
    {
        if(timeSpentInTrigger > TIME_TO_START_FROM)
        {
            timeSpentAfterBeingInTrigger += Time.deltaTime;

            if(timeSpentAfterBeingInTrigger > TIME_TO_STAY_IN_TRIGGER)
                EndEpisode();// starts from 0 to 6

            if (timeSpentInTrigger > TIME_TO_STAY_IN_TRIGGER)// starts from 1 to 6
            {
                Debug.LogError(" ------------- DelayedReward(after 5) ----------------");
                AddReward(3f);
                EndEpisode();
            }
        }

    }

    void ResetMeshRenderer()
    {

        FloorMeshRenderer.material = defaultMaterial;
    }

    void OnTriggerStay(Collider other)
    {


        //if (GameObject.ReferenceEquals(localGoal.gameObject, other.gameObject))
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            timeSpentInTrigger += Time.deltaTime;
            float newScale = timeSpentInTrigger / TIME_TO_STAY_IN_TRIGGER;
            DebugInCubeElem.transform.localScale = new Vector3(newScale, 1, newScale);
            AddReward(0.1f);
        }
    }

}
