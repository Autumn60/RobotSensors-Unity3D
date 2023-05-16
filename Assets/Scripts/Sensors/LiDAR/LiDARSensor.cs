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

        protected NativeArray<Vector3> _points;
        public NativeArray<Vector3>.ReadOnly points { get => GetPoints(); }

        protected override void Init()
        {
        }

        protected override void UpdateSensor()
        {
        }

        protected virtual NativeArray<Vector3>.ReadOnly GetPoints()
        {
            _handle.Complete();
            return _points.AsReadOnly();
        }
    }
}
