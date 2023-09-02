using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringArm : MonoBehaviour
{
    
    [SerializeField] private float _targetLength = 3.0f;
    [SerializeField] private float _speedDump = 0.0f;
    [SerializeField] private Transform _collisionSocket;
    [SerializeField] private float _collisionRadius = 0.25f;
    [SerializeField] private LayerMask _collisionMask = 0;
    [SerializeField] private Camera _cameraReference;
    [SerializeField] private float _cameraViewPortExtentsMultiplier = 1.0f;

    private Vector3 _socketVelocity;
    private Transform _thisTransform;
    private const float MinRadius = 0.001f;

    private void Awake()
    {
        _thisTransform = transform;
    }

    private void LateUpdate()
    {
        if (_cameraReference != null)
        {
            _collisionRadius = GetCollisionRadiusForCamera(_cameraReference);

            _cameraReference.transform.localPosition = -Vector3.forward * _cameraReference.nearClipPlane;
        }

        UpdateLength();
    }

    private void UpdateLength()
    {
        var desiredLength = GetDesiredTargetLength();
        var newSocketLocalPosition = -Vector3.forward * desiredLength;

        _collisionSocket.localPosition = Vector3.SmoothDamp(_collisionSocket.localPosition, newSocketLocalPosition,
            ref _socketVelocity, _speedDump);
    }

    private void OnDrawGizmos()
    {
        if (_collisionSocket == null) return;

        Gizmos.color = Color.green;
        var position = _collisionSocket.transform.position;

        Gizmos.DrawLine(transform.position, position);
        GizmosExtensions.DrawGizmosSphere(position, _collisionRadius);
    }
    private float GetCollisionRadiusForCamera(Camera cameraReference)
    {
        var halfFOV = (cameraReference.fieldOfView / 2.0f) * Mathf.Deg2Rad;
        var nearClipPlaneHalfHeight =
            Mathf.Tan(halfFOV) * cameraReference.nearClipPlane * _cameraViewPortExtentsMultiplier;
        var nearClipPlaneHalfWidth = nearClipPlaneHalfHeight * cameraReference.aspect;
        var buffer = new Vector2(nearClipPlaneHalfWidth, nearClipPlaneHalfHeight).magnitude;

        return buffer;
    }

    private float GetDesiredTargetLength()
    {
        var ray = new Ray(_thisTransform.position, -_thisTransform.position);
        return Physics.SphereCast(ray, Mathf.Max(MinRadius, _collisionRadius), out var hit, _targetLength,
            _collisionMask)
            ? hit.distance
            : _targetLength;
    }
}

public class GizmosExtensions : MonoBehaviour
{
    public static void DrawGizmosSphere(Vector3 position, float collisionRadius)
    {
        float positionMagnitude = position.magnitude;
        positionMagnitude = collisionRadius;
    }
}
