using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotSensors
{
    [System.Serializable]
    public class LiDARSerializer : Serializer
    {
        [SerializeField]
        private PointCloudFormatManager _format;

        //private SentenceMsg _msg;
        private AutoHeader _header;

        public PointCloudFormatManager format { get => _format; }
        //public SentenceMsg msg { get => _msg; }

        public void Start()
        {
            _format.Start();
        }

        public void Init(string frame_id)
        {
            //_msg = new SentenceMsg();
            _header = new AutoHeader();

            _header.Init(frame_id);
        }

        public void Update()
        {
            _format.Update();
        }

        public void Serialize(float time)
        {
            _header.Serialize(time);
            //_msg.sentence = _format.Serialize(coordinate, velocity);
            //_msg.header = _header.header;
        }
    }
}

