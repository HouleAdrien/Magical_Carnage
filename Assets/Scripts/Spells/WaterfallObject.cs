using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WaterfallObject : MonoBehaviour
{
    [SerializeField] GameObject visualObject;
    [SerializeField] AnimationCurve fallCurve;
    [SerializeField] float fallDuration; float d;
    [SerializeField] float height;
    [SerializeField] float duration;
    [SerializeField] int damage;
    SpellEffect effect;

    public void Setup(SpellEffect yahnisEffect)
    {
        effect = yahnisEffect;

        if (visualObject.TryGetComponent(out MeshRenderer mr))
        {
            mr.sharedMaterial = new Material(visualObject.GetComponent<MeshRenderer>().sharedMaterial);
            mr.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_BaseColor", effect.ElementData.PrimaryColor);
        }
        else if (visualObject.TryGetComponent(out VisualEffect ve))
        {
            ve.SetInt("effectId", (int)effect.Element);//.SetVector4("Color", color);
        }
    }

    private void FixedUpdate()
    {
        d += Time.deltaTime;
        float t = d / fallDuration;

        visualObject.transform.localPosition = new Vector3(visualObject.transform.localPosition.x, fallCurve.Evaluate(Mathf.Min(1, Mathf.Max(0, t))) * height, visualObject.transform.localPosition.z);
        GetComponent<BoxCollider>().center = visualObject.transform.localPosition;
        GetComponent<BoxCollider>().size = visualObject.transform.localScale;

       duration -= Time.deltaTime;
        if(duration < 0) { Destroy(gameObject); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Entity e))
        {
            e.ReceiveDamage(Mathf.CeilToInt(damage * effect.ElementData.Damage),effect.Element);
        }
    }
}
