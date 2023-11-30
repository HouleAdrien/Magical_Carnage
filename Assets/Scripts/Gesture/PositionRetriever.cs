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
    public Dictionary<SpellType, List<List<Vector3>>> LoadAllGestures()
    {
        Dictionary<SpellType, List<List<Vector3>>> allGestures = new Dictionary<SpellType, List<List<Vector3>>>();

        foreach (SpellType spell in System.Enum.GetValues(typeof(SpellType)))
        {
            allGestures.Add(spell, RetrieveGestures(spell.ToString()));
        }

        return allGestures;
    }

    private List<List<Vector3>> RetrieveGestures(string spellName)
    {
        List<List<Vector3>> gesturesForSpell = new List<List<Vector3>>();

        TextAsset jsonFile = Resources.Load<TextAsset>(spellName);

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
                        gesturesForSpell.Add(gesture);
                    }
                }
            }
        }

        return gesturesForSpell; // Retourne la liste de tous les gestes pour le sortilège
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
}
