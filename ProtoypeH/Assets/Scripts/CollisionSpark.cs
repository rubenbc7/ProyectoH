using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSpark : MonoBehaviour
{
    public GameObject sparksPrefab; // Assign the sparks particle system prefab in the Inspector

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0]; // Get the first contact point

        Instantiate(sparksPrefab, contact.point, Quaternion.identity);
    }
}
