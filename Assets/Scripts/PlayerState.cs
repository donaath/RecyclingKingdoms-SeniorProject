using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RecyclingKindoms.FinalCharacterController
{
public class PlayerState : MonoBehaviour
{
    // Start is called before the first frame update

    //Allows for viewing in editor 
    [field: SerializeField] public PlayerMovementState CurrentPlayerMovementState { get; private set; } = PlayerMovementState.Idling;
     public enum PlayerMovementState
     {
        Idling = 0,
        Walking = 1,
        Running = 2,
        Sprinting = 3,
        Jumping = 4,
        Falling = 5,
        Staring = 6,
        }

     }
}
