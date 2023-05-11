using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotSensors
{
    public class Publisher<T> : MonoBehaviour
    {
        [SerializeField]
        private float _frequency = 10.0f;

        protected float _time_now = 0.0f;
        private float _time_old = 0.0f;

        private float _frequency_inv;

        private void Start()
        {
            _time_old = Time.time;
            _frequency_inv = 1.0f / _frequency;
        }

        private void Update()
        {
            _time_now = Time.time;
            if(_time_now - _time_old > _frequency_inv)
            {
                Publish();
            }
        }

        protected void Publish()
        {
        
        }
    }
}