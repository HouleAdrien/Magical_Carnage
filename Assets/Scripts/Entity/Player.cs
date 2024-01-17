using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] int health; public int Health { get { return health; } } public float HealthRatio { get { return health / (float)maxHealth; } }

    private void Awake()
    {
        EntityManager.Instance.RegisterPlayer(gameObject);
    }

    private void Start()
    {
        health = maxHealth;
    }

    void Death()
    {
        Destroy(gameObject);
    }

    public void ReceiveDamages(int d)
    {
        health -= d;
        if(health < 0) { Death(); }
    }
}
