using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using RosMessageTypes.Sensor;

namespace RobotSensors
{
    [RequireComponent(typeof(LiDARSensor))]
    public class LiDARPublisher : Publisher<LiDARSensor>
    {
        [SerializeField]
        protected string _topicName = "lidar_points";
        [SerializeField]
        protected string _frame_id = "lidar_link";

        protected override void Init()
        {
        }

        protected void Serialize(float time)
        {
        }
    }
}
