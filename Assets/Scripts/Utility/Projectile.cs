using UnityEngine;
using UnityEngine.VFX;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject visualObject;
    [SerializeField] ParticleSystem[] explosions;
    [SerializeField] float explosionRadius;
    [SerializeField] int damages;
    Vector3 initialPosition;
    Vector3 initialVelocity; SpellEffect effect;

    public virtual void Setup(Vector3 ip, Vector3 iv,SpellEffect _effect)
    {
        effect = _effect;
        initialPosition = ip;
        initialVelocity = iv;

        if(visualObject.TryGetComponent(out MeshRenderer mr))
        {
            mr.sharedMaterial = new Material(visualObject.GetComponent<MeshRenderer>().sharedMaterial);
            mr.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_BaseColor", effect.ElementData.PrimaryColor);
        }
        else if(visualObject.TryGetComponent(out VisualEffect ve))
        {
            ve.SetInt("effectId", (int)effect.Element);//.SetVector4("Color", color);
        }

        foreach(ParticleSystem ps in explosions)
        {
            ParticleSystem.MainModule main = ps.main;
            main.startColor = effect.ElementData.PrimaryColor;
        }


        transform.position = initialPosition;
        transform.rotation = Quaternion.LookRotation(initialVelocity);
        GetComponent<Rigidbody>().velocity = initialVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ParticleSystem ps in explosions)
        {
            ps.gameObject.transform.parent = null;
            ps.gameObject.SetActive(true);
            Destroy(ps.gameObject, 1);
        }

        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider c in cols)
        {
            if(c.TryGetComponent(out Entity e))
            {
                e.ReceiveDamage(damages * effect.ElementData.Damage, effect.Element);
            }
        }

        Destroy(gameObject);
    }
}
