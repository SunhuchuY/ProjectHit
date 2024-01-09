using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class MovementInput
    {
        private float _horizontal;
        public float horizontal 
        {
            get
            {
                return _horizontal;
            }

            set
            {
                _horizontal = Mathf.Clamp(value, -1, 1);
            }
        }

        private float _vertical;
        public float vertical
        {
            get
            {
                return _vertical;
            }

            set
            {
                _vertical = Mathf.Clamp(value, -1, 1);
            }
        }

        public void SetZero()
        {
            _horizontal = 0;
            _vertical = 0;
        }
    }

    public class InputManager : MonoBehaviour
    {
        public MovementInput movementInput { get; } = new MovementInput();
        public bool isMouseButtonDown { get; set; }
    
        private void Update()
        {
            isMouseButtonDown = Input.GetMouseButtonDown(0);
            MovementInputUpdate();
        }

        private void OnDisable()
        {
            isMouseButtonDown = false;
            movementInput.SetZero();
        }

        private void MovementInputUpdate()
        {
            movementInput.vertical = Input.GetAxisRaw("Vertical");
            movementInput.horizontal = Input.GetAxisRaw("Horizontal");
        }

    }
}

