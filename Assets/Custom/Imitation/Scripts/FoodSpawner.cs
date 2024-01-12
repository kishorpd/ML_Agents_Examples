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

    public void ChangeFoodLocation()
    {
        target.transform.localPosition =
        new Vector3(
        target.transform.localPosition.x, transform.localPosition.y, 3);
        target.SetActive(false);
    }

    public void EnableFood()
    {
        target.SetActive(true);
    }

    public bool HasFoodSpawned()
    {
        return target.activeSelf;
    }


}
