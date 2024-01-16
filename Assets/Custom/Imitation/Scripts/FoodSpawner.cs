using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{

    public GameObject target;

    private void OnEnable()
    {
        ChangeFoodLocation();
    }


    public Transform GetLastFoodTransform()
    {
        return target.GetComponent<Transform>();
    }

    public void SpawnFood()
    {
        ChangeFoodLocation();
        EnableFood();
    }

    public void ChangeFoodLocation()
    {
        target.transform.localPosition =
        new Vector3(
        Random.Range(-3.0f,-1), 
        transform.localPosition.y,
        Random.Range(3.0f, -3)
        );
        target.SetActive(false);
    }


    public void EnableFood()
    {
        target.SetActive(true);
    }

    public void OnDisable()
    {
        target.SetActive(false);
    }

    public bool HasFoodSpawned()
    {
        return target.activeSelf;
    }


}
