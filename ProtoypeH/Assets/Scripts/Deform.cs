using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deform : MonoBehaviour
{
    //[Range(0, 10)]
    [SerializeField] float deformRadius = 0.2f;
    [SerializeField] float hardness = 0f;
    //[Range(0, 1)]
    [SerializeField] float damageFalloff = 1;
    //[Range(0, 10)]
    [SerializeField] float damageMultiplier = 1;
    //[Range(0, 100000)]
    [SerializeField] float minDamage = 1;

    public AudioClip[] collisionSounds;
 
    private MeshFilter filter;
    //private Rigidbody physics;
    private MeshCollider coll;
    private Vector3[] startingVerticies;
    private Vector3[] meshVerticies;

    [SerializeField] MeshFilter[] carParts;
 
    void Start()
    {
        CombineInstance[] combine = new CombineInstance[carParts.Length];
        
        for (int i = 0; i < carParts.Length; i++)
        {
            combine[i].mesh = carParts[i].sharedMesh;
            combine[i].transform = carParts[i].transform.localToWorldMatrix;

        }
        
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);
        
        GetComponent<MeshFilter>().sharedMesh = combinedMesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = combinedMesh;

        filter = GetComponent<MeshFilter>();
 
        if (GetComponent<MeshCollider>())
            coll = GetComponent<MeshCollider>();
 
        startingVerticies = filter.mesh.vertices;
        meshVerticies = filter.mesh.vertices;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        float collisionPower = collision.impulse.magnitude;

        float maxDeform = collisionPower / (hardness * 1000);

        if (collisionPower > minDamage)
        {
            if (collisionSounds.Length > 0)
                AudioSource.PlayClipAtPoint(collisionSounds[Random.Range(0, collisionSounds.Length)], transform.position, 0.5f);
 
            foreach (ContactPoint point in collision.contacts)
            {
                for (int i = 0; i < meshVerticies.Length; i++)
                {
                    Vector3 vertexPosition = meshVerticies[i];
                    Vector3 pointPosition = transform.InverseTransformPoint(point.point);
                    float distanceFromCollision = Vector3.Distance(vertexPosition, pointPosition);
                    float distanceFromOriginal = Vector3.Distance(startingVerticies[i], vertexPosition);
 
                    if (distanceFromCollision < deformRadius && distanceFromOriginal < maxDeform) // If within collision radius and within max deform
                    {
                        float falloff = 1 - (distanceFromCollision / deformRadius) * damageFalloff;
 
                        float xDeform = pointPosition.x * falloff;
                        float yDeform = pointPosition.y * falloff;
                        float zDeform = pointPosition.z * falloff;
 
                        xDeform = Mathf.Clamp(xDeform, 0, maxDeform);
                        yDeform = Mathf.Clamp(yDeform, 0, maxDeform);
                        zDeform = Mathf.Clamp(zDeform, 0, maxDeform);
 
                        Vector3 deform = new Vector3(xDeform, yDeform, zDeform);
                        meshVerticies[i] -= deform * damageMultiplier;
                    }
                }
            }
 
            UpdateMeshVerticies();
        }
    }
 
    void UpdateMeshVerticies()
    {
        filter.mesh.vertices = meshVerticies;
        coll.sharedMesh = filter.mesh;
    }
}