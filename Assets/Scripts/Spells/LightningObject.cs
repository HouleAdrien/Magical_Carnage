using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningObject : MonoBehaviour
{
    [SerializeField] GameObject visualObject;
    [SerializeField] GameObject visualObject2;
    [SerializeField] float moveSize;
    [SerializeField] float speed;
    [SerializeField] float maxDistance;
    [SerializeField] float lifetime; float d;
    [SerializeField] int damage;
    SpellEffect effect;
    Vector3 initialPosition;
    GameObject target;

    LightningObject mainObject;

    List<Entity> victims = new List<Entity>();
    List<Entity> Victims { get { return mainObject == this ? victims : mainObject.Victims; } }

    public virtual void Setup(Vector3 ip, GameObject _target, SpellEffect yahnisEffect, bool isMain = true)
    {
        initialPosition = ip;

        visualObject.GetComponent<MeshRenderer>().sharedMaterial = new Material(visualObject.GetComponent<MeshRenderer>().sharedMaterial);
        visualObject.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_BaseColor", yahnisEffect.ElementData.PrimaryColor);

        ParticleSystem.MainModule mod = visualObject2.GetComponent<ParticleSystem>().main;
        mod.startColor = yahnisEffect.ElementData.PrimaryColor;

        for (int i = 0; i < visualObject2.transform.childCount; i++)
        {
            if (visualObject2.transform.GetChild(i).TryGetComponent(out ParticleSystem p))
            {
                mod = p.main;
                mod.startColor = yahnisEffect.ElementData.PrimaryColor;
            }
        }

        transform.position = initialPosition;
        target = _target;

        victims.Add(target.GetComponent<Entity>());

        effect = yahnisEffect;

        if (isMain) { mainObject = this; }
    }

    void CreateDuplicate(Entity t)
    {
        LightningObject lo = Instantiate(this);
        lo.mainObject = mainObject;
        lo.Setup(transform.position, t.gameObject, effect,false);
    }

    private void Update()
    {
        d += Time.deltaTime; if(d > lifetime) { Destroy(gameObject); }
        if(mainObject == null) { Destroy(gameObject); }

        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);

            if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            {
                transform.localScale = Vector3.one;

                List<Entity> targets = new List<Entity>();
                foreach (Entity entity in EntityManager.Instance.GetEntities())
                {
                    if (Victims.Contains(entity)) { continue; }

                    Vector3 d = (entity.transform.position - transform.position);

                    if (d.magnitude < maxDistance)
                    {
                        targets.Add(entity);
                    }
                }

                foreach (Entity entity in targets)
                {
                    Victims.Add(entity);
                    CreateDuplicate(entity);
                }
            }
            else
            {
                transform.localScale = Vector3.one * moveSize;
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Entity e))
        {
            e.ReceiveDamage(Mathf.CeilToInt(damage * Time.fixedDeltaTime * effect.ElementData.Damage), effect.Element);
        }
    }
}
