using System;
using UnityEngine;


public interface ICopyable<T>
{
    RotationSettings Copy();
}

public class RotationSettings : MonoBehaviour, ICopyable<RotationSettings>
{
    [SerializeField] private float _minPitchAngle = -80f;
    [SerializeField] private float _maxPitchAngle = 80f;
    [SerializeField] private RotationBehavior _rotationBehavior = RotationBehavior.Continuous;

    [SerializeField] private float _minRotationSpeed = 20f;
    [SerializeField] private float _maxRotationSpeed = 360f;

    public float MinPitchAngle => _minPitchAngle;
    public float MaxPitchAngle => _maxPitchAngle;
    public RotationBehavior RotationBehavior => _rotationBehavior;
    public float MinRotationSpeed => _minRotationSpeed;
    public float MaxRotationSpeed => _maxRotationSpeed;

    public RotationSettings Copy()
    {
        return new RotationSettings()
        {
            _minPitchAngle = _minPitchAngle,
            _maxPitchAngle = _maxPitchAngle,
            _rotationBehavior = _rotationBehavior,
            _minRotationSpeed = _minRotationSpeed,
            _maxRotationSpeed = _maxRotationSpeed
        };
    }
}

public enum RotationBehavior
{
    Continuous,
    Limited
}