using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace RobotSensors
{
    [RequireComponent(typeof(Camera))]
    public class DepthCameraSensor : Sensor
    {
        [SerializeField]
        private Vector2Int _resolution = new Vector2Int(640, 480);

        [SerializeField, Range(0, 100)]
        private int _quality = 100;

        private Camera _cam;

        private RenderTexture _rt = null;
        private Texture2D _texture;

        public Texture2D texture { get => _texture; }

        public int quality { get => _quality; }

        protected override void Init()
        {
            _cam = GetComponent<Camera>();
            _rt = new RenderTexture(_resolution.x, _resolution.y, 32, RenderTextureFormat.ARGB32);
            _texture = new Texture2D(_resolution.x, _resolution.y, TextureFormat.RGBA32, false);

            _cam.clearFlags = CameraClearFlags.SolidColor;
            _cam.targetTexture = _rt;

            if (!GetComponent<DepthCamera>())
            {
                gameObject.AddComponent<DepthCamera>();
            }
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
    }
}
