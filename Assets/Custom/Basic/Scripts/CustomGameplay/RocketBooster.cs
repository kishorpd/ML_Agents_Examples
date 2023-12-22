using UnityEngine;

public class RocketBooster : MonoBehaviour
{
    public float thrust = 10f; // Adjust the force magnitude as needed

    public Transform referencePoint; // Assign the reference point in the Inspector
    private Rigidbody targetRigidbody;

    private void Start()
    {
        // Get the target Rigidbody (attach this script to the RocketBooster)
        targetRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
     //   if(referencePoint != null) { AddTorqueToRotate(); }
   //     else
        {
            // Calculate the booster's forward direction
            Vector3 boosterForward = transform.up;

            // Apply force to the target Rigidbody along the booster's forward direction
            targetRigidbody.AddForce(boosterForward * thrust, ForceMode.Force);
        }
    }


    private void AddTorque()
    {
        // Calculate the booster's forward direction
        Vector3 boosterForward = transform.forward;

        // Calculate the direction vector from booster to reference point
        Vector3 directionToReference = referencePoint.position - transform.position;

        // Calculate the rotation needed to align booster with directionToReference
        Quaternion rotationToReference = Quaternion.LookRotation(directionToReference);

        // Extract the torque axis from the rotation
        Vector3 torqueAxis = referencePoint.up;
        //float angle;
        //rotationToReference.ToAngleAxis(out angle, out torqueAxis);

        // Apply torque to the target Rigidbody around the torque axis
        targetRigidbody.AddTorque(torqueAxis * thrust, ForceMode.Force);
    }

    public float rotationSpeed = 30f; // Adjust the rotation speed as needed

    private void AddTorqueToRotate()
    {
        // Calculate the direction from A to B
        Vector3 directionToB = referencePoint.transform.position - transform.position;

        // Calculate the torque axis (cross product of forward and directionToB)
        Vector3 torqueAxis = Vector3.Cross(directionToB, transform.up);

        // Apply torque to rotate around the center axis
        referencePoint.GetComponent<Rigidbody>().AddTorque(torqueAxis * rotationSpeed, ForceMode.Force);
    }
}