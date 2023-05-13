using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotSensors
{
    public class PointCloudFormat : AttachableScriptableObject
    {
        public virtual void Init()
        {

        }

        public virtual string Serialize()
        {
            return "";
        }
    }
}
