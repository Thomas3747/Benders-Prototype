using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public class TouchManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction TouchPositionAction;
    private InputAction TouchPressAction;
    [SerializeField]
    private GameObject player;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
       TouchPositionAction = playerInput.actions["TouchDrag"];
        TouchPressAction = playerInput.actions["TouchPress"];
    }
    private void OnEnable()
    {
        TouchPressAction.performed += TouchPressed;
    }

    private void OnDisable()
    {
        TouchPressAction.performed -= TouchPressed;
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        TouchState input = context.ReadValue<TouchState>();       
        //Touchl position = Camera.main.ScreenToWorldPoint(TouchPositionAction.ReadValue<Touch>());
       // position.z = player.transform.position.z;
    }
}
