using DeviceSensors.Interface;
using DeviceSensors.WinPhone;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(PhoneOrientation))]
namespace DeviceSensors.WinPhone
{
    public class PhoneOrientation : IDeviceOrientation
    {
        public float[] getOrientation()
        {
            throw new NotImplementedException();
        }

        public float[] getOrientation(float[] rotationMatrix)
        {
            throw new NotImplementedException();
        }

        public float[] getRotationMatrix()
        {
            throw new NotImplementedException();
        }

        public void getRotationMatrix(float[] R, float[] I, float[] elem1, float[] elem2)
        {
            throw new NotImplementedException();
        }

        public IDeviceOrientation setAccelerometerParam(float[] values)
        {
            throw new NotImplementedException();
        }

        public IDeviceOrientation setAccelerometerParam(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        public IDeviceOrientation setMagnetometerParam(float[] values)
        {
            throw new NotImplementedException();
        }

        public IDeviceOrientation setMagnetometerParam(float x, float y, float z)
        {
            throw new NotImplementedException();
        }
    }
}
