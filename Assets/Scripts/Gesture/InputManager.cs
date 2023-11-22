using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using System.IO;

public class InputManager : MonoBehaviour
{
    #region BackButtons
    [SerializeField] InputAction rightTrigger;
    [SerializeField] InputAction leftTrigger;

    [SerializeField] InputAction rightGrip;
    [SerializeField] InputAction leftGrip;
    #endregion
    #region FrontButtons
    [SerializeField] InputAction x;
    [SerializeField] InputAction y;
    [SerializeField] InputAction b;
    [SerializeField] InputAction a;
    #endregion

    private XRNode rightHandNode = XRNode.RightHand;
    private XRNode leftHandNode = XRNode.LeftHand;

    private List<Vector3> rightHandPositions = new List<Vector3>();
    private bool isRecording = false;


    private bool SaveMode = true;
    private PositionSaver positionSaver = new PositionSaver();
    private PositionRetriever positionRetriever = new PositionRetriever();

    private enum SpellType
    {
        Fire, Water, Air, Electricity, Vitality, Earth
    }

    void Start()
    {

        List<Vector3> averageGestures = positionRetriever.RetrieveAverageGestures();

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("Average Gestures: [");

        for (int i = 0; i < averageGestures.Count; i++)
        {
            sb.Append(averageGestures[i].ToString());
            if (i < averageGestures.Count - 1)
                sb.Append(", ");
        }

        sb.Append("]");

        // Enregistrer la chaîne de caractères dans un seul log
        Debug.Log(sb.ToString());


        rightTrigger.performed += _ => StartRecording();
        rightTrigger.canceled += _ => StopRecording();
        rightTrigger.Enable();

        leftTrigger.performed += _ => LeftTrigger();
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
        if (isRecording)
        {
            // Enregistrement de la position du contrôleur droit
            rightHandPositions.Add(InputTracking.GetLocalPosition(rightHandNode));
        }
    }


    void StartRecording()
    {
        isRecording = true;
        rightHandPositions.Clear();
    }

    void StopRecording()
    {
        isRecording = false;
        if (SaveMode)
        {
            // Ajouter la liste actuelle au gestionnaire de sauvegarde
            positionSaver.AddPositionList(rightHandPositions);
        }

        rightHandPositions.Clear();
    }

 


    void LeftTrigger()
    {
        Debug.Log("left trigger");
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
        if (SaveMode)
        {
            // Sauvegarder toutes les positions dans un fichier JSON à la fin
            positionSaver.SaveAllPositionsToJson(Application.persistentDataPath + "/all_positions.json");
        }
    }
}
