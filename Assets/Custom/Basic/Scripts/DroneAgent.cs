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

    Goal localGoal;

    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private MeshRenderer FloorMeshRenderer;
    public List<RocketBooster> boosters;

    bool bIsStillInTrigger = false;


    void Start()
    {
        localGoal = targetTransform.GetComponent<Goal>();
        defaultMaterial = FloorMeshRenderer.material;
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0,3,0);
        transform.localEulerAngles = Vector3.zero;
        localGoal.Default();

        bIsStillInTrigger = false;
        Invoke("ResetMeshRenderer", 2);


    }


    float minRotation = 80;
    float maxRotation = 280;

    private void Update()
    {
        var localRotation = transform.localEulerAngles;
        if(
            (Mathf.Abs(localRotation.x) > minRotation && Mathf.Abs(localRotation.x) < maxRotation) ||
            (Mathf.Abs(localRotation.y) > minRotation && Mathf.Abs(localRotation.y) < maxRotation) ||
            (Mathf.Abs(localRotation.z) > minRotation && Mathf.Abs(localRotation.z) < maxRotation)
            )
        {
            //Debug.LogError(" localRotation : " + localRotation);
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


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        sensor.AddObservation(bIsStillInTrigger);
        sensor.AddObservation(Mathf.Abs(targetTransform.localEulerAngles.x));
        sensor.AddObservation(Mathf.Abs(targetTransform.localEulerAngles.y));
        sensor.AddObservation(Mathf.Abs(targetTransform.localEulerAngles.z));
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
            GiveReward();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            Penalize();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        bIsStillInTrigger = false;
        if (GameObject.ReferenceEquals(localGoal.gameObject, other.gameObject))
        {
            Penalize();
        }
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
        if(bIsStillInTrigger)
        {
            AddReward(3f);
        }
    }

    void ResetMeshRenderer()
    {

        FloorMeshRenderer.material = defaultMaterial;
    }

    void OnTriggerStay(Collider other)
    {
        bIsStillInTrigger = true;
        AddReward(0.1f);
    }
}
