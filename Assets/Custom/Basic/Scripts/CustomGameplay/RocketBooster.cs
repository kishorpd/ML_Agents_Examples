using UnityEngine;
using UnityEngine.InputSystem;

public class RocketBooster : MonoBehaviour
{
    public float localThrust = 10f; // Adjust the force magnitude as needed

    public Transform referencePoint; // Assign the reference point in the Inspector
    private Rigidbody targetRigidbody;

    private void Start()
    {
        // Get the target Rigidbody (attach this script to the RocketBooster)
        targetRigidbody = GetComponent<Rigidbody>();
    }


    public bool isUp = true;
    public bool bInput = false;

    private void Update()
    {
        bInput = (Input.GetKey(isUp ? KeyCode.J : KeyCode.L));
    }

    private void FixedUpdate()
    {
        if(bInput)
        {
         //   AddForceOrTorque(1);
        }
    }

    /// <summary>
    /// Depending on the location of the rocket booster
    /// </summary>
    public void AddForceOrTorque(float thrust)
    {

        // Calculate the booster's forward direction
        Vector3 boosterForward = transform.forward;

        // Apply force to the target Rigidbody along the booster's forward direction
        targetRigidbody.AddForce(boosterForward * localThrust * thrust, ForceMode.Force);
    }
}