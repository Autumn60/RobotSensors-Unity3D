using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotSensors
{
    [System.Serializable]
    public class GNSSFormatManager : AttachableScriptableObjectManager
    {
        private GNSSFormat _format;

        public override void Start()
        {
            base.Start();
            _format = base._scriptableObject as GNSSFormat;
            if (_format) _format.Init();
            else Debug.LogError("Type of GNSSFormat does not match.");
        }

        public override void Update()
        {
            base.Update();
        }

        public string Serialize(GeoCoordinate coordinate, Vector3 velocity)
        {
            return _format.Serialize(coordinate, velocity);
        }
    }
}
