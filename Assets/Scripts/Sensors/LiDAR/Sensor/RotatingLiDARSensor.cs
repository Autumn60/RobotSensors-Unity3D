using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using Unity.Burst;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;

namespace RobotSensors
{
    class RotatingLiDARSensor : LiDARSensor
    {
        [SerializeField, Range(-90.0f, 90.0f)]
        private float _minZenithAngle = -30.0f;
        [SerializeField, Range(-90.0f, 90.0f)]
        private float _maxZenithAngle = 30.0f;

        [SerializeField]
        private float _minRange = 0.1f;
        [SerializeField]
        private float _maxRange = 100.0f;

        [SerializeField, Range(1, 128)]
        private int _channelNum = 1;

        [SerializeField]
        private int _resolution = 360;

        private Transform _rotator;

        private Camera[] _cams;

        private RenderTexture[] _rts = null;
        private Texture2D[] _textures;

        private Vector2Int _textureSize;

        private TextureToPointsJob _job;
        private NativeArray<int> _pixelIndices;
        private NativeArray<Vector3> _directions;

        private Transform _transform;

        protected override void Init()
        {
            _resolution = (_resolution / 3) * 3;
            CreateSensor();
            SetupCameras();
            SetupIndicesAndDirections();
            SetupJobs();
            base.Init();
        }

        private void CreateSensor()
        {
            _transform = this.transform;

            _rotator = new GameObject().transform;
            _rotator.parent = _transform;
            _rotator.name = "Rotator";
            _rotator.localPosition = Vector3.zero;

            _cams = new Camera[3];
            for(int i = 0; i < 3; i++)
            {
                GameObject cam_obj = new GameObject();
                Transform cam_transform = cam_obj.transform;
                _cams[i] = cam_obj.AddComponent<Camera>();
                cam_transform.parent = _rotator;
                cam_transform.name = "Camera" + i.ToString();
                cam_transform.localPosition = Vector3.zero;
                cam_transform.localEulerAngles = new Vector3(0, 60 + 120*i, 0.0f);
            }
        }

        private void SetupCameras()
        { 
            float fov = Mathf.Max(Mathf.Abs(_minZenithAngle), Mathf.Abs(_maxZenithAngle)) * 4.0f;

            float resolution_y = _channelNum / (_maxZenithAngle - _minZenithAngle) * fov;
            float resolution_x = Mathf.CeilToInt(resolution_y / Mathf.Tan(fov*0.5f*Mathf.Deg2Rad) * Mathf.Tan(60.0f*Mathf.Deg2Rad));
            _textureSize.x = Mathf.CeilToInt(resolution_x);
            _textureSize.y = Mathf.CeilToInt(resolution_y);

            _rts = new RenderTexture[3];
            _rts[0] = new RenderTexture(_textureSize.x, _textureSize.y, 32, RenderTextureFormat.ARGBFloat);
            _rts[1] = new RenderTexture(_textureSize.x, _textureSize.y, 32, RenderTextureFormat.ARGBFloat);
            _rts[2] = new RenderTexture(_textureSize.x, _textureSize.y, 32, RenderTextureFormat.ARGBFloat);
            for(int i = 0; i < 3; i++)
            {
                _cams[i].targetTexture = _rts[i];
                _cams[i].fieldOfView = fov;
                _cams[i].nearClipPlane = _minRange;
                _cams[i].farClipPlane = _maxRange;
                _cams[i].gameObject.AddComponent<DepthCamera>();
                _cams[i].clearFlags = CameraClearFlags.SolidColor;
            }

            _textures = new Texture2D[3];
            for (int i = 0; i < 3; i++)
            {
                _textures[i] = new Texture2D(_textureSize.x, _textureSize.y, TextureFormat.RGBAFloat, false);
            }
        }

