using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Burst;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;

namespace RobotSensors
{
    public class LiDARSensor : Sensor
    {
        protected JobHandle _handle;

        public NativeArray<Vector3> points;

        public uint pointNum { get => GetPointNum(); }

        protected override void Init()
        {
            base.Init();
        }

        protected override void UpdateSensor()
        {
        }

        protected virtual uint GetPointNum()
        {
            return 0;
        }

        public void CompleteJob()
        {
            _handle.Complete();
        }
    }
}
