using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotSensors
{
    [ExecuteAlways]
    public class Visualizer<T> : MonoBehaviour
    {
        protected enum VisualizeMode
        {
            NONE,
            SELECTED,
            ALWAYS
        }

        [SerializeField]
        protected VisualizeMode _visualizeMode_edit = VisualizeMode.SELECTED;
        [SerializeField]
        private VisualizeMode _visualizeMode_play = VisualizeMode.ALWAYS;
        [SerializeField]
        private Color _defaultColor = Color.white;

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