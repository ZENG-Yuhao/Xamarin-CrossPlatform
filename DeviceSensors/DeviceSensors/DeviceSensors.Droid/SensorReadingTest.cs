using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using DeviceSensors.Interface;
using Xamarin.Forms;
using DeviceSensors.Droid;
using Android.Hardware;

[assembly: Dependency(typeof(SensorReadingTest))]

namespace DeviceSensors.Droid
{
    public class SensorReadingTest : Java.Lang.Object, ISensorEventListener, IDeviceSensorTest
    {
        private SensorManager sensorManager;
        private Sensor sensorAccelerometer;
        private Sensor sensorMagnetometer;
        private Sensor sensorGravity;
        private Sensor sensorGyroscope;
        private IDictionary<SensorType, bool> sensorStatus;

        public float[] gravity;
        public float[] magnetic;

        public event SensorValueChangedEventHandler SensorValueChanged;

        public SensorReadingTest()
        {
            sensorManager = (SensorManager)Android.App.Application.Context.GetSystemService(Context.SensorService);
            sensorAccelerometer = sensorManager.GetDefaultSensor(SensorType.Accelerometer);
            sensorMagnetometer = sensorManager.GetDefaultSensor(SensorType.MagneticField);
            sensorGravity = sensorManager.GetDefaultSensor(SensorType.Gravity);
            sensorGyroscope = sensorManager.GetDefaultSensor(SensorType.Gyroscope);
            sensorStatus = new Dictionary<SensorType, bool>()
            {
                { SensorType.Accelerometer, false},
                { SensorType.MagneticField, false},
                { SensorType.Gravity, false},
                { SensorType.Gyroscope, false}
            };

        }

        public float[] getDeviceOrientation()
        {
            throw new NotImplementedException();
        }

        public void Start(string delayType)
        {


            SensorDelay delay;
            switch (delayType)
            {
                case "Normal":
                    delay = SensorDelay.Normal;
                    break;
                case "Ui":
                    delay = SensorDelay.Ui;
                    break;
                case "Game":
                    delay = SensorDelay.Game;
                    break;
                case "Fastest":
                    delay = SensorDelay.Fastest;
                    break;
                default:
                    delay = SensorDelay.Normal;
                    break;

            }

            if (sensorGravity != null)
                sensorManager.RegisterListener(this, sensorGravity, delay);
            else
                Console.WriteLine("Gravity not available.");

            if (sensorMagnetometer != null)
                sensorManager.RegisterListener(this, sensorMagnetometer, delay);
            else
                Console.WriteLine("Magnetometer not available.");

        }

        public void Stop()
        {
            if (sensorGravity != null)
                sensorManager.UnregisterListener(this, sensorGravity);
            if (sensorMagnetometer != null)
                sensorManager.UnregisterListener(this, sensorMagnetometer);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (SensorValueChanged == null) return;

            float[] values = new float[3];
            values[0] = e.Values[0];
            values[1] = e.Values[1];
            values[2] = e.Values[2];
            
            switch (e.Sensor.Type)
            {
                case SensorType.Gravity:
                    gravity = values;
                    filterLowPass(values, gravity, 0.8f);
                    Console.WriteLine("Accelerometer"+values[0]);
                    break;
                case SensorType.MagneticField:
                    magnetic = values;
                    Console.WriteLine("Magnetometer" + values[0]);
                    if (gravity != null && magnetic != null)
                    {
                        float[] R = new float[9];
                        float[] I = new float[9];
                        if (SensorManager.GetRotationMatrix(R, I, gravity, magnetic))
                        {
                            float[] orientation = new float[3];
                            SensorManager.GetOrientation(R, orientation);
                            SensorValueChanged(this, orientation, "Orientation");
                        }
                    }
                    break;
            }
        }

        public void ClearEvents()
        {
            throw new NotImplementedException();
        }

        public void filterLowPass(float[] arrin, float[] arrout, float alpha)
        {
            int len = arrin.Length;
            //Debug.WriteLine("arrin.length={0}, sizeof float={1}, len={2}", arrin.Length, sizeof(float), len);
            for (int i = 0; i < len; i++)
            {
                arrout[i] = alpha * arrout[i] + (1 - alpha) * arrin[i];
            }
        }
    }
}