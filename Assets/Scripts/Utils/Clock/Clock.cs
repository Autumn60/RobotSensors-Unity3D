using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Rosgraph;

namespace RobotSensors
{
    public class Clock : MonoBehaviour
    {

        [SerializeField]
        private string _topicName = "clock";

        private ROSConnection _ros;

        private ClockMsg _msg;

        void Start()
        {
            _ros = ROSConnection.GetOrCreateInstance();
            _ros.RegisterPublisher<ClockMsg>(_topicName);

            _msg = new ClockMsg();
        }

        void Update()
        {
            float time = Time.time;
            
            uint sec = (uint)Math.Truncate(time);
            _msg.clock.sec = sec;
            _msg.clock.nanosec = (uint)((time - sec) * 1e+9);

            _ros.Publish(this._topicName, _msg);
        }
    }
}
