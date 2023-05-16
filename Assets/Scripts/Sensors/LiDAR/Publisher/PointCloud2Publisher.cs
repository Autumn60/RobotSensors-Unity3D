using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RosMessageTypes.Sensor; 

namespace RobotSensors
{
    class PointCloud2Publisher : LiDARPublisher
    {
        [SerializeField]
        protected PointCloud2Serializer _serializer;

        private bool _init = false;

        protected override void Init()
        {
            if (!_sensor.initialized) return;

            _ros.RegisterPublisher<PointCloud2Msg>(_topicName);
            _serializer.Init(_frame_id, ref _sensor.points, _sensor.pointNum);

            _init = true;
        }

        private void OnApplicationQuit()
        {
            _serializer.Dispose();
        }

        protected override void Publish(float time)
        {
            if (!_init)
            {
                if (_sensor.initialized) Init();
                return;
            }
            _sensor.CompleteJob();
            _serializer.Serialize(time);
            _ros.Publish(_topicName, _serializer.msg);
        }
    }
}
