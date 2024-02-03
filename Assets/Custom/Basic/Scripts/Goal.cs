using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Goal : MonoBehaviour
{

    
    [SerializeField] private Material NoneMat;
    [SerializeField] private Material WinMat;
    [SerializeField] private Material LoseMat;

    [SerializeField] private MeshRenderer meshRenderer;

    public enum GoalState
    {
        None,
        Win,
        Lose
    }

    public GoalState currentGoalState;


    public void Awake()
    {
        Default();
    }

    public void Won()
    {
        currentGoalState = GoalState.Win;
        UpdateMaterial();
    }
    
    public void Lost()
    {

        currentGoalState = GoalState.Lose;
        UpdateMaterial();
    }

    
    public void Default()
    {

        currentGoalState = GoalState.None;
        UpdateMaterial();
    }




    void UpdateMaterial()
    {
        switch (currentGoalState)
        {
            case GoalState.None : meshRenderer.material = NoneMat;   break;
            case GoalState.Win  : meshRenderer.material = WinMat;    break;
            case GoalState.Lose : meshRenderer.material = LoseMat;   break;
        }
    }
}
