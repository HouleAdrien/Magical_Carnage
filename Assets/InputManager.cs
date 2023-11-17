using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // Start is called before the first frame update
    void Start()
    {
        rightTrigger.performed += _ => RightTrigger();
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

    void RightTrigger()
    {
        Debug.Log("right trigger");
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

    // Update is called once per frame
    void Update()
    {

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
    }
}
