using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotSensors
{
    public class ScanPatternVisualizer : Visualizer<Transform>
    {
        private enum Mode
        {
            LASER,
            POINT
        }

        [SerializeField]
        private ScanPattern _scanPattern;

        [SerializeField]
        private Mode _mode;

        [SerializeField]
        private float _range = 1.0f;
        
        [SerializeField]
        private int _drawNumPerVisualize = 1;

        [SerializeField]
        private float _duration = 0.1f;

        private int _counter = 0;

        protected override void Update()
        {
            base.Update();
            if (_visualizeMode_edit != Visualizer<Transform>.VisualizeMode.NONE) _visualizeMode_edit = Visualizer<Transform>.VisualizeMode.NONE;
        }

        protected override void Visualize()
        {
            if (!_scanPattern) return;
            if (!_scanPattern.loaded) return;

            int counter_old = (_counter==0 ? _scanPattern.size - 1 : _counter - 1);
            for(int i = 0; i < _drawNumPerVisualize; i++)
            {
                Vector3 start = (_mode == Mode.LASER ? (_target.position) : (_target.position + _target.TransformDirection(_scanPattern.scans[counter_old] * _range)));
                Debug.DrawLine(start, _target.position + _target.TransformDirection(_scanPattern.scans[_counter]) * _range, _defaultColor, _duration);
                counter_old = _counter;
                _counter++;
                if (_counter >= _scanPattern.size) _counter = 0;
            }
        }
    }
}
