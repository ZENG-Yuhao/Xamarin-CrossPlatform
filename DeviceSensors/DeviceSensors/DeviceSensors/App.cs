using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using DeviceSensors.Hardware.Sensors.Abstraction;
using DeviceSensors.Interface;
using DeviceSensors.View;
using DeviceSensors.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;                          
using System.Text;

using Xamarin.Forms;

namespace DeviceSensors
{
    public class App : Application
    {
        private float[] gravity = new float[3];
        private float[] orientation = new float[3];
        private float[] degree = new float[3];
        private IDeviceOrientation orienGetter = DependencyService.Get<IDeviceOrientation>();

        public App()
        {
            MainPage = new MainContentPage();
        }

        protected override void OnStart()
        {
            //sensorRegistration();
            //sensorRT();
            func();
        }

        private void func()
        {
            IDeviceSensor instance = DependencyService.Get<IDeviceSensor>();
            instance.Start(DeviceSensorType.Orientation, DeviceSensorDelay.FASTEST);
            instance.SensorValueChanged += (s, e) =>
              {
                  float[] values = e.SensorValues.Values;
                  filterLowPass(values, orientation, 0.95f);

                  degree = orientation;

                  //give a text title to current direction.
                  string str = directionEstimate(degree[0]);


                  MainPage_ViewModel viewmodel = MainPage.FindByName<MainPage_ViewModel>("mainpageVM");
                  viewmodel.TextContent = str;
                  viewmodel.ValueX = degree[0];
                  viewmodel.ValueY = degree[1];
                  viewmodel.ValueZ = degree[2];
                  viewmodel.Rotation = -degree[0];
                  viewmodel.RotationX = -degree[1];
                  viewmodel.RotationY = degree[2];
                  
              };
        }

        private void sensorRT()
        {
            IDeviceSensorTest instance = DependencyService.Get<IDeviceSensorTest>();
            instance.Start("Fastest");
            instance.SensorValueChanged += (sender, values, type) =>
              {
                  //Debug.WriteLine("############## value changed");
                  filterLowPass(values, orientation, 0.95f);

                  for (int i = 0; i < 3; i++)
                      degree[i] = (float)(orientation[i] * 180 / Math.PI);

                  //give a text title to current direction.
                  string str = "This is the new version.";


                  MainPage_ViewModel exemp1 = MainPage.FindByName<MainPage_ViewModel>("mainpageVM");
                  exemp1.TextContent = str;
                  exemp1.ValueX = degree[0];
                  exemp1.ValueY = degree[1];
                  exemp1.ValueZ = degree[2];
                  exemp1.Rotation = -degree[0];
                  exemp1.RotationX = -degree[1];
                  exemp1.RotationY = -degree[2];
              };
        }

        private void sensorRegistration()
        {
            CrossDeviceMotion.Current.Start(MotionSensorType.Accelerometer, MotionSensorDelay.Game);
            CrossDeviceMotion.Current.Start(MotionSensorType.Magnetometer, MotionSensorDelay.Game);
            CrossDeviceMotion.Current.SensorValueChanged += (s, a) =>
            {
                float[] values = new float[3];
                switch (a.SensorType)
                {
                    case MotionSensorType.Accelerometer:
                        //x = ((MotionVector)a.Value).X;
                        // y = ((MotionVector)a.Value).Y;
                        // z = ((MotionVector)a.Value).Z;
                        // gravity[0] = alpha * gravity[0] + (1 - alpha) * (float)x;
                        // gravity[1] = alpha * gravity[1] + (1 - alpha) * (float)y;
                        // gravity[2] = alpha * gravity[2] + (1 - alpha) * (float)z;

                        values[0] = (float)((MotionVector)a.Value).X;
                        values[1] = (float)((MotionVector)a.Value).Y;
                        values[2] = (float)((MotionVector)a.Value).Z;
                        filterLowPass(values, gravity, 0.8f);
                        //orienGetter.setAccelerometerParam(gravity);
                        orienGetter.setAccelerometerParam(gravity[0], gravity[1], gravity[2]);
                        //orienGetter.setAccelerometerParam((float)x, (float)y, (float)z);
                        Debug.WriteLine("AcceleroValues: {0}, {1}, {2}", values[0], values[1], values[2]);
                        break;
                    case MotionSensorType.Magnetometer:
                        //x = ((MotionVector)a.Value).X;
                        //y = ((MotionVector)a.Value).Y;
                        //z = ((MotionVector)a.Value).Z;
                        //orienGetter.setMagnetometerParam((float)x, (float)y, (float)z);     
                        values[0] = (float)((MotionVector)a.Value).X;
                        values[1] = (float)((MotionVector)a.Value).Y;
                        values[2] = (float)((MotionVector)a.Value).Z;
                        Debug.WriteLine("MagnetoValues: {0}, {1}, {2}", values[0], values[1], values[2]);
                        //orienGetter.setMagnetometerParam(values);
                        orienGetter.setMagnetometerParam(values[0], values[1], values[2]);
                        filterLowPass(orienGetter.getOrientation(), orientation, 0.95f);

                        for (int i = 0; i < 3; i++)
                            degree[i] = (float)(orientation[i] * 180 / Math.PI);

                        //give a text title to current direction.
                        string str = directionEstimate(degree[0]);
                        //string str = "old version.";

                        MainPage_ViewModel exemp1 = MainPage.FindByName<MainPage_ViewModel>("mainpageVM");
                        exemp1.TextContent = str;
                        exemp1.ValueX = degree[0];
                        exemp1.ValueY = degree[1];
                        exemp1.ValueZ = degree[2];
                        exemp1.Rotation = -degree[0];
                        exemp1.RotationX = -degree[1];
                        exemp1.RotationY = -degree[2];



                        break;
                }
            };
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

        public string directionEstimate(float d)
        {
            string result = "Result can not be found.";
            if (d >= -30 && d < 30)
                result = "Nord";
            else if (d >= 30 && d < 60)
                result = "Nord-Est";
            else if (d >= 60 && d < 120)
                result = "Est";
            else if (d >= 120 && d < 150)
                result = "Sud-Est";
            else if ((d >= 150 && d <= 180) || (d >= -180 && d < -150))
                result = "Sud";
            else if (d >= -150 && d < -120)
                result = "Sud-West";
            else if (d >= -120 && d < -60)
                result = "West";
            else if (d >= -60 && d < -30)
                result = "Nord-West";

            return result;
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
