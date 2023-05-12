using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RosMessageTypes.Sensor;

namespace RobotSensors
{
    [RequireComponent(typeof(DepthCameraSensor))]
    public class DepthCameraPublisher : Publisher<DepthCameraSensor>
    {
        private DepthCameraSerializer _serializer;

        [SerializeField]
        private string _topicName = "depthImage";
        [SerializeField]
        private string _frame_id = "camera";

        protected override void Init()
        {
            _topicName += "/compressed";
            _ros.RegisterPublisher<CompressedImageMsg>(_topicName);
            _serializer = new DepthCameraSerializer();
            _serializer.Init(_frame_id);
        }

        protected override void Publish(float time)
        {
            _serializer.Serialize(time, _sensor.texture, _sensor.quality);
            _ros.Publish(_topicName, _serializer.msg);
        }
    }
}
