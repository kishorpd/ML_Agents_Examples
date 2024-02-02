using UnityEngine;
using UnityEngine.InputSystem;

public class RocketBooster : MonoBehaviour
{
    public float localThrust = 1f; // Adjust the force magnitude as needed

    private Rigidbody targetRigidbody;

    private void Start()
    {
        // Get the target Rigidbody (attach this script to the RocketBooster)
        targetRigidbody = GetComponent<Rigidbody>();
    }


    public bool bInput = false;

    bool positive = true;

    private void Update()
    {
        bInput = false;
        //bInput = (Input.GetKey(KeyCode.J : KeyCode.L));
        if (Input.GetKey(KeyCode.J))
        {
            bInput = true;
            positive = false;
        }
        if(Input.GetKey(KeyCode.L))
        {
            bInput = true;
            positive = true;
        }
    }

    private void FixedUpdate()
    {
        if(bInput)
        {
            AddForceOrTorque(1);
        }
    }

    /// <summary>
    /// Depending on the location of the rocket booster
    /// </summary>
    public void AddForceOrTorque(float thrust)
    {

        // Calculate the booster's forward direction
        Vector3 boosterForward = -transform.forward;

        // Apply force to the target Rigidbody along the booster's forward direction
        targetRigidbody.AddForce(boosterForward * localThrust * thrust*(positive? 1 : -1), ForceMode.Impulse);
    }

}