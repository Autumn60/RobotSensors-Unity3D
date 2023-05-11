using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.ROSTCPConnector;

namespace RobotSensors
{
    public class Publisher<T> : MonoBehaviour
    {
        [SerializeField]
        private float _frequency = 10.0f;

        protected ROSConnection _ros;
        protected T _sensor;

        protected float _time_now = 0.0f;
        private float _time_old = 0.0f;

        private float _frequency_inv;

        private void Awake()
        {
            _ros = ROSConnection.GetOrCreateInstance();
            _sensor = GetComponent<T>();
        }

        private void Start()
        {
            _time_old = Time.time;
            _frequency_inv = 1.0f / _frequency;
            Init();
        }

        protected virtual void Init()
        {
        
        }

        private void Update()
        {
            _time_now = Time.time;
            if(_time_now - _time_old > _frequency_inv)
            {
                Publish(_time_now);
            }
        }

        protected virtual void Publish(float time)
        {
        
        }
    }
}