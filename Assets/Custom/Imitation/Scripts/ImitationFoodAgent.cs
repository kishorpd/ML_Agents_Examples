using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.Sentis.Layers;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;

public class ImitationFoodAgent : Agent
{

 //   public event EventHandler OnAteFood;
    public event EventHandler OnEpisodeBeginEvent;

    [SerializeField] private FoodSpawner foodSpawner;
    [SerializeField] private RewardButton rewardButton;



    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer FloorMeshRenderer;

    [SerializeField] private Rigidbody rbAgent;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = Vector3.zero;

        OnEpisodeBeginEvent?.Invoke(this, EventArgs.Empty);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(rewardButton.CanUseButton() ? 1 : 0); //1

        Vector3 dirToFoodButton = (rewardButton.transform.localPosition - transform.localPosition).normalized;
        sensor.AddObservation(dirToFoodButton.x); //1
        sensor.AddObservation(dirToFoodButton.z); //1

        sensor.AddObservation(foodSpawner.HasFoodSpawned() ? 1 : 0); //1

        if (foodSpawner.HasFoodSpawned()) //2
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

        //Total : 6
    }

    public void SpawnFood()
    {
        foodSpawner.SpawnFood();
    }
    public void ResetButton()
    {
        rewardButton.ResetButton();
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
            case 2: addForce.x = +1f; break;
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
            
            if (rewardButton.CanUseButton())
            {
                rewardButton.UseButton();
                SpawnFood();
                AddReward(1f);

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
            case  0: discreteActions[0] = 0; break;
            case +1: discreteActions[0] = 2; break;
        }


        switch (Mathf.RoundToInt(Input.GetAxisRaw("Vertical")))
        {
            case -1: discreteActions[1] = 1; break;
            case  0: discreteActions[1] = 0; break;
            case +1: discreteActions[1] = 2; break;

        }

        discreteActions[2] = Input.GetKey(KeyCode.E) ? 1 : 0; // Use Action
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.TryGetComponent<RewardButton>(out RewardButton rewardButtonOther))
        {
            rewardButtonOther.EnableUseButton(); 
        }

        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            foodSpawner.ChangeFoodLocation();

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


    private void OnTriggerExit(Collider other)
    {


        if (other.TryGetComponent<RewardButton>(out RewardButton rewardButtonOther))
        {
            rewardButtonOther.DisableUseButton();
        }
    }


    void Penalize(float penalty = -1f)
    {
        SetReward(penalty);
     //   FloorMeshRenderer.material = loseMaterial;
        EndEpisode();
    }

    void GiveRewards(float reward = 1f)
    {
        SetReward(1f);
     //   FloorMeshRenderer.material = winMaterial;
        ResetButton();

        EndEpisode();

    }
}
