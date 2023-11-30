using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public enum SpellType
{
    Fire, Water, Air, Electricity, Vitality, Earth
}

public class InputManager : MonoBehaviour
{
    #region BackButtons
    [SerializeField] private InputAction rightTrigger;
    [SerializeField] private InputAction leftTrigger;

    [SerializeField] private InputAction rightGrip;
    [SerializeField] private InputAction leftGrip;
    #endregion

    #region FrontButtons
    [SerializeField] private InputAction x;
    [SerializeField] private InputAction y;
    [SerializeField] private InputAction b;
    [SerializeField] private InputAction a;
    #endregion

    private XRNode rightHandNode = XRNode.RightHand;
    private XRNode leftHandNode = XRNode.LeftHand;

    private bool saveMode = false;

    private List<Vector3> leftHandPositions = new List<Vector3>();
    private List<Vector3> rightHandPositions = new List<Vector3>();

    private bool isRecordingRight = false;
    private bool isRecordingLeft = false;

    private SpellType? rightHandSpell = null;
    private SpellType? leftHandSpell = null;

    private PositionSaver positionSaver = new PositionSaver();
    private PositionRetriever positionRetriever = new PositionRetriever();
    private Dictionary<SpellType, List<List<Vector3>>> spellGestures;


    void Start()
    {
        spellGestures = positionRetriever.LoadAllGestures();

        rightTrigger.performed += _ => StartRecordingRight();
        rightTrigger.canceled += _ => StopRecordingRight();
        rightTrigger.Enable();

        leftTrigger.performed += _ => StartRecordingLeft();
        leftTrigger.canceled += _ => StopRecordingLeft();
        leftTrigger.Enable();

        rightGrip.performed += _ => RightGrip();
        rightGrip.Enable();

        leftGrip.performed += _ => LeftGrip();
        leftGrip.Enable();

        x.performed += _ => XButton();
        x.Enable();

        y.performed += _ => YButton();
        y.Enable();

        b.performed += _ => BButton();
        b.Enable();

        a.performed += _ => AButton();
        a.Enable();
    }

    private void Update()
    {
        if (isRecordingRight)
        {
            rightHandPositions.Add(InputTracking.GetLocalPosition(rightHandNode));
        }
        if (isRecordingLeft)
        {
            leftHandPositions.Add(InputTracking.GetLocalPosition(leftHandNode));
        }
    }

    void StartRecordingRight()
    {
        if (!rightHandSpell.HasValue)
        {
            isRecordingRight = true;
            rightHandPositions.Clear();
        }
    }

    void StopRecordingRight()
    {
        isRecordingRight = false;

        if (saveMode)
        {
            // Ajouter la liste actuelle au gestionnaire de sauvegarde
            positionSaver.AddPositionList(rightHandPositions);
        }

        if (rightHandPositions.Count > 0 && !rightHandSpell.HasValue)
        {
            rightHandSpell = DetectSpellType(rightHandPositions);
            CheckSpells();
        }
        rightHandPositions.Clear();
    }

    void StartRecordingLeft()
    {
        if (!leftHandSpell.HasValue)
        {
            isRecordingLeft = true;
            leftHandPositions.Clear();
        }
    }

    void StopRecordingLeft()
    {
        isRecordingLeft = false;

        if (saveMode)
        {
            // Ajouter la liste actuelle au gestionnaire de sauvegarde
            positionSaver.AddPositionList(leftHandPositions);
        }


        if (leftHandPositions.Count > 0 && !leftHandSpell.HasValue)
        {
            leftHandSpell = DetectSpellType(leftHandPositions);
            CheckSpells();
        }
        leftHandPositions.Clear();
    }


    void RightGrip()
    {
        Debug.Log("right grip");
    }

    void LeftGrip()
    {
        Debug.Log("left grip");
    }

    void XButton()
    {
        Debug.Log("X button");
    }

    void YButton()
    {
        Debug.Log("Y button");
    }

    void BButton()
    {
        Debug.Log("B button");
    }

    void AButton()
    {
        Debug.Log("A button");
    }

    private void OnEnable()
    {
        rightTrigger.Enable();
        leftTrigger.Enable();
        rightGrip.Enable();
        leftGrip.Enable();
        x.Enable();
        y.Enable();
        b.Enable();
        a.Enable();
    }

    private void OnDisable()
    {
        rightTrigger.Disable();
        leftTrigger.Disable();
        rightGrip.Disable();
        leftGrip.Disable();
        x.Disable();
        y.Disable();
        b.Disable();
        a.Disable();
        if (saveMode)
        {
            // Sauvegarder toutes les positions dans un fichier JSON à la fin
            positionSaver.SaveAllPositionsToJson(Application.persistentDataPath + "/all_positions.json");
        }
    }


