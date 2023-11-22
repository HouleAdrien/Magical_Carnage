using System.Collections.Generic;
using UnityEngine;

public class MovementUtils
{
    public static List<Vector3> NormalizePositions(List<Vector3> positions)
    {
        Vector3 start = positions[0];
        List<Vector3> normalized = new List<Vector3>();
        foreach (var pos in positions)
        {
            normalized.Add(pos - start); // Translation
        }
        return normalized;
    }

}