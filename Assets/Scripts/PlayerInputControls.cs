using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputControls : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerControls playerControls;
    public Vector2 JoystickValue { get; private set; }
    
    
    private void OnEnable()
    {       
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerControls.Player.Move.performed += OnMovedPerformed;
        playerControls.Player.Move.canceled += OnMovedPerformed;   
    }
    
    private void OnDisable()
    {       
        playerControls.Player.Move.performed -= OnMovedPerformed;
        playerControls.Player.Move.canceled -= OnMovedPerformed;
        playerControls.Player.Disable();
    }
    private void OnMovedPerformed(CallbackContext context)
    {
        JoystickValue = context.ReadValue<Vector2>();
    }
   

}
