using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBaseElectricity : SpellBase
{
    [SerializeField] LightningObject projectile;

    public override void Use(Transform caster, SpellEffect effect)
    {
        base.Use(caster, effect);

        Entity target = null; float score = float.MaxValue;
        foreach (Entity entity in EntityManager.Instance.GetEntities())
        {
            if (entity.gameObject == caster.gameObject) { continue; }

            Vector3 d = (entity.transform.position - caster.transform.position);
            float angle = Vector3.Angle(d.normalized, caster.forward);

            if (angle < 60 && d.magnitude * angle < score)
            {
                target = entity;
                score = d.magnitude * angle;
            }
        }

        if (target != null)
        {
            LightningObject p = Instantiate(projectile);
            p.Setup(transform.position, target.gameObject, effect);
        }
    }
}