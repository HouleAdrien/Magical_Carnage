using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magic Element", menuName = "Magic & Spells/Magic Element", order = 1)]
public class MagicElement : ScriptableObject
{
    [SerializeField] new string name; public string Name { get { return name; } }
    [SerializeField] ElementEnum element; public ElementEnum Element { get { return element; } }
    [SerializeField] Color[] colors; public Color Color(int i) { return colors[i % colors.Length]; }
    [SerializeField] float castDelay; public float CastDelay { get { return castDelay; } }

    [SerializeField] int damage; public int Damage { get { return damage; } }
    public Color PrimaryColor { get { return Color(0); } }
    public Color SecondaryColor { get { return Color(1); } }
    public Color TertiaryColor { get { return Color(2); } }


    public enum ElementEnum
    {
        Fire,
        Water,
        Air,
        Electricity,
        Vitality,
        Earth
    }
}
