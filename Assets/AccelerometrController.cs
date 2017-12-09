using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{

    public class AccelerometrController
    {
        Matrix4x4 calibrationMatrix;
        Vector3 wantedDeadZone = Vector3.zero;

        const float AccelerometerUpdateInterval = 1.0f / 60.0f;
        const float LowPassKernelWidthInSeconds = 0.3f;

        private float LowPassFilterFactor = AccelerometerUpdateInterval / LowPassKernelWidthInSeconds; // tweakable
        private Vector3 lowPassValue = Vector3.zero;


        public AccelerometrController()
        {
            lowPassValue = Input.acceleration;
            CalibrateAcceleration();
        }

        public Vector3 GetVector3()
        {
            lowPassValue = Vector3.Lerp(lowPassValue, Input.acceleration, LowPassFilterFactor);
            return getAccelerometer(lowPassValue).normalized;
        }

        Vector3 getAccelerometer(Vector3 accelerator)
        {
            Vector3 accel = this.calibrationMatrix.MultiplyVector(accelerator);
            return accel;
        }

        public void CalibrateAcceleration()
        {
            wantedDeadZone = Input.acceleration;
            Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0f, 0f, -1f), wantedDeadZone);
            //create identity matrix ... rotate our matrix to match up with down vec
            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, new Vector3(1f, 1f, 1f));
            //get the inverse of the matrix
            calibrationMatrix = matrix.inverse;
        }
    }
}
