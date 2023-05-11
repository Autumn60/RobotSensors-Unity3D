using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RobotSensors
{
    [RequireComponent(typeof(RGBCameraSensor))]
    public class RGBCameraVisualizer : Visualizer<RGBCameraSensor>
    {
        [SerializeField]
        private RawImage _image;

        protected override void Update()
        {
            base.Update();
            base._visualizeMode_edit = Visualizer<RGBCameraSensor>.VisualizeMode.NONE;
            base._visualizeMode_play = Visualizer<RGBCameraSensor>.VisualizeMode.NONE;
            Visualize();
        }

        protected override void Visualize()
        {
            if (!_image || !_target.texture) return;
            _image.texture = _target.texture;
        }
    }
}
