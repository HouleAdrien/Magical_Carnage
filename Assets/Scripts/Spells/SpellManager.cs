using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [SerializeField] [Tooltip("Array of all the elements scriptable-objects. Must ordre it by element order")] 
    MagicElement[] elements;

    [SerializeField] [Tooltip("Array of all the spell bases. Must ordre it by element order")]
    SpellBase[] spellBases;

    [SerializeField][Tooltip("Array of all the spell effects. Must ordre it by element order")]
    SpellEffect[] spellEffects;

    [SerializeField] SpellAbility spellAbilityPrefab;

    public MagicElement GetElement(MagicElement.ElementEnum e) { return elements[(int)e]; }

    public SpellBase GetSpellBase(MagicElement.ElementEnum e) { return spellBases[(int)e]; }
    public SpellEffect GetSpellEffect(MagicElement.ElementEnum e) { return spellEffects[(int)e]; }


    public SpellAbility CreateSpell(MagicElement.ElementEnum baseElement, MagicElement.ElementEnum effectElement)
    {
        SpellAbility sa = Instantiate(spellAbilityPrefab);
        sa.Build(baseElement, effectElement);
        return sa;
    }

    void Awake() { instance = this; }
    protected static SpellManager instance; public static SpellManager Instance { get { return instance; } }
}
