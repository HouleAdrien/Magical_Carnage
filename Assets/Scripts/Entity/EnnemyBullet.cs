using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float upSpeed;
    [SerializeField] float randomness;
    [SerializeField] int damages;

    public void Setup(GameObject target)
    {
        Vector3 d = (target.transform.position - transform.position);
        Vector3 v = new Vector3(d.x, 0, d.z);

        Vector3 right = Vector3.Cross(v.normalized, Vector3.up);

        Vector3 vr = v + (Random.Range(-randomness, randomness) * Vector3.up) + (Random.Range(-randomness, randomness) * right);

        GetComponent<Rigidbody>().velocity = (vr * speed) + (d.magnitude * Vector3.up * upSpeed) ;

        Destroy(gameObject, 3);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Player p))
        {
            p.ReceiveDamages(Mathf.CeilToInt(damages));
        }
    }
}
