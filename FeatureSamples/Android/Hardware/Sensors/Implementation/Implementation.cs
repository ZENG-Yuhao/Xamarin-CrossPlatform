using System;
using Xamarin.Forms;
using Android.Hardware;
using Android.Runtime;
using System.Collections.Generic;
using Urho.Samples.Droid.Hardware.Sensors.Implementation;
using Urho.Samples.Hardware.Sensors.Abstractions;

[assembly: Dependency(typeof(Implementation))]

namespace Urho.Samples.Droid.Hardware.Sensors.Implementation
{
    public class Implementation : Java.Lang.Object, ISensorEventListener, IDeviceSensor
    {

        private SensorManager sensorManager;
        private Sensor sensorAccelerometer;
        private Sensor sensorGravimeter;
        private Sensor sensorGyroscope;
        private Sensor sensorMagnetometer;
        private Sensor sensorOrientation;

        private IDictionary<DeviceSensorType, bool> sensorStatus;

        public event SensorValueChangedEventHandler SensorValueChanged;

        public Implementation()
        {
            sensorManager = (SensorManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.SensorService);
            sensorAccelerometer = sensorManager.GetDefaultSensor(SensorType.Accelerometer);
            sensorGravimeter = sensorManager.GetDefaultSensor(SensorType.Gravity);
            sensorGyroscope = sensorManager.GetDefaultSensor(SensorType.Gyroscope);
            sensorMagnetometer = sensorManager.GetDefaultSensor(SensorType.MagneticField);
            sensorOrientation = sensorManager.GetDefaultSensor(SensorType.Orientation);

            sensorStatus = new Dictionary<DeviceSensorType, bool>()
            {
                { DeviceSensorType.Accelerometer, false},
                { DeviceSensorType.Gravimeter, false},
                { DeviceSensorType.Gyroscope, false},
                { DeviceSensorType.Magnetometer, false},
                { DeviceSensorType.Orientation, false }

            };
        }


        public bool IsActive(DeviceSensorType sensorType)
        {
            return sensorStatus[sensorType];
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (SensorValueChanged == null)
                return;

            DeviceSensorValues sensorValues = new DeviceSensorValues();
            sensorValues.Values[0] = e.Values[0];
            sensorValues.Values[1] = e.Values[1];
            sensorValues.Values[2] = e.Values[2];
            //float[] temp = new float[3];
            //SensorManager.RemapCoordinateSystem(sensorValues.Values, Axis.X, Axis.Z, temp);
            //sensorValues.Values = temp;
            switch (e.Sensor.Type)
            {
                case SensorType.Accelerometer:
                    SensorValueChanged(this, new SensorValueChangedEventArgs(sensorValues, DeviceSensorType.Accelerometer));
                    break;
                case SensorType.Gravity:
                    SensorValueChanged(this, new SensorValueChangedEventArgs(sensorValues, DeviceSensorType.Gravimeter));
                    break;
                case SensorType.Gyroscope:
                    SensorValueChanged(this, new SensorValueChangedEventArgs(sensorValues, DeviceSensorType.Gyroscope));
                    break;
                case SensorType.MagneticField:
                    SensorValueChanged(this, new SensorValueChangedEventArgs(sensorValues, DeviceSensorType.Magnetometer));
                    break;
                case SensorType.Orientation:
                    SensorValueChanged(this, new SensorValueChangedEventArgs(sensorValues, DeviceSensorType.Orientation));
                    break;
            }
        }

        public void Start(DeviceSensorType sensorType, DeviceSensorDelay sensorDelay)
        {
            SensorDelay delay;
            switch (sensorDelay)
            {
                case DeviceSensorDelay.UI:
                    delay = SensorDelay.Ui;
                    break;
                case DeviceSensorDelay.GAME:
                    delay = SensorDelay.Game;
                    break;
                case DeviceSensorDelay.FASTEST:
                    delay = SensorDelay.Fastest;
                    break;
                default:
                    delay = SensorDelay.Normal;
                    break;
            }

            // Android will check whether there is already a sensor has been registered,
            // If you have already registered listener, Android is not gonna add the same listener twice.
            // The same is applicable for unregister logic
            switch (sensorType)
            {
                case DeviceSensorType.Accelerometer:
                    if (sensorAccelerometer != null)
                        sensorManager.RegisterListener(this, sensorAccelerometer, delay);
                    else
                        Console.WriteLine("Accelerometer is null.");
                    break;
                case DeviceSensorType.Gravimeter:
                    if (sensorGravimeter != null)
                        sensorManager.RegisterListener(this, sensorGravimeter, delay);
                    else
                        Console.WriteLine("Gravimeter is null.");
                    break;

                case DeviceSensorType.Gyroscope:
                    if (sensorGyroscope != null)
                        sensorManager.RegisterListener(this, sensorGyroscope, delay);
                    else
                        Console.WriteLine("Gyroscope is null.");
                    break;

                case DeviceSensorType.Magnetometer:
                    if (sensorMagnetometer != null)

                        sensorManager.RegisterListener(this, sensorMagnetometer, delay);
                    else
                        Console.WriteLine("Magnetometer is null.");
                    break;

                case DeviceSensorType.Orientation:
                    if (sensorOrientation != null)
                        sensorManager.RegisterListener(this, sensorOrientation, delay);
                    else
                        Console.WriteLine("Orientation not available. Accelerometer and Magnetometer must not be null.");
                    break;
            } //end case
            sensorStatus[sensorType] = true;
        }

        public void Stop(DeviceSensorType sensorType)
        {
            switch (sensorType)
            {
                case DeviceSensorType.Accelerometer:
                    if (sensorAccelerometer != null)
                        sensorManager.UnregisterListener(this, sensorAccelerometer);
                    else
                        Console.WriteLine("Accelerometer is null.");
                    break;
                case DeviceSensorType.Gravimeter:
                    if (sensorGravimeter != null)
                        sensorManager.UnregisterListener(this, sensorGravimeter);
                    else
                        Console.WriteLine("Gravimeter is null.");
                    break;

                case DeviceSensorType.Gyroscope:
                    if (sensorGyroscope != null)
                        sensorManager.UnregisterListener(this, sensorGyroscope);
                    else
                        Console.WriteLine("Gyroscope is null.");
                    break;

                case DeviceSensorType.Magnetometer:
                    if (sensorMagnetometer != null)

                        sensorManager.UnregisterListener(this, sensorMagnetometer);
                    else
                        Console.WriteLine("Magnetometer is null.");
                    break;

                case DeviceSensorType.Orientation:
                    if (sensorOrientation != null)
                        sensorManager.UnregisterListener(this, sensorOrientation);
                    else
                        Console.WriteLine("Orientation not available. Accelerometer and Magnetometer must not be null.");
                    break;
            } //end case
            sensorStatus[sensorType] = false;
        }
    }
}