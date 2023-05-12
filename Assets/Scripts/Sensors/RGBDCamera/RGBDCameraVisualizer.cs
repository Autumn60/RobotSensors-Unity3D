using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RobotSensors
{
    [RequireComponent(typeof(RGBDCameraSensor))]
    public class RGBDCameraVisualizer : Visualizer<RGBDCameraSensor>
    {
        [SerializeField]
        private RawImage _image;

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
            if (!_image || !_target.texture) return;
            _image.texture = _target.texture;
        }
    }
}
