using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class HandPin : MonoBehaviour
{
    [SerializeField] Transform pin;
    [SerializeField] LayerMask layer;
    [SerializeField] Vector2 distanceRange;
    [SerializeField] Vector2 sizes;

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1000000, layer))
        {
            float r = Mathf.Clamp((hit.distance - distanceRange.x) / (distanceRange.y - distanceRange.x),0,1);
            pin.transform.position = hit.point;
            pin.transform.localScale = Vector3.one * Mathf.Lerp(sizes.x, sizes.y, r);
        }
    }
}
