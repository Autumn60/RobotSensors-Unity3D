using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes.Sensor;

namespace RobotSensors
{
    [System.Serializable]
    public class LiDARSerializer : Serializer
    {
        private AutoHeader _header;

        public void Start()
        {
        }

        public void Init(string frame_id)
        {
            _header = new AutoHeader();
            _header.Init(frame_id);
        }

        public void Update()
        {
        }

        public void Serialize(float time)
        {
            _header.Serialize(time);
        }
    }
}

