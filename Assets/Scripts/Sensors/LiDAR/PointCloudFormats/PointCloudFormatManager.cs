using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotSensors
{
    [System.Serializable]
    public class PointCloudFormatManager : AttachableScriptableObjectManager
    {
        private PointCloudFormat _format;

        public override void Start()
        {
            base.Start();
            _format = base._scriptableObject as PointCloudFormat;
            if (_format) _format.Init();
            else Debug.LogError("Type of PointCloudFormat does not match.");
        }

        public override void Update()
        {
            base.Update();
        }

        public void Serialize()
        {
            _format.Serialize();
        }
    }
}
