using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class ImitationFoodAgent : Agent
{

    public event EventHandler OnAteFood;
    public event EventHandler OnEpisodeBeginEvent;

    [SerializeField] private FoodSpawner foodSpawner;
    [SerializeField] private RewardButton rewardButton;

    private Rigidbody rbAgent;

    private void Awake()
    {
        rbAgent = GetComponent<Rigidbody>(); 
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = Vector3.zero;
    }
}
