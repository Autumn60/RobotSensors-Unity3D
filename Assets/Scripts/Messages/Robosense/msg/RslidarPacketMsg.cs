//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.BuiltinInterfaces;

namespace RosMessageTypes.Robosense
{
    [Serializable]
    public class RslidarPacketMsg : Message
    {
        public const string k_RosMessageName = "Robosense/rslidarPacket";
        public override string RosMessageName => k_RosMessageName;

        //  Raw LIDAR packet.
        public TimeMsg stamp;
        //  packet timestamp
        public byte[] data;
        //  packet contents

        public RslidarPacketMsg()
        {
            this.stamp = new TimeMsg();
            this.data = new byte[1248];
        }

        public RslidarPacketMsg(TimeMsg stamp, byte[] data)
        {
            this.stamp = stamp;
            this.data = data;
        }

        public static RslidarPacketMsg Deserialize(MessageDeserializer deserializer) => new RslidarPacketMsg(deserializer);

        private RslidarPacketMsg(MessageDeserializer deserializer)
        {
            this.stamp = TimeMsg.Deserialize(deserializer);
            deserializer.Read(out this.data, sizeof(byte), 1248);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.stamp);
            serializer.Write(this.data);
        }

        public override string ToString()
        {
            return "RslidarPacketMsg: " +
            "\nstamp: " + stamp.ToString() +
            "\ndata: " + System.String.Join(", ", data.ToList());
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize);
        }
    }
}
