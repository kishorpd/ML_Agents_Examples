using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.Sentis.Layers;
using UnityEngine;

public class ImitationFoodAgent : Agent
{

    public event EventHandler OnAteFood;
    public event EventHandler OnEpisodeBeginEvent;

    [SerializeField] private FoodSpawner foodSpawner;
    [SerializeField] private RewardButton rewardButton;



    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer FloorMeshRenderer;

    private Rigidbody rbAgent;

    private void Awake()
    {
        rbAgent = GetComponent<Rigidbody>(); 
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = Vector3.zero;

        OnEpisodeBeginEvent?.Invoke(this, EventArgs.Empty);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(rewardButton.CanUseButton() ? 1 : 0);

        Vector3 dirToFoodButton = (rewardButton.transform.localPosition - transform.localPosition).normalized;
        sensor.AddObservation(dirToFoodButton.x);
        sensor.AddObservation(dirToFoodButton.z);

        sensor.AddObservation(foodSpawner.HasFoodSpawned() ? 1 : 0);

        if (foodSpawner.HasFoodSpawned())
        {
            Vector3 dirToFood = (foodSpawner.GetLastFoodTransform().localPosition - transform.localPosition).normalized;
            sensor.AddObservation(dirToFood.x);
            sensor.AddObservation(dirToFood.z);
        }
        else
        {
            // Food not spawned
            sensor.AddObservation(0f); // x
            sensor.AddObservation(0f); // z
                                       // }
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int moveX = actions.DiscreteActions[0]; // 0 = Dont Move; 1 = Left; 2 = Right
        int moveZ = actions.DiscreteActions[1]; // 0 = Dont Move; 1 = Back; 2 = Forward

        Vector3 addForce = new Vector3(0, 0, 0);

        switch (moveX)
        {
            case 0: addForce.x = 0f; break;
            case 1: addForce.x = -1f; break;
            case 2:
                addForce.x = +1f; break;
        }
        switch (moveZ)
        {
            case 0: addForce.z = 0f; break;
            case 1: addForce.z = -1f; break;
            case 2: addForce.z = +1f; break;
        }


        float moveSpeed = 5f;
        rbAgent.velocity = addForce * moveSpeed + new Vector3(0, rbAgent.velocity.y, 0);

        bool isUseButtonDown = actions.DiscreteActions[2] == 1;
        if (isUseButtonDown)
        {
            // Use Action
            Collider[] colliderArray = Physics.OverlapBox(transform.position, Vector3.one * .5f);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<RewardButton>(out RewardButton foodButton))
                {
                    if (foodButton.CanUseButton())
                    {
                        foodButton.UseButton();
                        AddReward(1f);

                    }

                }
            }
        }
      //  AddReward(-1f / MaxStep);
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        switch (Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")))
        {
            case -1: discreteActions[0] = 1; break;
            case 0: discreteActions[0] = 0; break;
            case +1:
                discreteActions[0] = 2; break;

                switch (Mathf.RoundToInt(Input.GetAxisRaw("Vertical")))
                {
                    case -1: discreteActions[1] = 1; break;
                    case 0: discreteActions[1] = 0; break;
                    case +1: discreteActions[1] = 2; break;

                }

                discreteActions[2] = Input.GetKey(KeyCode.E) ? 1 : 0; // Use Action

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

        /*if (collision.gameObject.TryGetComponent<Wall>(out Wall wall)) {
        EndEpisode();
        */

    }




    void Penalize(float penalty = -1f)
    {
        SetReward(penalty);
        FloorMeshRenderer.material = loseMaterial;
        EndEpisode();
    }

    void GiveRewards(float reward = 1f)
    {
        SetReward(1f);
        FloorMeshRenderer.material = winMaterial;

        foodSpawner.ChangeFoodLocation();

        rewardButton.ResetButton();


        EndEpisode();

    }





}
