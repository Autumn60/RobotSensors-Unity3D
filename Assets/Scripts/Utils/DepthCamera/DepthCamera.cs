using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotSensors
{
    [RequireComponent(typeof(Camera))]
    public class DepthCamera : MonoBehaviour
    {
        private Material _mat;

        private void Awake()
        {
            _mat = new Material(Shader.Find("Depth"));

            Camera cam = GetComponent<Camera>();

            float f = cam.farClipPlane;

            _mat.SetFloat("_F", f);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture dest)
        {
            Graphics.Blit(source, dest, _mat);
        }
    }
}
