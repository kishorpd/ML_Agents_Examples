using UnityEngine;
using UnityEngine.InputSystem;

public class RocketBooster : MonoBehaviour
{
    public float localThrust = 1f; // Adjust the force magnitude as needed

    private Rigidbody targetRigidbody;
    public GameObject debugMesh;
    Vector3 debugScale = Vector3.zero;

    bool bIsPositiveOnly = true;


    private void Start()
    {
        // Get the target Rigidbody (attach this script to the RocketBooster)
        targetRigidbody = GetComponent<Rigidbody>();
        debugScale = debugMesh.transform.localScale;
    }


    public bool bInput = false;

   // bool positive = true;

    private void Update()
    {
     //   bInput = false;
        //bInput = (Input.GetKey(KeyCode.J : KeyCode.L));
       // localThrust = Input.GetAxis("Horizontal");
        UpdateDebugMesh();
        //  positive = localThrust > 0f;


    }


    private void OnDrawGizmos()
    {

      //  debugScale = debugMesh.transform.localScale;
       // UpdateDebugMesh();
    }

    void UpdateDebugMesh()
    {
        if (debugMesh != null)
        {
            debugMesh.transform.localScale = new Vector3(1, 1,
                bIsPositiveOnly ? -Mathf.Abs(localThrust) : localThrust
                );
        }
    }

    private void FixedUpdate()
    {
        //if(bInput)
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
        float finalThrust = bIsPositiveOnly ? -Mathf.Abs(localThrust) : localThrust;
        // Apply force to the target Rigidbody along the booster's forward direction
        targetRigidbody.AddForce(-boosterForward * localThrust * finalThrust, ForceMode.Impulse);
    }

}