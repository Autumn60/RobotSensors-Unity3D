using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotSensors
{
    [CreateAssetMenu]
    public class GPRMC : GNSSFormat
    {
        [SerializeField]
        private TimeMode _timeMode;

        [SerializeField]
        private GNSSMode _mode;

        [SerializeField, Tooltip("False : Available, True : Warning")]
        private bool _status;

        private const float _meterPerSec2knot = 1.9384f;

        public override string Serialize(GeoCoordinate coordinate, Vector3 velocity)
        {
            velocity.y = 0.0f;
            float groundSpeed = velocity.magnitude * _meterPerSec2knot;
            float directionOfMovement = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
            if (directionOfMovement < 0) directionOfMovement += 360.0f;

            string sentence = "$GPRMC,";

            switch (_timeMode)
            {
                case TimeMode.SIM:
                    base.AddSimTime(ref sentence);
                    break;
                case TimeMode.UTC:
                    base.AddUTCTime(ref sentence);
                    break;
            };

            sentence += _status ? "V" : "A";
            sentence += ",";

            base.AddLatitude(ref sentence, (float) coordinate.latitude);
            base.AddLongitude(ref sentence, (float) coordinate.longitude);

            sentence += groundSpeed.ToString("000.0");
            sentence += ",";

            sentence += directionOfMovement.ToString("000.0");
            sentence += ",";

            AddUTCDate(ref sentence);
            
            sentence += ",,";

            switch (_mode)
            {
                case GNSSMode.NONE:
                    sentence += "N";
                    break;
                case GNSSMode.AUTONOMOUS:
                    sentence += "A";
                    break;
                case GNSSMode.DIFFERENTIAL:
                    sentence += "D";
                    break;
                case GNSSMode.ESTIMATED:
                    sentence += "E";
                    break;
            }

            base.AddChecksum(ref sentence);

            sentence += "\r\n";

            return sentence;
        }

        private void AddUTCDate(ref string sentence)
        {
            sentence += DateTime.UtcNow.Day.ToString("D02");
            sentence += DateTime.UtcNow.Month.ToString("D02");
            sentence += ((int)(DateTime.UtcNow.Year - (int)(DateTime.UtcNow.Year * 0.01) * 100)).ToString("D02");
            sentence += ",";
        }
    }
}
