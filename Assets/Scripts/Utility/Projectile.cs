using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector3 initialPosition;
    Vector3 initialVelocity;

    public virtual void Setup(Vector3 ip, Vector3 iv)
    {
        initialPosition = ip;
        initialVelocity = iv;


        transform.position = initialPosition;
        transform.rotation = Quaternion.LookRotation(initialVelocity);
        GetComponent<Rigidbody>().velocity = initialVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        Destroy(gameObject);
    }
}
