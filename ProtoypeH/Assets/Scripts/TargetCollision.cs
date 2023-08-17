using UnityEngine;
using System.Collections;

public class TargetCollision : MonoBehaviour
{
    
    public Transform cameraToShake;
    public float maxShakeIntensity = 0.5f;
    public float shakeDuration = 0.5f;

    private Vector3 originalCameraPosition;
    private float currentShakeDuration;
    private float currentShakeIntensity;

    private void Start()
    {
        originalCameraPosition = cameraToShake.localPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Calculate shake intensity based on collision force
        float collisionForce = collision.impulse.magnitude;
        currentShakeIntensity = Mathf.Clamp(collisionForce * 0.05f, 0f, maxShakeIntensity);
        
        // Start shaking the camera
        currentShakeDuration = shakeDuration;
        StartCoroutine(ShakeCamera());
    }

    private IEnumerator ShakeCamera()
    {
        while (currentShakeDuration > 0)
        {
            Vector3 randomShake = Random.insideUnitSphere * currentShakeIntensity;
            cameraToShake.localPosition = originalCameraPosition + randomShake;

            currentShakeDuration -= Time.deltaTime;
            yield return null;
        }

        cameraToShake.localPosition = originalCameraPosition;
    }
}
