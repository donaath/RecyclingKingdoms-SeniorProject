using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RecyclingKindoms.FinalCharacterController
{
    public class PlayerAnimation : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private Animator _animator;
        private PlayerLocomotionInput _playerLocomotionInput;
        private static int inputXHash = Animator.StringToHash("InputX");
        private static int inputYHash = Animator.StringToHash("InputY");

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        }

         private void Update()
        {
            UpdateAnimationState();
        }

        private void UpdateAnimationState()
        {
            Vector2 inputTarget = _playerLocomotionInput.MovementInput;

            _animator.SetFloat(inputXHash, inputTarget.x);
            _animator.SetFloat(inputYHash, inputTarget.y);

            Debug.Log("inputXHash: " + inputXHash + inputTarget.x);
            Debug.Log("inputYHash: " + inputYHash + inputTarget.y);
        }
    }
}
