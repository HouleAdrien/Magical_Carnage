using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX ;

public class SpellBase : SpellElement
{
    protected SpellAbility linkedAbility; public SpellAbility LinkedAbility { get { return linkedAbility; } }

    [SerializeField] protected List<GameObject> useVfxs;

    public void Setup(SpellAbility sa)
    {
        linkedAbility = sa;
    }

    public virtual void Use()
    {
        foreach(GameObject vfx in useVfxs)
        {
            //vfx.Play();
        }
    }
}
