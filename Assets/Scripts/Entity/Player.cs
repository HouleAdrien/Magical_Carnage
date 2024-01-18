using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] int health; public int Health { get { return health; } } public float HealthRatio { get { return health / (float)maxHealth; } }

    int killCount; public int KillCount { get { return killCount; } }

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
        //Destroy(gameObject);
        SceneManager.LoadScene(0);
    }

    public void AddKill() { killCount++; }

    public void ReceiveDamages(int d)
    {
        health -= d;
        if(health < 0) { Death(); }
        if(health > maxHealth) { health = maxHealth; }
    }
}
