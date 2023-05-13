using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotSensors
{
    [RequireComponent(typeof(LiDARSensor))]
    public class LiDARPublisher : Publisher<LiDARSensor>
    {
        [SerializeField]
        private string _topicName = "lidar_points";
        [SerializeField]
        private string _frame_id = "lidar_link";

        [SerializeField]
        private LiDARSerializer _serializer;

        protected override void Start()
        {
            base.Start();
            if (!Application.isPlaying) return;
            _serializer.Start();
        }

        protected override void Init()
        {
            //_ros.RegisterPublisher<SentenceMsg>(_topicName);
            _serializer.Init(_frame_id);
        }

        protected override void Update()
        {
            base.Update();
            /*
            _serializer.Update();
            if (!Application.isPlaying && (_serializer.format.updated))
            {
                EditorUtility.SetDirty(this);
            }
            */
        }

        protected override void Publish(float time)
        {
            /*
            _serializer.Serialize(time, _sensor.coordinate, _sensor.velocity);
            _ros.Publish(_topicName, _serializer.msg);
            */
        }
    }
}
