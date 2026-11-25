using UnityEngine;

public class InputsControl : MonoBehaviour
{
    public int _BodyPartsCount;
    public BodyPartsContainer BodyPartsContainer;
    [SerializeField] Transform _Head;
    [SerializeField] Transform _Torso;
    public bool LeftFootOnFloor;
    public bool RightFootOnFloor; 
    public float[] _Inputs;

    float MaxJointSpeed = 400;

    float time = 0;

    void Awake()
    {
        _BodyPartsCount = BodyPartsContainer._BodyParts.Length;
        _Inputs = new float[_BodyPartsCount * 2 + 7];
    }

    public void SetFootSensorData(bool state, bool IsRight)
    {
       if(IsRight) {RightFootOnFloor = state; return;}
       LeftFootOnFloor = state;
    }

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;

        for (int i = 0; i < BodyPartsContainer._BodyParts.Length; i++)
        {
            HingeJoint2D bodyPart = BodyPartsContainer._BodyParts[i];

            JointAngleLimits2D lim = bodyPart.limits;
            float normalizedangle = Mathf.InverseLerp(lim.min, lim.max, bodyPart.jointAngle) * 2f - 1f;

            float speedNorm = Mathf.Clamp(bodyPart.jointSpeed / MaxJointSpeed, -1f, 1f);


            _Inputs[i] = normalizedangle;
            _Inputs[i + _BodyPartsCount] = speedNorm;
        }

        //_Inputs[_Inputs.Length - 1] = Mathf.Sin(Time.time);
        _Inputs[_Inputs.Length - 1] = Mathf.Clamp01(Mathf.InverseLerp(-3, 1.3f, _Head.position.y));
        float rad = _Torso.transform.eulerAngles.z * Mathf.Deg2Rad;
        _Inputs[_Inputs.Length - 2] = Mathf.Sin(rad);
        _Inputs[_Inputs.Length - 3] = Mathf.Cos(rad); 
        _Inputs[_Inputs.Length - 4] = Mathf.Sin(time * 2f * Mathf.PI);
        _Inputs[_Inputs.Length - 5] = Mathf.Cos(time * 2f * Mathf.PI);
        _Inputs[_Inputs.Length - 6] = RightFootOnFloor ? 1 : 0;
        _Inputs[_Inputs.Length - 7] = LeftFootOnFloor ? 1 : 0;
    }
}
