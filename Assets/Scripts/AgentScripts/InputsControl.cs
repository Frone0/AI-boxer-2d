using UnityEngine;

public class InputsControl : MonoBehaviour
{
    public int _BodyPartsCount;
    public BodyPartsContainer BodyPartsContainer;
    public float[] _Inputs;

    float MaxJointSpeed = 400;

    void Awake()
    {
        _Inputs = new float[_BodyPartsCount * 2 + 1];
    }

    void FixedUpdate()
    {
        for (int i = 0; i < BodyPartsContainer._BodyParts.Length; i++)
        {
            HingeJoint2D bodyPart = BodyPartsContainer._BodyParts[i];

            JointAngleLimits2D lim = bodyPart.limits;
            float normalizedangle = Mathf.InverseLerp(lim.min, lim.max, bodyPart.jointAngle) * 2f - 1f;

            float speedNorm = Mathf.Clamp(bodyPart.jointSpeed / MaxJointSpeed, -1f, 1f);


            _Inputs[i] = normalizedangle;
            _Inputs[i + _BodyPartsCount] = speedNorm;
        }

        _Inputs[_Inputs.Length - 1] = Mathf.Sin(Time.time);
    }
}
