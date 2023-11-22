using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PositionSaver
{
    public List<FloatArrayWrapper> allPositions = new List<FloatArrayWrapper>();

    public void AddPositionList(List<Vector3> positionList)
    {
        FloatArrayWrapper wrapper = new FloatArrayWrapper { positions = new List<float>() };
        List<Vector3> normalizedPos = MovementUtils.NormalizePositions(positionList);
        foreach (Vector3 pos in normalizedPos)
        {
            wrapper.positions.Add(pos.x);
            wrapper.positions.Add(pos.y);
            wrapper.positions.Add(pos.z);
        }
        allPositions.Add(wrapper);
    }

    public void SaveAllPositionsToJson(string filePath)
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Positions saved to: " + filePath);
    }
}
