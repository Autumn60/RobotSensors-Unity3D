using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using RosMessageTypes.Nmea;

namespace RobotSensors
{
    [RequireComponent(typeof(GNSSSensor))]
    public class GNSSPublisher : Publisher<GNSSSensor>
    {
        [SerializeField]
        private string _topicName = "gnss/raw_data";
        [SerializeField]
        private string _frame_id = "gnss_link";

        [SerializeField]
        private GNSSSerializer _serializer;

        protected override void Start()
        {
            base.Start();
            if (!Application.isPlaying) return;
            _serializer.Start();
        }

        protected override void Init()
        {
            _ros.RegisterPublisher<SentenceMsg>(_topicName);
            _serializer.Init(_frame_id);
        }

        protected override void Update()
        {
            base.Update();
            _serializer.Update();
            if (!Application.isPlaying && (_serializer.format.updated))
            {
                EditorUtility.SetDirty(this);
            }
        }

        protected override void Publish(float time)
        {
            _serializer.Serialize(time, _sensor.coordinate, _sensor.velocity);
            _ros.Publish(_topicName, _serializer.msg);
        }
    }
}
