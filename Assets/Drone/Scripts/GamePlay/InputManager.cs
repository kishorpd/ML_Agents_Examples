﻿using UnityEngine;
using UnityEngine.InputSystem;

namespace Drone.Scripts.GamePlay
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        #region Variables

        private Vector2 _cyclic;
        private float _pedals;
        private float _throttle;

        public Vector2 Cyclic => _cyclic;
        public float Pedals => _pedals;
        public float Throttle => _throttle;

        #endregion

        #region Main Methods

        public void Inputs(Vector2 directionalTiltMove, float turn, float upDown)
        {
            _cyclic = directionalTiltMove;
            _pedals = turn;
            _throttle = upDown;
        }


        #endregion

        #region Input Methods

        private void OnCyclic(InputValue value)
        {
            _cyclic = value.Get<Vector2>();
            
        }

        private void OnPedals(InputValue value)
        {
            _pedals = value.Get<float>();
        }

        private void OnThrottle(InputValue value)
        {
            _throttle = value.Get<float>();
        }
        #endregion
    }
}

