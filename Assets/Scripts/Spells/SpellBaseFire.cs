using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBaseFire : SpellBase
{
    [SerializeField] Projectile projectile;
    [SerializeField] Vector3 speed;

    public override void Use()
    {
        base.Use();

        Projectile p = Instantiate(projectile);
        Vector3 v = transform.right * speed.x + transform.up * speed.y + transform.forward * speed.z;
        p.Setup(transform.position, v);
    }
}
