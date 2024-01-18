using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] Image healthVignette;
    [SerializeField] AnimationCurve healthCurve;

    [SerializeField] TMP_Text killCount;

    private void Update()
    {
        healthVignette.color = new Color(1,0,0, healthCurve.Evaluate(1-EntityManager.Instance.Player.GetComponent<Player>().HealthRatio));

        killCount.text = EntityManager.Instance.Player.GetComponent<Player>().KillCount.ToString();
    }
}
