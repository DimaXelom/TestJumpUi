using System;
using UnityEngine;


public interface ICopyable<T>
{
    RotationSettings Copy();
}

public class RotationSettings : MonoBehaviour, ICopyable<RotationSettings>
{
    [SerializeField] private float minPitchAngle = -80f;
    [SerializeField] private float maxPitchAngle = 80f;
    [SerializeField] private RotationBehavior rotationBehavior = RotationBehavior.Continuous;

    [SerializeField] private float minRotationSpeed = 20f;
    [SerializeField] private float maxRotationSpeed = 360f;

    public float MinPitchAngle => minPitchAngle;
    public float MaxPitchAngle => maxPitchAngle;
    public RotationBehavior RotationBehavior => rotationBehavior;
    public float MinRotationSpeed => minRotationSpeed;
    public float MaxRotationSpeed => maxRotationSpeed;

    public RotationSettings Copy()
    {
        return new RotationSettings()
        {
            minPitchAngle = minPitchAngle,
            maxPitchAngle = maxPitchAngle,
            rotationBehavior = rotationBehavior,
            minRotationSpeed = minRotationSpeed,
            maxRotationSpeed = maxRotationSpeed
        };
    }
}

public enum RotationBehavior
{
    Continuous,
    Limited
}