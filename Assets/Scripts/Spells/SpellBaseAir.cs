using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBaseAir : SpellBase
{
    [SerializeField] TornadoObject tornadoObject;

    public override void Use(Transform caster, SpellEffect effect)
    {
        base.Use(caster,effect);

        TornadoObject p = Instantiate(tornadoObject);
        Vector3 v = transform.forward; v.y = 0; v.Normalize();
        p.Setup(effect, v);
        p.transform.position = transform.position;

    }
}
