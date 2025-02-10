using UnityEngine;

public class SailController : MonoBehaviour
{
    public Transform[] sails; 
    public Rigidbody shipRigidbody; 
    public Vector3 windDirection = new Vector3(1, 0, 0);
    public float maxSailAngle = 45f;  
    public float windStrength = 10f;  

    private float currentSailAngle = 0f;

    void Update()
    {
        if (shipRigidbody.linearVelocity.magnitude < 0.1f) return; 

        float speed = shipRigidbody.linearVelocity.magnitude; 

    
        Vector3 shipDirection = shipRigidbody.linearVelocity.normalized;
        float angle = Vector3.SignedAngle(shipDirection, windDirection, Vector3.up);

      
        float targetSailAngle = Mathf.Lerp(0, maxSailAngle, speed / windStrength) * Mathf.Sign(angle);

    
        currentSailAngle = Mathf.Lerp(currentSailAngle, targetSailAngle, Time.deltaTime * 2f);

    
        foreach (Transform sail in sails)
        {
            if (sail != null)
            {
                sail.localRotation = Quaternion.Euler(0, currentSailAngle, 0);
            }
        }
    }
}
