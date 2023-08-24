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

    private void Update()
    {
        if (_currentPlayerState == PLAYER_STATE.Move)
        {
            HandleMovement();
        }
        else if (_currentPlayerState == PLAYER_STATE.Hit)
        {
            HandleHit();
        }

        HandleLookAtTarget();
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float moveSpeed = 5f;
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    private void HandleHit()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HitBall(PLAYER_HIT.dynamicHit);
        }
    }

    private void HandleLookAtTarget()
    {
        gameObject.transform.LookAt(target.transform);
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
}