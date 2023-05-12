using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RobotSensors
{
    [RequireComponent(typeof(DepthCameraSensor))]
    public class DepthCameraVisualizer : Visualizer<DepthCameraSensor>
    {
        [SerializeField]
        private RawImage _output;

        protected override void Update()
        {
            base.Update();
            VisualizeTexture();
        }

        protected override void Visualize()
        {
        }

        private void VisualizeTexture()
        {
            if (!_output || !_target.texture) return;
            _output.texture = _target.texture;
        }
    }
}
