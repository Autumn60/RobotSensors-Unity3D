using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Nmea;

namespace RobotSensors
{
    [System.Serializable]
    public class GNSSSerializer : Serializer
    {
        [SerializeField]
        private GNSSFormatManager _format;

        private SentenceMsg _msg;
        private AutoHeader _header;

        public GNSSFormatManager format { get => _format; }
        public SentenceMsg msg { get => _msg; }

        public void Start()
        {
            _format.Start();
        }

        public void Init(string frame_id)
        {
            _msg = new SentenceMsg();
            _header = new AutoHeader();

            _header.Init(frame_id);
        }

        public void Update()
        {
            _format.Update();
        }

        public void Serialize(float time, GeoCoordinate coordinate, Vector3 velocity)
        {
            _header.Serialize(time);
            _msg.sentence = _format.Serialize(coordinate, velocity);
            _msg.header = _header.header;
        }
    }
}
