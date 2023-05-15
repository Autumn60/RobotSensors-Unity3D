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

            float n = cam.nearClipPlane;
            float f = cam.farClipPlane;

            _mat.SetFloat("_N", n);
            _mat.SetFloat("_F", f);
            _mat.SetFloat("_F_N", f - n);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture dest)
        {
            Graphics.Blit(source, dest, _mat);
        }
    }
}
