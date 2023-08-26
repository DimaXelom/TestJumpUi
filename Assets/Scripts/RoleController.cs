using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


public class RoleController : MonoBehaviour
{
    public static RoleController thisScript;
    public event Action<PLAYER_STATE> EventMove;
    public Animator animator = null;
    public GameObject target = null;
    private PLAYER_STATE _currentPlayerState = PLAYER_STATE.Move;

    public int inputCount;

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


    internal void SpeedBall(PLAYER_MOVE moveState)
    {
        switch (moveState)
        {
            case PLAYER_MOVE.slowSpeed:
                animator.Play("slow");
                break;
            case PLAYER_MOVE.normalSpeed:
                animator.Play("normal");
                break;
            case PLAYER_MOVE.fastSpeed:
                animator.Play("fast");
                break;
            default:
                break;
        }
    }
}