    private SpellType DetectSpellType(List<Vector3> recordedGesture)
    {
        SpellType detectedSpell = SpellType.Fire;
        float bestMatchPercentage = 0f;
        Dictionary<SpellType, float> averageMatchPercentages = new Dictionary<SpellType, float>();

        foreach (var spellGesture in spellGestures)
        {
            float totalMatchPercentage = 0f;
            int gestureCount = spellGesture.Value.Count;

            foreach (var gesture in spellGesture.Value)
            {
                totalMatchPercentage += CalculateMatchPercentage(recordedGesture, gesture);
            }

            float averageMatchPercentage = gestureCount > 0 ? totalMatchPercentage / gestureCount : 0f;
            averageMatchPercentages.Add(spellGesture.Key, averageMatchPercentage);

            if (averageMatchPercentage > bestMatchPercentage)
            {
                bestMatchPercentage = averageMatchPercentage;
                detectedSpell = spellGesture.Key;
            }
        }

       // LogSpellMatches(averageMatchPercentages);

        return detectedSpell;
    }

    private void LogSpellMatches(Dictionary<SpellType, float> averageMatchPercentages)
    {
        foreach (var spell in averageMatchPercentages)
        {
            Debug.Log(spell.Key + " match: " + spell.Value.ToString("F2") + "%");
        }
    }

    private float CalculateMatchPercentage(List<Vector3> recordedGesture, List<Vector3> referenceGesture)
    {
        // Normalisation des gestes
        List<Vector3> normalizedRecorded = MovementUtils.NormalizePositions(recordedGesture);
        List<Vector3> normalizedReference = MovementUtils.NormalizePositions(referenceGesture);

        // Assurer que les deux listes ont la même longueur pour la comparaison
        int targetLength = Mathf.Max(normalizedRecorded.Count, normalizedReference.Count);
        List<Vector3> interpolatedRecorded = InterpolateToLength(normalizedRecorded, targetLength);
        List<Vector3> interpolatedReference = InterpolateToLength(normalizedReference, targetLength);

        // Comparer les gestes en prenant en compte les inversions potentielles
        float normalMatch = CalculateGestureDifference(interpolatedRecorded, interpolatedReference);
        float invertedMatch = CalculateGestureDifference(interpolatedRecorded, InvertGesture(interpolatedReference));

        float betterMatch = Mathf.Min(normalMatch, invertedMatch);

        // Convertir la différence en pourcentage de correspondance
        float maxPossibleDifference = CalculateMaxPossibleDifference(targetLength);
        float matchPercentage = (1 - (betterMatch / maxPossibleDifference)) * 100;
        matchPercentage = Mathf.Clamp(matchPercentage, 0, 100);

        return matchPercentage;
    }

    private float CalculateGestureDifference(List<Vector3> gestureA, List<Vector3> gestureB)
    {
        float difference = 0f;
        for (int i = 0; i < gestureA.Count; i++)
        {
            difference += Vector3.Distance(gestureA[i], gestureB[i]);
        }
        return difference;
    }

    private List<Vector3> InvertGesture(List<Vector3> gesture)
    {
        List<Vector3> inverted = new List<Vector3>(gesture);
        inverted.Reverse();
        return inverted;
    }

    private float CalculateMaxPossibleDifference(int length)
    {
        // Cette fonction calcule la différence maximale possible entre deux gestes
        // Cela dépend de la façon dont vous voulez mesurer cette différence
        // Exemple simple : longueur * distance maximale possible entre deux points
        return length * 0.4f; // MaxPointDistance est une constante à définir
    }


    private List<Vector3> InterpolateToLength(List<Vector3> positions, int targetLength)
    {
        List<Vector3> interpolated = new List<Vector3>();

        if (positions.Count > 1)
        {
            for (int i = 0; i < targetLength; i++)
            {
                float lerpFactor = (float)i / (targetLength - 1) * (positions.Count - 1);
                interpolated.Add(Vector3.Lerp(positions[(int)lerpFactor], positions[Mathf.Min((int)lerpFactor + 1, positions.Count - 1)], lerpFactor % 1));
            }
        }
        else if (positions.Count == 1)
        {
            // Dans le cas où il n'y a qu'un seul point, on le répète simplement
            for (int i = 0; i < targetLength; i++)
            {
                interpolated.Add(positions[0]);
            }
        }

        return interpolated;
    }

    private void CheckSpells()
    {
        if (rightHandSpell.HasValue && leftHandSpell.HasValue)
        {
            Debug.Log($"Right Hand Spell: {rightHandSpell}, Left Hand Spell: {leftHandSpell}");
            UnlockHands();
        }
    }

    private void UnlockHands()
    {
        rightHandSpell = null;
        leftHandSpell = null;
    }

}
