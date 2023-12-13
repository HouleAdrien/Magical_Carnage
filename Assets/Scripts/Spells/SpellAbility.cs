using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAbility : MonoBehaviour
{
    protected SpellBase sbase; public SpellBase Base { get { return sbase; } }
    protected SpellEffect seffect; public SpellEffect Effect { get { return seffect; } }

    public void Build(MagicElement.ElementEnum sbase, MagicElement.ElementEnum seffect)
    {
        Build(SpellManager.Instance.GetSpellBase(sbase), SpellManager.Instance.GetSpellEffect(seffect));
    }
    public void Build(SpellBase _sbase, SpellEffect _seffect)
    {
        sbase = Instantiate(_sbase,transform);
        sbase.transform.localPosition = Vector3.zero;
        sbase.transform.localRotation = Quaternion.identity;
        sbase.Setup(this);
        seffect = _seffect;
    }

    private void OnDestroy()
    {
        Destroy(sbase.gameObject);
    }

    public void Use()
    {
        sbase.Use();
    }
}
