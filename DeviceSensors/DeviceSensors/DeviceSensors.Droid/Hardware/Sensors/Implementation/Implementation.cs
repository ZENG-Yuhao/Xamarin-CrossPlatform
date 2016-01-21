
using System;
using Xamarin.Forms;
using Android.Hardware;
using Android.Runtime;
using System.Collections.Generic;
using DeviceSensors.Droid.Hardware.Sensors.Implementation;
using DeviceSensors.Hardware.Sensors.Abstractions;
using Android.Content;

[assembly: Dependency(typeof(Implementation))]

namespace DeviceSensors.Droid.Hardware.Sensors.Implementation
{
    public class Implementation : Java.Lang.Object, ISensorEventListener, IDeviceSensor
    {

        private SensorManager sensorManager;
        private Sensor sensorAccelerometer;
        private Sensor sensorGravimeter;
        private Sensor sensorGyroscope;
        private Sensor sensorMagnetometer;
        private Sensor sensorOrientation;
		private bool isOrientationRawActivated;
		private DeviceSensorValues gravity = new DeviceSensorValues();
		private DeviceSensorValues magnetic = new DeviceSensorValues();

        private IDictionary<DeviceSensorType, bool> sensorStatus;

        public event SensorValueChangedEventHandler SensorValueChanged;


        public Implementation()
        {
            sensorManager = (SensorManager)Android.App.Application.Context.GetSystemService(Context.SensorService);
            sensorAccelerometer = sensorManager.GetDefaultSensor(SensorType.Accelerometer);
            sensorGravimeter = sensorManager.GetDefaultSensor(SensorType.Gravity);
            sensorGyroscope = sensorManager.GetDefaultSensor(SensorType.Gyroscope);
            sensorMagnetometer = sensorManager.GetDefaultSensor(SensorType.MagneticField);
            sensorOrientation = sensorManager.GetDefaultSensor(SensorType.Orientation);

            //Instead of using raw data from the orientation sensor, we recommend that you use the getRotationMatrix() method in conjunction with the getOrientation() method to compute orientation values.You can also use the remapCoordinateSystem() method to translate the orientation values to your application's frame of reference.
            sensorStatus = new Dictionary<DeviceSensorType, bool>()
            {
                { DeviceSensorType.Accelerometer, false},
                { DeviceSensorType.Gravimeter, false},
                { DeviceSensorType.Gyroscope, false},
                { DeviceSensorType.Magnetometer, false},
                { DeviceSensorType.Orientation, false },
				{ DeviceSensorType.OrientationRaw, false}
            };

			isOrientationRawActivated = false;
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

			DeviceSensorValues sensorValues = new DeviceSensorValues(e.Values[0], e.Values[1], e.Values[2]);

            switch (e.Sensor.Type)
            {
                case SensorType.Accelerometer:
                    SensorValueChanged(this, new SensorValueChangedEventArgs(sensorValues, DeviceSensorType.Accelerometer));
                    break;
				case SensorType.Gravity:
                    if (isOrientationRawActivated)
                    {
                        //gravity = sensorValues.getCopy ();
                        filterLowPass(sensorValues.Values, gravity.Values, 0.8f);
                    }
                    SensorValueChanged(this, new SensorValueChangedEventArgs(sensorValues, DeviceSensorType.Gravimeter));
                    break;
                case SensorType.Gyroscope:
                    SensorValueChanged(this, new SensorValueChangedEventArgs(sensorValues, DeviceSensorType.Gyroscope));
                    break;
				case SensorType.MagneticField:
					if (isOrientationRawActivated) {
						magnetic = sensorValues.getCopy ();
						if (gravity != null) {
							float[] R1 = new float[9];
							float[] R = new float[9];
							float[] I = new float[9];
							if (SensorManager.GetRotationMatrix(R1, I, gravity.Values, magnetic.Values))
							{
                                //remap y axis to z axis
                                SensorManager.RemapCoordinateSystem(R1, Axis.X, Axis.Z, R);
                                //R = R1;
								DeviceSensorValues orienValues = new DeviceSensorValues ();
								SensorManager.GetOrientation(R, orienValues.Values);
								SensorValueChanged(this, new SensorValueChangedEventArgs(orienValues, DeviceSensorType.OrientationRaw));
							}
						}
					}
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
                        Console.WriteLine("Orientation not available.");
                    break;
				case DeviceSensorType.OrientationRaw:
					if (sensorGravimeter != null && sensorMagnetometer != null) {
						sensorManager.RegisterListener (this, sensorGravimeter, delay);
						sensorManager.RegisterListener (this, sensorMagnetometer, delay);
						isOrientationRawActivated = true;
					} else {
							Console.WriteLine ("OrientationRaw not available. Accelerometer and Magnetometer must not be null.");
					}
					
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
                        Console.WriteLine("Orientation not available.");
                    break;
				case DeviceSensorType.OrientationRaw:
					if (sensorGravimeter != null && sensorMagnetometer != null) {
						sensorManager.UnregisterListener(this, sensorGravimeter);
						sensorManager.UnregisterListener(this, sensorMagnetometer);
						isOrientationRawActivated = false;
					} else
						Console.WriteLine ("OrientationRaw not available. Accelerometer and Magnetometer must not be null.");
					break;
            } //end case
            sensorStatus[sensorType] = false;
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

    }//end class
} //end namespace