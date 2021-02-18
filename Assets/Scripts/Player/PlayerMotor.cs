using UnityEngine;
using Assets.Scripts.Generic;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMotor : Motor
    {
        public void HandleMovementInput(CallbackContext context)
        {
            SetMovementVector(context.ReadValue<Vector2>());
        }
    }
}

