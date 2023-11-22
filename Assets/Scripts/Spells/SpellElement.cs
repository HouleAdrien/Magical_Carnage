using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellElement : MonoBehaviour
{
    protected MagicElement.ElementEnum element; public MagicElement.ElementEnum Element { get { return element; } }
    protected MagicElement elementData; public MagicElement ElementData { get { return SpellManager.Instance.GetElement(element); } }
}
