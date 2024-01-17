using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellElement : MonoBehaviour
{
    [SerializeField] protected MagicElement.ElementEnum element; public MagicElement.ElementEnum Element { get { return element; } }
    public MagicElement ElementData { get { return SpellManager.Instance.GetElement(element); } }
}
