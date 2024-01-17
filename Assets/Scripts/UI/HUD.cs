using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Image healthVignette;
    [SerializeField] AnimationCurve healthCurve;


    private void Update()
    {
        healthVignette.color = new Color(1,0,0, healthCurve.Evaluate(1-EntityManager.Instance.Player.GetComponent<Player>().HealthRatio));
    }
}
