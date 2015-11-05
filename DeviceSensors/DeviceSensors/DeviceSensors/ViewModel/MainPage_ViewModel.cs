using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSensors.ViewModel
{
    public class MainPage_ViewModel : INotifyPropertyChanged
    {
        string textContent;
        double valueX, valueY, valueZ;
        double rotation, rotationX, rotationY;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly double interval = 1.2;
        public string TextContent
        {
            set
            {
                if (textContent != value)
                {
                    textContent = value;
                    OnPropertyChanged("TextContent");
                }
            }

            get
            {
                return textContent;
            }
        }

        public double ValueX
        {
            set
            {
                if (valueX != value)
                {
                    valueX = value;
                    OnPropertyChanged("ValueX");
                }
            }

            get
            {
                return valueX;
            }
        }

        public double ValueY
        {
            set
            {
                if (valueY != value)
                {
                    valueY = value;
                    OnPropertyChanged("ValueY");
                }
            }

            get
            {
                return valueY;
            }
        }

        public double ValueZ
        {
            set
            {
                if (valueZ != value)
                {
                    valueZ = value;
                    OnPropertyChanged("ValueZ");
                }
            }

            get
            {
                return valueZ;
            }
        }

        public double Rotation
        {
            set
            {
                if (Math.Abs(rotation - value) >= interval)
                {
                    rotation = value;
                    OnPropertyChanged("Rotation");
                }
            }
            get
            {
                return rotation;
            }
        }

        public double RotationX
        {
            set
            {
                if (Math.Abs(rotationX - value) >= interval)
                {
                    rotationX = value;
                    OnPropertyChanged("RotationX");
                }
            }
            get
            {
                return rotationX;
            }
        }

        public double RotationY
        {
            set
            {
                if (Math.Abs(rotationY - value) >= interval)
                {
                    rotationY = value;
                    OnPropertyChanged("RotationY");
                }
            }
            get
            {
                return rotationY;
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
