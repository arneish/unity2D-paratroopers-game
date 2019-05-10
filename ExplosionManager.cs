using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{

    public GameObject bullet;
    public float power = 10.0f;
    public float radius = 5.0f;
    public float upforce = 1.0f;
    /* void OnCollisionEnter(Collision col)
     {
         if (col.gameObject.name == "helicopter")
         {
             Destroy(col.gameObject);
         }
     }
     */
    // Update is called once per frame
    void FixedUpdate()
    {
        if (bullet == enabled)
        {
            Invoke("Detonate", 5);
        }
    }

    void Detonante()
    {
        Vector3 explosionPosition = bullet.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upforce, ForceMode.Impulse);
            }
        }
    }
}
