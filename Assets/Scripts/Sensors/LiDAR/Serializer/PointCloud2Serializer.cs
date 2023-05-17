using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Burst;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;

using RosMessageTypes.Sensor;

namespace RobotSensors
{
    [System.Serializable]
    class PointCloud2Serializer : LiDARSerializer<PointCloud2Msg>
    {
        private JobHandle _handle;
        private PointsToPointCloud2MsgJob _job;

        private NativeArray<byte> _data;

        public override void Init(string frame_id, ref NativeArray<Vector3> points, uint pointNum)
        {
            base.Init(frame_id, ref points, pointNum);

            _msg = new PointCloud2Msg();
            _msg.height = 1;
            _msg.width = pointNum;
            _msg.fields = new PointFieldMsg[3];
            for(int i = 0; i < 3; i++)
            {
                _msg.fields[i] = new PointFieldMsg();
                _msg.fields[i].name = ((char)('x' + i)).ToString();
                _msg.fields[i].offset = (uint)(4 * i);
                _msg.fields[i].datatype = 7;
                _msg.fields[i].count = 1;
            }
            _msg.is_bigendian = false;
            _msg.point_step = 12;
            _msg.row_step = pointNum * 12;
            _msg.data = new byte[pointNum * 12];
            _msg.is_dense = true;

            _data = new NativeArray<byte>((int)pointNum * 12, Allocator.Persistent);

            _job = new PointsToPointCloud2MsgJob
            {
                pointNum = (int)pointNum,
                points = points,
                data = _data
            };
        }

        public override void Serialize(float time)
        {
            base.Serialize(time);
            _msg.header = _header;
            _handle = _job.Schedule(_pointNum, 1);

            JobHandle.ScheduleBatchedJobs();
            _handle.Complete();

            _msg.data = _data.ToArray();
        }

        public override void Dispose()
        {
            _handle.Complete();
            _data.Dispose();
        }

        [BurstCompile]
        public struct PointsToPointCloud2MsgJob : IJobParallelFor
        {
            public int pointNum;

            [ReadOnly]
            public NativeArray<Vector3> points;

            public NativeArray<byte> data;

            public void Execute(int index)
            {
                NativeArray<float> tmp = new NativeArray<float>(3, Allocator.Temp);
                tmp[0] = points[index].x;
                tmp[1] = points[index].z;
                tmp[2] = points[index].y;
                var slice = new NativeSlice<float>(tmp).SliceConvert<byte>();
                slice.CopyTo(data.GetSubArray(index * 12, 12));
                
            }
        }
    }
}
