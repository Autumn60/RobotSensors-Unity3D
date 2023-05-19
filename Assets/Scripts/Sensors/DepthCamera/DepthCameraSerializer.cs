using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RosMessageTypes.Sensor;

namespace RobotSensors
{
    public class DepthCameraSerializer : Serializer
    {
        private CompressedImageMsg _msg;

        private AutoHeader _header;

        public CompressedImageMsg msg { get => _msg; }

        public void Init(string frame_id)
        {
            _msg = new CompressedImageMsg();
            _header = new AutoHeader();

            _msg.format = "jpeg";
            _header.Init(frame_id);
        }

        public void Serialize(float time, Texture2D texture, int quality)
        {
            _header.Serialize(time);
            _msg.header = _header.header;

            _msg.data = texture.EncodeToJPG(quality);
        }
    }
}
