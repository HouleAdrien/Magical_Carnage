using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneObject : MonoBehaviour
{
    [SerializeField] GameObject visualObject;
    [SerializeField] ParticleSystem[] explosions;
    [SerializeField] float explosionRadius;
    [SerializeField] int damages;
    SpellEffect effect;
    public void Setup(SpellEffect _effect)
    {
        effect = _effect;
        ParticleSystem.MainModule mod = visualObject.GetComponent<ParticleSystem>().main;
        mod.startColor = effect.ElementData.PrimaryColor;

        for (int i = 0; i < visualObject.transform.childCount;i++)
        {
            if (visualObject.transform.GetChild(i).TryGetComponent(out ParticleSystem p))
            {
                mod = p.main;
                mod.startColor = effect.ElementData.PrimaryColor;
            }
        }

        foreach (ParticleSystem ps in explosions)
        {
            ParticleSystem.MainModule main = ps.main;
            main.startColor = effect.ElementData.PrimaryColor;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Entity e))
        {
            foreach (ParticleSystem ps in explosions)
            {
                ps.gameObject.transform.parent = null;
                ps.gameObject.SetActive(true);
                Destroy(ps.gameObject, 1);
            }

            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider c in cols)
            {
                if (c.TryGetComponent(out Entity ee))
                {
                    e.ReceiveDamage(damages * effect.ElementData.Damage);
                }
            }

            Destroy(gameObject);
        }
    }
}
