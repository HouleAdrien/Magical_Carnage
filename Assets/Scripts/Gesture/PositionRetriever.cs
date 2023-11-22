using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloatArrayWrapper
{
    public List<float> positions;
}

[System.Serializable]
public class FloatArrayListWrapper
{
    public List<FloatArrayWrapper> allPositions;
}

[System.Serializable]
public class PositionRetriever
{
    public List<Vector3> RetrieveAverageGestures()
    {
        List<Vector3> cumulativeGestures = new List<Vector3>();
        int gestureCount = 0;

        // Charger spécifiquement le fichier "Water.json"
        TextAsset jsonFile = Resources.Load<TextAsset>("Water");

        if (jsonFile != null)
        {
            FloatArrayListWrapper wrapperList = JsonUtility.FromJson<FloatArrayListWrapper>(jsonFile.text);

            if (wrapperList != null && wrapperList.allPositions != null)
            {
                foreach (var wrapper in wrapperList.allPositions)
                {
                    if (wrapper.positions != null)
                    {
                        List<Vector3> gesture = ConvertToVector3List(wrapper.positions);
                        AddGesture(cumulativeGestures, gesture);
                        gestureCount++;
                    }
                }
            }
        }

        if (gestureCount > 0)
        {
            return CalculateAverageGesture(cumulativeGestures, gestureCount);
        }

        return new List<Vector3>(); // Retourne une liste vide si aucun geste n'est trouvé ou si une erreur se produit
    }

    private List<Vector3> ConvertToVector3List(List<float> flatList)
    {
        List<Vector3> vectorList = new List<Vector3>();
        for (int i = 0; i < flatList.Count; i += 3)
        {
            vectorList.Add(new Vector3(flatList[i], flatList[i + 1], flatList[i + 2]));
        }
        return vectorList;
    }

    private void AddGesture(List<Vector3> cumulativeGestures, List<Vector3> gesture)
    {
        for (int i = 0; i < gesture.Count; i++)
        {
            if (i < cumulativeGestures.Count)
            {
                cumulativeGestures[i] += gesture[i];
            }
            else
            {
                cumulativeGestures.Add(gesture[i]);
            }
        }
    }

    private List<Vector3> CalculateAverageGesture(List<Vector3> cumulativeGestures, int gestureCount)
    {
        List<Vector3> averageGestures = new List<Vector3>();
        foreach (var pos in cumulativeGestures)
        {
            averageGestures.Add(pos / gestureCount);
        }
        return averageGestures;
    }
}
