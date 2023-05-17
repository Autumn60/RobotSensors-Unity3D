using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotSensors
{
    [ExecuteAlways]
    public class Visualizer<T> : MonoBehaviour
    {
        [System.Serializable]
        protected class SphereSetting
        {
            public Color color = Color.white;
            public float radius = 0.5f;
        }

        [System.Serializable]
        protected class LineSetting
        {
            public Color color = Color.white;
            public bool fixLineLength = true;
            public float lineLengthFactor = 1.0f;
        }

        protected enum VisualizeMode
        {
            NONE,
            SELECTED,
            ALWAYS
        }

        [SerializeField]
        protected VisualizeMode _visualizeMode_edit = VisualizeMode.SELECTED;
        [SerializeField]
        protected VisualizeMode _visualizeMode_play = VisualizeMode.ALWAYS;
        [SerializeField]
        protected Color _defaultColor = Color.white;

        protected T _target;

        protected virtual void Update()
        {
            if (_target != null) return;
            _target = GetComponent<T>();
        }

        private void OnDrawGizmosSelected()
        {
            if ((Application.isPlaying ? _visualizeMode_play : _visualizeMode_edit) != VisualizeMode.SELECTED) return;
            Gizmos.color = _defaultColor;
            Visualize();
        }

        private void OnDrawGizmos()
        {
            if ((Application.isPlaying ? _visualizeMode_play : _visualizeMode_edit) != VisualizeMode.ALWAYS) return;
            Gizmos.color = _defaultColor;
            Visualize();
        }

        protected virtual void Visualize()
        {
        }
    }
}