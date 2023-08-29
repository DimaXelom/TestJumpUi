using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

public enum PLAYER_STATE
{
    Move,
    Hit
}

public enum PLAYER_HIT
{
    staticHit,
    dynamicHit
}

public enum PLAYER_MOVE
{
    slowSpeed,
    normalSpeed,
    fastSpeed,
}


public class RoleController : MonoBehaviour, IInjectReceiver<ISpeedReference>
{
    public static RoleController thisScript;
    public event Action<PLAYER_STATE> EventMove;
    public Animator animator = null;
    public GameObject target = null;
    private PLAYER_STATE _currentPlayerState = PLAYER_STATE.Move;

    public int inputCount;
    private ISpeedReference _speedReference;

    private void Awake()
    {
        thisScript = this;
    }

    private void Start()
    {
        inputCount = 0;
        _currentPlayerState = PLAYER_STATE.Move;
        EventMove?.Invoke(_currentPlayerState);
    }

    private void HandleMovement()
    {
        if (_currentPlayerState != PLAYER_STATE.Move)
            return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float moveSpeed = 5f;
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    private void HandleHit()
    {
        if (_currentPlayerState != PLAYER_STATE.Hit)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HitBall(PLAYER_HIT.dynamicHit);
        }
    }


    private void HandleLookAtTarget()
    {
        if (target != null)
        {
            gameObject.transform.LookAt(target.transform);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleHit();
        HandleLookAtTarget();
    }

    internal void HitBall(PLAYER_HIT hitState)
    {
        if (inputCount != 0) return;
        inputCount++;

        switch (hitState)
        {
            case PLAYER_HIT.dynamicHit:
                animator.Play("dynamic");
                break;
            case PLAYER_HIT.staticHit:
                animator.Play("static");
                break;

            default:
                break;
        }
    }


    public class SpeedReference : MonoBehaviour, ISpeedReference
    {
        public Animator animator;
        private float _speedCounter = 1f;

        public void Speed(PLAYER_MOVE moveState)
        {
            switch (moveState)
            {
                case PLAYER_MOVE.slowSpeed:
                    _speedCounter = 1;

                    break;
                case PLAYER_MOVE.normalSpeed:
                    _speedCounter = 2;

                    break;
                case PLAYER_MOVE.fastSpeed:
                    _speedCounter = 3;

                    break;
                default:
                    break;
            }

            switch (_speedCounter)
            {
                case 1:
                    animator.Play("slow");
                    break;
                case 2:
                    animator.Play("normal");
                    break;
                case 3:
                    animator.Play("fast");
                    break;
                default:
                    break;
            }
        }
    }


    public void Inject(ISpeedReference reference)
    {
        _speedReference = reference;
    }

    internal void Speed(PLAYER_MOVE moveState)
    {
        _speedReference.Speed(moveState);
    }
}

public interface IInjectReceiver
{
}

public interface IInjectReceiver<in T> : IInjectReceiver
{
    void Inject(T reference);
}

public interface ISpeedReference
{
    void Speed(PLAYER_MOVE moveState);
}

public static class InjectExtensions
{
    public static T InjectReference<T>(this IInjectReceiver<T> receiver) where T : Object, ISpeedReference
    {
        if (ReferenceDistibutor.TryGetReference(out T reference))
        {
            receiver.Inject(reference);
        }
        else
        {
            Debug.LogException(new NullReferenceException($"{typeof(T).Name} missing in scene"));
        }

        return reference;
    }
}

public class ReferenceDistibutor : MonoBehaviour
{
    public static bool TryGetReference<T>(out T distributingReference) where T : Object, ISpeedReference
    {
        distributingReference = FindObjectOfType<T>();

        if (distributingReference != null)
        {
            return true;
        }
        else
        {
            Debug.LogError($"{typeof(T).Name} missing in scene");
            return false;
        }
    }
}