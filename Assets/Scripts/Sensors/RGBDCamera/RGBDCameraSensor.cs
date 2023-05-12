using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace RobotSensors
{
    [RequireComponent(typeof(Camera))]
    public class RGBDCameraSensor : Sensor
    {
        [SerializeField]
        private Vector2Int _resolution = new Vector2Int(640, 480);

        [SerializeField]
        private Shader _depthShader;

        [SerializeField, Range(0, 100)]
        private int _quality = 100;

        private Camera _cam;

        private Material _material;

        private RenderTexture _rt = null;
        private Texture2D _texture;

        private Vector3[] _points;

        public Texture2D texture { get => _texture; }
        public Vector3[] points { get => _points; }

        public int quality { get => _quality; }

        protected override void Init()
        {
            _cam = GetComponent<Camera>();
            _material = new Material(_depthShader);
            _rt = new RenderTexture(_resolution.x, _resolution.y, 32, RenderTextureFormat.ARGB32);
            _texture = new Texture2D(_resolution.x, _resolution.y, TextureFormat.RGBA32, false);
            _points = new Vector3[_resolution.x * _resolution.y];

            _cam.clearFlags = CameraClearFlags.SolidColor;
            _cam.targetTexture = _rt;

            float n = _cam.nearClipPlane;
            float f = _cam.farClipPlane;
            float n_inv = 1.0f / n;
            float f_inv = 1.0f / f;
            float n_f = n_inv - f_inv;

            float vDisW = _resolution.x * 0.5f / Mathf.Tan(_cam.fieldOfView * _cam.aspect * 0.5f * Mathf.Deg2Rad);
            float vDisH = _resolution.y * 0.5f / Mathf.Tan(_cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

            _material.SetFloat("_N", n);
            _material.SetFloat("_F", f);
            _material.SetFloat("_F_N", f-n);
        }

        protected override void UpdateSensor()
        {
            UpdateTexture();
        }

        private void UpdateTexture()
        {
            AsyncGPUReadback.Request(_rt, 0, request => {
                if (request.hasError)
                {
                }
                else
                {
                    if (!Application.isPlaying) return;
                    var data = request.GetData<Color32>();
                    _texture.LoadRawTextureData(data);
                    _texture.Apply();
                }
            });
        }

        private void OnDestroy()
        {
            _rt.Release();
        }

        private void OnApplicationQuit()
        {
            _rt.Release();
        }

        private void OnRenderImage(RenderTexture source, RenderTexture dest)
        {
            Graphics.Blit(source, dest, _material);
        }
    }
}
