using UnityEngine;

public class BodyPartsControl : MonoBehaviour
{
    public BodyPartsContainer BodyPartsContainer;

    [SerializeField] float _Gain = 10f;
    [SerializeField] float _MaxMotorSpeed = 400f;
    [SerializeField] float _MotorForce = 1500f;

    public void Apply(float[] outputs)
    {
        HingeJoint2D[] joints = BodyPartsContainer._BodyParts;

        for (int i = 0; i < joints.Length; i++)
        {
            HingeJoint2D joint = joints[i];

            JointAngleLimits2D lim = joint.limits;
            float target = Mathf.Lerp(lim.min, lim.max, (outputs[i] + 1f) * 0.5f);
            float error = target - joint.jointAngle;

            JointMotor2D m = joint.motor;
            m.motorSpeed = Mathf.Clamp(error * _Gain, -_MaxMotorSpeed, _MaxMotorSpeed);
            m.maxMotorTorque = _MotorForce;
            joint.motor = m;
            joint.useMotor = true;
        }
    }
}