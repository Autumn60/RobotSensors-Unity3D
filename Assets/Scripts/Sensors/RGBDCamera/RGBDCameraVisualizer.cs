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
            base._visualizeMode_edit = Visualizer<RGBDCameraSensor>.VisualizeMode.NONE;
            base._visualizeMode_play = Visualizer<RGBDCameraSensor>.VisualizeMode.NONE;
            Visualize();
        }

        protected override void Visualize()
        {
            if (!_image || !_target.texture) return;
            _image.texture = _target.texture;
        }
    }
}
