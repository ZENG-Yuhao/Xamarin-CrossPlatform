

namespace DeviceSensors.Interface
{
    public interface IDeviceOrientation
    {
        IDeviceOrientation setAccelerometerParam(float x, float y, float z);
        IDeviceOrientation setAccelerometerParam(float[] values);
        IDeviceOrientation setMagnetometerParam(float x, float y, float z);
        IDeviceOrientation setMagnetometerParam(float[] values);
        float[] getRotationMatrix();
        float[] getOrientation();
        float[] getOrientation(float[] rotationMatrix);
        void getRotationMatrix(float[] R, float[] I, float[] elem1, float[] elem2);

    }
}
