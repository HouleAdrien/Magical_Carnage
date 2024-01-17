using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBaseEarth : SpellBase
{
    [SerializeField] RuneObject runeObject;
    [SerializeField] float maxDistance;
    [SerializeField] LayerMask layer;

    public override void Use(Transform caster, SpellEffect effect)
    {
        base.Use(caster,effect);

        RuneObject p = Instantiate(runeObject);
        p.Setup(effect);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistance, layer))
        {
            p.transform.position = hit.point;
            Vector3 v = Vector3.Cross(p.transform.position - transform.position, hit.normal);
            p.transform.rotation = Quaternion.LookRotation(v, hit.normal);
        }
    }
}
