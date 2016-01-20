
using System;

namespace DeviceSensors.Hardware.Sensors.Abstractions
{
    public class DeviceSensorValues
    {
        //public float[] values = new float[3];
        public float[] Values;

		public DeviceSensorValues(){
			Values = new float[3];
		}

		public DeviceSensorValues(float x, float y, float z){
			Values = new float[3];
			Values [0] = x;
			Values [1] = y;
			Values [2] = z;
		}

		public DeviceSensorValues getCopy(){
			return new DeviceSensorValues (this.Values [0], this.Values [1], this.Values [2]);
		}

    }
}
