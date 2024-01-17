using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SpellBaseVitality : SpellBase
{
    [SerializeField] float duration;
    [SerializeField] Transform lineRenderersParent;
    [SerializeField] float turbulenceAmplitude;
    [SerializeField] float turbulenceFrequence;

    [SerializeField] float maxDistance;
    List<Entity> entities; 
    List<List<Vector3>> entitiesDeltas;
    List<List<Vector3>> entitiesDirections;
    [SerializeField] int damages;
    SpellEffect effect;

    IEnumerator UpdateCoroutine(Transform caster)
    {
        float d = 0;
        while (d < duration)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Vector3 p = caster.position - Vector3.up * 0.5f;
                Vector3 dd = entities[i].transform.position - p;
                for (int j = 0; j < 100; j++)
                {
                    lineRenderersParent.GetChild(i).GetComponent<LineRenderer>().SetPosition(j, (p + (dd * (j / 99f))) + entitiesDeltas[i][i]);
                    if (j != 0 && j != 99)
                    {
                        entitiesDeltas[i][j] += entitiesDirections[i][j];
                        entitiesDirections[i][j] += Time.deltaTime * turbulenceFrequence * new Vector3(Random.Range(-turbulenceAmplitude, turbulenceAmplitude), Random.Range(-turbulenceAmplitude, turbulenceAmplitude), Random.Range(-turbulenceAmplitude, turbulenceAmplitude));
                    }
                }

                entities[i].ReceiveDamage(Mathf.CeilToInt(damages * effect.ElementData.Damage * Time.deltaTime));
            }
            yield return new WaitForEndOfFrame();
            d += Time.deltaTime;
        }

        lineRenderersParent.gameObject.SetActive(false);

        Destroy(transform.parent.gameObject);
    }

    void SetupLines(SpellEffect _effect)
    {
        effect = _effect;
        entitiesDirections = new List<List<Vector3>>();
        entitiesDeltas = new List<List<Vector3>>();
        while (lineRenderersParent.childCount < entities.Count) 
        {
           Instantiate(lineRenderersParent.GetChild(0).GetComponent<LineRenderer>(), lineRenderersParent);    
        }
        for (int i = 0; i < entities.Count; i++)
        {
            lineRenderersParent.GetChild(i).GetComponent<LineRenderer>().sharedMaterial = new Material(lineRenderersParent.GetChild(i).GetComponent<LineRenderer>().sharedMaterial);
            lineRenderersParent.GetChild(i).GetComponent<LineRenderer>().sharedMaterial.SetColor("_Color", effect.ElementData.PrimaryColor);
            lineRenderersParent.GetChild(i).GetComponent<LineRenderer>().positionCount = 100;
            entitiesDirections.Add(new List<Vector3>());
            entitiesDeltas.Add(new List<Vector3>());
            for (int j = 0; j < 100; j++)
            {
                if (j != 0 && j != 99)
                {
                    entitiesDirections[i].Add(new Vector3(Random.Range(-turbulenceAmplitude, turbulenceAmplitude), Random.Range(-turbulenceAmplitude, turbulenceAmplitude), Random.Range(-turbulenceAmplitude, turbulenceAmplitude)));
                }
                else
                {
                    entitiesDirections[i].Add(Vector3.zero);
                }
                entitiesDeltas[i].Add(Vector3.zero);
            }
        }

    }

    public override void Use(Transform caster, SpellEffect effect)
    {
        base.Use(caster,effect);

        entities = new List<Entity>(); 
        foreach (Entity entity in EntityManager.Instance.GetEntities())
        {
            if(entity.gameObject == caster.gameObject) { continue; }

            Vector3 d = (entity.transform.position - caster.transform.position) ;
            float angle = Vector3.Angle(d.normalized, caster.forward);

            if(angle < 60 && d.magnitude < maxDistance) 
            { 
                entities.Add(entity); 
            }
        }

        if (entities.Count > 0)
        {
            SetupLines(effect);

            StartCoroutine(UpdateCoroutine(caster));
        }
    }
}