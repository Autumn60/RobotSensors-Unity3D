using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Burst;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;

using RosMessageTypes.Std;
using RosMessageTypes.Sensor;

namespace RobotSensors
{
    [System.Serializable]
    public class LiDARSerializer<T> : Serializer
    {
        private AutoHeader _autoHeader;

        protected T _msg;

        protected int _pointNum;

        public T msg { get => _msg; }

        protected HeaderMsg _header { get => _autoHeader.header; }

        public void Start()
        {
        }

        public virtual void Init(string frame_id, ref NativeArray<Vector3> points, uint pointNum)
        {
            _autoHeader = new AutoHeader();
            _autoHeader.Init(frame_id);

            _pointNum = (int)pointNum;
        }

        public void Update()
        {
        }

        public virtual void Dispose()
        {
        
        }

        public virtual void Serialize(float time)
        {
            _autoHeader.Serialize(time);
        }
    }
}

