using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBoosterGroup : MonoBehaviour
{
    public float value;
    public List<RocketBooster> rocketBoosterValues = new List<RocketBooster>();


    // Update is called once per frame
    void Update()
    {
        foreach (var rocketBooster in rocketBoosterValues)
        {
            rocketBooster.localThrust = value;
        }
    }
}
