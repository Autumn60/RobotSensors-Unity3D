using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotSensors
{
    [RequireComponent(typeof(IMUSensor))]
    public class IMUPublisher : Publisher<IMUSensor>
    {
        
    }
}
