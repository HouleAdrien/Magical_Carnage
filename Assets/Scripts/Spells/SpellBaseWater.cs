using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBaseWater : SpellBase
{
    [SerializeField] WaterfallObject waterfallObject;
    [SerializeField] float maxDistance;
    [SerializeField] LayerMask layer;

    public override void Use(Transform caster, SpellEffect effect)
    {
        base.Use(caster,effect);

        WaterfallObject p = Instantiate(waterfallObject);
        p.Setup(effect);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistance, layer))
        {
            p.transform.position = hit.point;
            p.transform.rotation = Quaternion.LookRotation(p.transform.position - transform.position, Vector3.up);
        }
    }
}