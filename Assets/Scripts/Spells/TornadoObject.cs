using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TornadoObject : MonoBehaviour
{
    [SerializeField] GameObject visualObject;
    [SerializeField] float speed;
    [SerializeField] float lifetime; float currentLifetime;
    [SerializeField] AnimationCurve speedOverTime;
    [SerializeField] float randomness;
    [SerializeField] LayerMask layer;
    [SerializeField] int damage;
    SpellEffect effect;
    Vector3 direction;
    float alpha;

    public void Setup(SpellEffect yahnisEffect,Vector3 _direction)
    {
        //visualObject.GetComponent<MeshRenderer>().sharedMaterial = new Material(visualObject.GetComponent<MeshRenderer>().sharedMaterial);
        //visualObject.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_BaseColor", new Color(color.r,color.g,color.b, visualObject.GetComponent<MeshRenderer>().sharedMaterial.GetColor("_BaseColor").a));
        Vector4 alphaShave = visualObject.GetComponent<VisualEffect>().GetVector4("TornadoColor");
        alpha = alphaShave.w;
        Color color = yahnisEffect.ElementData.PrimaryColor; 
        color.a = alpha;
        visualObject.GetComponent<VisualEffect>().SetVector4("TornadoColor", color);
        direction = _direction;
        effect = yahnisEffect;
    }

    private void FixedUpdate()
    {
        currentLifetime += Time.fixedDeltaTime;

        Vector3 v = direction;
        direction += transform.right * Random.Range(-1.0f, 1.0f) * randomness;
        direction.Normalize();

        transform.Translate(v * speed * speedOverTime.Evaluate(Mathf.Min(1, currentLifetime / lifetime)));

        if (Physics.Raycast(transform.position + Vector3.up * 10, -transform.up, out RaycastHit hit, 100000, layer))
        {
            transform.position = hit.point;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out Entity e))
        {
            e.ReceiveDamage(Mathf.CeilToInt(damage * Time.fixedDeltaTime * effect.ElementData.Damage));
        }
    }
}
