using DeviceSensors.Hardware.Sensors.Abstractions;
using Xamarin.Forms;

namespace DeviceSensors.Hardware.Sensors
{
    public class DeviceSensor
    {
        private IDeviceSensor instance;
        public DeviceSensor()
        {
            instance = DependencyService.Get<IDeviceSensor>();
        }

        public IDeviceSensor Instance
        {
            get { return this.instance; }
        }
            
    }
}
