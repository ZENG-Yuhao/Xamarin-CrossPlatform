using DeviceSensors.Interface;
using DeviceSensors.WinPhone;
using Microsoft.Devices.Sensors;
using System;
using System.Windows;
using System.Windows.Threading;

[assembly: Xamarin.Forms.Dependency(typeof(PhoneOrientation))]
namespace DeviceSensors.WinPhone
{

    public class PhoneOrientation : IDeviceOrientation
    {
        Motion motion;
        float[] values;
        Dispatcher dispatcher;
        public PhoneOrientation()
        {
            values = new float[3];
            motion = new Motion();
            motion.CurrentValueChanged +=
            new EventHandler<SensorReadingEventArgs<MotionReading>>(motion_CurrentValueChanged);

            // Try to start the Motion API.
            try
            {
                motion.Start();
            } catch (Exception e)
            {
                MessageBox.Show("unable to start the Motion API.");
            }
        }

        public void motion_CurrentValueChanged(object sender, SensorReadingEventArgs<MotionReading> e)
        {
            if (motion.IsDataValid)
            {
                values[0] = e.SensorReading.Attitude.Yaw;
                values[1] = e.SensorReading.Attitude.Pitch;
                values[2] = e.SensorReading.Attitude.Roll;
            }
        }

        public void CurrentValueChanged(MotionReading e)
        {
            // Check to see if the Motion data is valid.
   
        }

        public float[] getOrientation()
        {
            return values;
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
            return this;
        }

        public IDeviceOrientation setAccelerometerParam(float x, float y, float z)
        {
            return this;
        }

        public IDeviceOrientation setMagnetometerParam(float[] values)
        {
            return this;
        }

        public IDeviceOrientation setMagnetometerParam(float x, float y, float z)
        {
            return this;
        }
    }
}