        private void SetupIndicesAndDirections()
        {
            int pointsNum = _channelNum * _resolution;
            float fov_2 = Mathf.Max(Mathf.Abs(_minZenithAngle), Mathf.Abs(_maxZenithAngle));

            _pixelIndices = new NativeArray<int>(pointsNum/3, Allocator.Persistent);
            _directions = new NativeArray<Vector3>(pointsNum, Allocator.Persistent);

            float radius = _textureSize.x * 0.5f / Mathf.Tan(60.0f * Mathf.Deg2Rad);
            for(int r = 0; r < _resolution / 3; r++)
            {
                for(int c = 0; c < _channelNum; c++)
                {
                    float angle_r = ((float)r / (float)_resolution) * 360.0f;
                    float angle_c = Mathf.Lerp(_minZenithAngle, _maxZenithAngle, (float)c / (float)_channelNum);
                    Vector3 dir = Quaternion.Euler(-angle_c, angle_r - 60.0f, 0) * Vector3.forward * radius;
                    dir *= (radius/dir.z);
                    
                    for (int i = 0; i < 3; i++)
                    {
                        _directions[i * _channelNum * (_resolution/3) + r * _channelNum + c] = Quaternion.Euler(-angle_c, i * 120.0f + angle_r, 0) * Vector3.forward;
                    }

                    int index_x = (int)Mathf.Clamp(_textureSize.x * 0.5f + dir.x, 0, _textureSize.x - 1);
                    int index_y = (int)Mathf.Clamp(_textureSize.y * 0.5f + dir.y, 0, _textureSize.y - 1);
                    _pixelIndices[r * _channelNum + c] = index_y * _textureSize.x + index_x;
                }
            }
        }

        private void SetupJobs()
        {
            int pointsNum = _channelNum * _resolution;
            base.points = new NativeArray<Vector3>(pointsNum, Allocator.Persistent);
            _job = new TextureToPointsJob()
            {
                far = _maxRange,
                pointsNum_3 = pointsNum/3,
                pixelIndices = _pixelIndices,
                directions = _directions,
                pixels0 = _textures[0].GetPixelData<Color>(0),
                pixels1 = _textures[1].GetPixelData<Color>(0),
                pixels2 = _textures[2].GetPixelData<Color>(0),
                points = base.points
            };
        }

        protected override void UpdateSensor()
        {
            base._handle.Complete();

            AsyncGPUReadback.Request(_rts[0], 0, request => {
                if (request.hasError)
                {
                }
                else
                {
                    if (!Application.isPlaying) return;
                    var data = request.GetData<Color>();
                    _textures[0].LoadRawTextureData(data);
                    _textures[0].Apply();
                }
            });
            AsyncGPUReadback.Request(_rts[1], 0, request => {
                if (request.hasError)
                {
                }
                else
                {
                    if (!Application.isPlaying) return;
                    var data = request.GetData<Color>();
                    _textures[1].LoadRawTextureData(data);
                    _textures[1].Apply();
                }
            });
            AsyncGPUReadback.Request(_rts[2], 0, request => {
                if (request.hasError)
                {
                }
                else
                {
                    if (!Application.isPlaying) return;
                    var data = request.GetData<Color>();
                    _textures[2].LoadRawTextureData(data);
                    _textures[2].Apply();
                }
            });

            base._handle = _job.Schedule(_resolution * _channelNum, 1);

            JobHandle.ScheduleBatchedJobs();
        }

        private void OnDestroy()
        {
            foreach(RenderTexture rt in _rts)
            {
                rt.Release();
            }
        }

        private void OnApplicationQuit()
        {
            base._handle.Complete();
            _pixelIndices.Dispose();
            _directions.Dispose();
            points.Dispose();

            foreach (RenderTexture rt in _rts)
            {
                rt.Release();
            }
        }

        protected override uint GetPointNum()
        {
            return (uint)(_resolution * _channelNum);
        }

        [BurstCompile]
        private struct TextureToPointsJob : IJobParallelFor
        {
            public float far;
            public int pointsNum_3;

            [ReadOnly]
            public NativeArray<int> pixelIndices;
            [ReadOnly]
            public NativeArray<Vector3> directions;

            [ReadOnly]
            public NativeArray<Color> pixels0;
            [ReadOnly]
            public NativeArray<Color> pixels1;
            [ReadOnly]
            public NativeArray<Color> pixels2;

            public NativeArray<Vector3> points;

            public void Execute(int index)
            {
                int pixelIndex = pixelIndices.AsReadOnly()[index%pointsNum_3];
                float distance;
                
                if(index >= pointsNum_3 * 2)
                {
                    distance = pixels2.AsReadOnly()[pixelIndex].r;
                }
                else if(index >= pointsNum_3)
                {
                    distance = pixels1.AsReadOnly()[pixelIndex].r;
                }
                else
                {
                    distance = pixels0.AsReadOnly()[pixelIndex].r;
                }
                points[index] = directions[index] * far * Mathf.Clamp01(1.0f - distance);
            }
        }
    }
}
