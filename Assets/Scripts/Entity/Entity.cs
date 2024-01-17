using Gaia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Entity : MonoBehaviour
{
    [SerializeField] Vector3 spawnPos;
    [SerializeField] GameObject prefab;

    [SerializeField] int damages;

    [SerializeField] protected int maxHealth; 
    [SerializeField] protected int health;
    [SerializeField] AnimationCurve sizeCurve;
    [SerializeField] float height;

    [SerializeField] LayerMask layer;
    [SerializeField] Vector2 speed;
    [SerializeField] Vector2 distanceRange;
    [SerializeField] AnimationCurve distanceSpeedCurve;
    [SerializeField] Transform deathVisuals;

    [SerializeField] float attackDistance;
    [SerializeField] float attackRate; float d;
    [SerializeField] GameObject bullet;
    [SerializeField] Spawner spawner;

    [SerializeField] Transform head;
    [SerializeField] MeshRenderer meshRenderer;

    MagicElement.ElementEnum type; public MagicElement.ElementEnum Type { get { return type; } }

    Transform Target { get { return EntityManager.Instance.Player.transform; } }


    public void Setup(Spawner s)
    {
        spawner = s;
    }

    protected virtual void Start()
    {
        health = maxHealth; 
        EntityManager.Instance.AddEntity(this);

        meshRenderer.sharedMaterials[0] = new Material(meshRenderer.sharedMaterials[0]);
        meshRenderer.sharedMaterials[1] = new Material(meshRenderer.sharedMaterials[1]);
        //meshRenderer.sharedMaterials[0].color = SpellManager.Instance.GetElement(type).PrimaryColor;
        meshRenderer.sharedMaterials[1].color = SpellManager.Instance.GetElement(type).PrimaryColor;
    }

    private void OnDestroy()
    {
        EntityManager.Instance.RemoveEntity(this);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * sizeCurve.Evaluate(health / (float)maxHealth);

        Vector3 d = Target.position - transform.position;
        if (d.magnitude < attackDistance)
        { 
            Attack();
        }
        float r = Mathf.Clamp((d.magnitude - distanceRange.x) / (distanceRange.y - distanceRange.x),0,1);
        float s = distanceSpeedCurve.Evaluate(r);

        transform.position = Vector3.MoveTowards(transform.position,Target.position, Mathf.Lerp(speed.x,speed.y, (health / (float)maxHealth)) * s * Time.deltaTime);

        //transform.rotation = Quaternion.LookRotation(d.normalized, Vector3.up);
        head.transform.LookAt(Target.position);//, Vector3.up);

        if (Physics.Raycast(transform.position + Vector3.up * 10, -transform.up, out RaycastHit hit, 100000, layer))
        {
            transform.position = hit.point + Vector3.up * height;
        }

        if (health < 0)
        {
            Death();
        }
    }

    void Death()
    {
        deathVisuals.parent = null;
        deathVisuals.gameObject.SetActive(true) ;
        spawner.DeleteEntity();
        Destroy(gameObject);
        Destroy(deathVisuals.gameObject, 1);
    }

    void Attack()
    {
        d += Time.deltaTime;
        if(d > 1f / attackRate)
        {
            GameObject b = Instantiate(bullet, transform.position + transform.right * 0.5f, transform.rotation);
            b.GetComponent<EnnemyBullet>().Setup(Target.gameObject);

            d = 0;
        }
    }

    public void ReceiveDamage(int d)
    {
        health -= d;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out Player p))
        {
            p.ReceiveDamages(Mathf.CeilToInt(damages * Time.fixedDeltaTime));
        }
    }
}
