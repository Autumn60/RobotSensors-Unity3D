using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RosMessageTypes.Sensor;

namespace RobotSensors
{
    [RequireComponent(typeof(RGBCameraSensor))]
    public class RGBDCameraPublisher : Publisher<RGBDCameraSensor>
    {
        private RGBDCameraSerializer _serializer;

        [SerializeField]
        private string _topicName = "imu/raw_data";
        [SerializeField]
        private string _frame_id = "imu_link";

        protected override void Init()
        {
            _ros.RegisterPublisher<CompressedImageMsg>(_topicName);
            _serializer = new RGBDCameraSerializer();
            _serializer.Init(_frame_id);
        }

        protected override void Publish(float time)
        {
            _serializer.Serialize(time, _sensor.texture, _sensor.quality);
            _ros.Publish(_topicName, _serializer.msg);
        }
    }
}