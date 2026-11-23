using UnityEngine;

public class Neuron
{
    public float[] _Weights;
    public float _Bias;

    public float GetOutput(float[] input)
    {
        float result = 0;
        for (int i = 0; i < input.Length; i++)
        {
            result += input[i] * _Weights[i];
        }
        return result + _Bias;
    }

    public void InitializeWeights(int inputCount)
    {
        _Weights = new float[inputCount];
        
        for (int i = 0; i < inputCount; i++)
        {
            _Weights[i] = UnityEngine.Random.Range(-1f, 1f);
        }
        _Bias = UnityEngine.Random.Range(-1f, 1f);
    }

    public void SetWeights(float[] weights, float bias)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            _Weights[i] = weights[i];
        }
        
        _Bias = bias;
    }

    public Neuron Clone()
    {
        Neuron n = new Neuron();

        n._Weights = (float[])_Weights.Clone();
        n._Bias = _Bias;

        return n;
    }
    public void BecameSameNeuron(Neuron neuron)
    {
        _Weights = (float[])neuron._Weights.Clone();
        _Bias = neuron._Bias;

    }

    public void Mutate(float Amount)
    {
        for(int i = 0; i < _Weights.Length;i++)
        {
           _Weights[i] += Random.Range(-Amount,Amount); 
        }
    }
}
