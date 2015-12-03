using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSensors.Interface
{
    public delegate void SensorValueChangedEventHandler(object sender, float[] values, string sensorType);

    public interface IDeviceSensorTest : IDisposable
    {      
        event SensorValueChangedEventHandler SensorValueChanged;
        float[] getDeviceOrientation();
        void Start(string delay);
        void Stop();
        void ClearEvents();
        
    }
}
