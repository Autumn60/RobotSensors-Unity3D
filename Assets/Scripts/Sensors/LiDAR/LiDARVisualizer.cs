using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotSensors
{
    [RequireComponent(typeof(LiDARSensor))]
    public class LiDARVisualizer : Visualizer<LiDARSensor>
    {
        protected override void Visualize()
        {
        }
    }
}
