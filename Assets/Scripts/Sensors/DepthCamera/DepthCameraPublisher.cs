using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RosMessageTypes.Sensor;

namespace RobotSensors
{
    [RequireComponent(typeof(DepthCameraSensor))]
    public class DepthCameraPublisher : Publisher<DepthCameraSensor>
    {
        private DepthCameraSerializer _serializer_img;
        private PointCloud2Serializer _serializer_pc;

        [SerializeField]
        private string _topicName_img = "depthImage";
        [SerializeField]
        private string _topicName_pc = "points";
        [SerializeField]
        private string _frame_id = "camera";

        private bool _init = false;

        protected override void Init()
        {
            if (!_sensor.initialized) return;

            if (_sensor.mode != DepthCameraSensor.DepthCamerMode.POINTCLOUD_ONLY)
            {
                _topicName_img += "/compressed";
                _ros.RegisterPublisher<CompressedImageMsg>(_topicName_img);
                _serializer_img = new DepthCameraSerializer();
                _serializer_img.Init(_frame_id);
            }

            if(_sensor.mode != DepthCameraSensor.DepthCamerMode.TEXTURE_ONLY)
            {
                _ros.RegisterPublisher<PointCloud2Msg>(_topicName_pc);
                _serializer_pc = new PointCloud2Serializer();
                _serializer_pc.Init(_frame_id, ref _sensor.points, (uint)_sensor.pointsNum);
            }

            _init = true;
        }

        private void OnApplicationQuit()
        {
            if (_sensor.mode == DepthCameraSensor.DepthCamerMode.TEXTURE_ONLY) return;
            _serializer_pc.Dispose();
        }

        protected override void Publish(float time)
        {
            if (!_init)
            {
                if (_sensor.initialized) Init();
                return;
            }

            if (_sensor.mode != DepthCameraSensor.DepthCamerMode.POINTCLOUD_ONLY)
            {
                _serializer_img.Serialize(time, _sensor.texture, _sensor.quality);
                _ros.Publish(_topicName_img, _serializer_img.msg);
            }

            if (_sensor.mode != DepthCameraSensor.DepthCamerMode.TEXTURE_ONLY)
            {
                _sensor.CompleteJob();
                _serializer_pc.Serialize(time);
                _ros.Publish(_topicName_pc, _serializer_pc.msg);
            }
        }
    }
}
