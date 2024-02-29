using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    // Public variable to set the target GameObject
    public GameObject target;

    // Speed at which the rotation occurs
    public float rotationSpeed = 5.0f;

    // Custom angle to correct the rotation by degrees
    float correctionAngle = 225.0f;

    void Update()
    {
        // Check if the target is set
        if (target != null)
        {
            // Calculate the direction to the target
            Vector3 targetDirection = target.transform.position - transform.position;

            // Calculate the rotation towards the target
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            // Apply custom correction angle
            targetRotation *= Quaternion.Euler(0, correctionAngle, 0);

            // Lerp the current rotation towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Target not set. Please assign a target GameObject in the Unity Editor.");
        }
    }
}
