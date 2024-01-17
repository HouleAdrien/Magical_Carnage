using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Vector2 spawnRate;
    [SerializeField] AnimationCurve spawnCurve;
    [SerializeField] float spawnDuration; float d;
    [SerializeField] int maxCount;

    [SerializeField] Transform sp;
    [SerializeField] GameObject ennemyPrefab;
    int entityCount;

    private void Start()
    {
        d = Random.Range(0, spawnRate.x);
    }

    private void Update()
    {
        d += Time.deltaTime;

        float r = Mathf.Lerp(spawnRate.x, spawnRate.y, spawnCurve.Evaluate(d / spawnDuration));

        if(d > r && entityCount < maxCount)
        {
            Spawn();
            d = 0;
        }
    }

    public void DeleteEntity()
    {
        entityCount--;
    }

    void Spawn()
    {
        Vector3 d = new Vector3(Random.Range(-sp.localScale.x, sp.localScale.x), 0, Random.Range(-sp.localScale.z, sp.localScale.z));
        GameObject g = Instantiate(ennemyPrefab, sp.position + d, sp.rotation);
        g.GetComponent<Entity>().Setup(this);
        entityCount++;
    }
}
