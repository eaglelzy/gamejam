using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    public PlayerData Data;

    #region Variables
    public Rigidbody2D RB { get; private set; }
    public BoxCollider2D Collider { get; private set; }
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsDashing { get; private set; }
    public bool IsFlying{get; private set; }
    public float LastOnGroundTime { get; private set; }
    public float LastPressedDashTime { get; private set; }
    public float LastPressedFlyTime { get; private set; }
    public float LastPressedJumpTime { get; private set; }

    private bool _isJumpCut;

    private bool _isJumpFalling;

    private Vector2 _moveInput;
    
    
    private int _dashesLeft;
    private bool _dashRefilling;
    private Vector2 _lastDashDir;
    private bool _isDashAttacking;

    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;    
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    #endregion

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();
        SetGravityScale(Data.gravityScale);
        IsFacingRight = true;
    }
    

    void Update()
    {

        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;
        LastPressedFlyTime -= Time.deltaTime;
        #endregion

        #region Input
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        if(_moveInput.x!=0)
            CheckDirectionToFace(_moveInput.x > 0);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            OnJumpInput();
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W))
        {
            OnJumpUpInput();
        }

        // if (Input.GetKeyDown(KeyCode.LeftShift))
        // {
        //     OnDashInput();
        // }

        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     OnFlyInput();
        // }
        #endregion
       
        #region Collision Check

        if (!IsJumping)
        {
            if(Physics2D.OverlapBox(_groundCheckPoint.position,_groundCheckSize,0,_groundLayer) && !IsJumping)
            {
                LastOnGroundTime = Data.coyoteTime;
            }

          
        }

        #endregion

        #region Jump Check

        if (IsJumping && RB.velocity.y < 0)
        {
            IsJumping = false;
            _isJumpFalling = true;

        }

        if (LastOnGroundTime > 0 && !IsJumping)
        {
            _isJumpCut = false;

            if (!IsJumping)
                _isJumpFalling = false;

        }

        if (CanJump() && LastPressedJumpTime > 0)
        {
            IsJumping = true;
            _isJumpFalling = false;
            _isJumpCut = false;
            Jump();
        }

        #endregion

        #region Dash Check
        if(CanDash()&&LastPressedDashTime>0)
        {
            Sleep(Data.dashSleepTime);

            if (_moveInput != Vector2.zero)
                _lastDashDir = _moveInput;
            else
                _lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;

            IsDashing = true;
            IsJumping = false;
            _isJumpCut = false;

            StartCoroutine(nameof(StartDash), _lastDashDir);

        }
        #endregion

        #region Fly Check

        if (CanFly()&&LastPressedFlyTime>0)
        {
            IsFlying = true;
            IsJumping = false;
            Fly();
        }
        if (LastOnGroundTime > 0)
        {
            IsFlying = false;
        }

        #endregion

        #region Gravity

        

        if (RB.velocity.y < 0 && _moveInput.y < 0)
        {
            SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
        }
        else if (_isJumpCut)
        {
            SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
        }
        else if((IsJumping||_isJumpFalling) && MathF.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
        }
        else if (RB.velocity.y < 0)
        {
            if(IsFlying)
                SetGravityScale(Data.gravityScale * Data.flyingGravityMult);
            else
                SetGravityScale(Data.gravityScale * Data.fallGravityMult);
            
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
        }
        else
        {
            SetGravityScale(Data.gravityScale);
        }
        #endregion
       
    }

    private void FixedUpdate()
    {
         if(!IsDashing)
        {           
            Run(1);
        }        
         else if (_isDashAttacking)
        {
            Run(Data.dashEndRunLerp);
        }    
    }

    #region Interact Check
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RefillDashNow(1);
        //Destroy(collision.gameObject);
    }
    #endregion

    #region Method

    private void ShowTrailer()
    {
        
    }

    private void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight&&!IsDashing)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            IsFacingRight = !IsFacingRight;
        }
            
    }

    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;

    }
    private void Sleep(float duration)
    {
        StartCoroutine(nameof(PerformSleep), duration);
    }
    private IEnumerator PerformSleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }

    #region Jump Method
    private void OnJumpInput()
    {
        LastPressedJumpTime = Data.jumpInputBufferTime;
    }
    public void OnJumpUpInput()
    {
        if (CanJumpCut())
            _isJumpCut = true;
    }

    private bool CanJumpCut()
    {
        return IsJumping && RB.velocity.y > 0;
    }

    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;

    }

    private void Jump()
    {
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        float force = Data.jumpForce;
        if (RB.velocity.y < 0)
        {
            force -= RB.velocity.y; 
        }

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        EffectSoundManager.Instance.PlayEffectSound("Jump");
    }
    #endregion

    #region Run Method
    private void Run(float lerpAmount)
    {
        float targetSpeed = _moveInput.x * Data.runMaxSpeed;
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        float accelRate;

        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
        #region Add Bonus Jump Apex Acceleration
        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }
        #endregion
        #region Conserve Momentum
        
        if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            
            accelRate = 0;
        }
        #endregion

        float speedDif = targetSpeed - RB.velocity.x;

        float movement = speedDif * accelRate;

        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    #endregion

    #region Dash Method
    private void OnDashInput()
    {
        LastPressedDashTime = Data.dashInputBufferTime;
    }

    private bool CanDash()
    {
        if (!IsDashing && _dashesLeft < Data.dashAmount && LastOnGroundTime > 0 && !_dashRefilling)
        {
            StartCoroutine(nameof(RefillDash), 1);
        }
        
        return _dashesLeft > 0;
    }

    private IEnumerator StartDash(Vector2 dir)
    {
        LastOnGroundTime = 0;
        LastPressedDashTime = 0;

        float startTime = Time.time;

        _dashesLeft--;
        _isDashAttacking = true;

        SetGravityScale(0);

        
        while (Time.time - startTime <= Data.dashAttackTime)
        {
            RB.velocity = dir.normalized * Data.dashSpeed;
           
            yield return null;
        }

        startTime = Time.time;

        _isDashAttacking = false;

        //Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
        SetGravityScale(Data.gravityScale);
        RB.velocity = Data.dashEndSpeed * dir.normalized;

        while (Time.time - startTime <= Data.dashEndTime)
        {
            yield return null;
        }

       
        IsDashing = false;
    }

   
    private IEnumerator RefillDash(int amount)
    {
        
        _dashRefilling = true;
        yield return new WaitForSeconds(Data.dashRefillTime);
        _dashRefilling = false;
        _dashesLeft = Mathf.Min(Data.dashAmount, _dashesLeft + 1);
    }


    private void RefillDashNow(int amount)
    {
        _dashesLeft = Mathf.Min(Data.dashAmount, _dashesLeft + 1);
    }
    #endregion

    #region Fly Method

    private void OnFlyInput()
    {
        LastPressedFlyTime = Data.flyInputBufferTime;
    }

    private bool CanFly()
    {
        
        return LastOnGroundTime < 0 &&!IsFlying ;

    }

    private void Fly()
    {
        RB.AddForce(Vector2.up*Data.upForce, ForceMode2D.Impulse);
        //SetGravityScale(Data.gravityScale * Data.flyingGravityMult);
        //RB.velocity = new Vector2(Mathf.Clamp(RB.velocity.x, 0f, Data.flyMaxSpeed.x),
        //                        Mathf.Clamp(RB.velocity.y, 0f, Data.flyMaxSpeed.y));
    }

    #endregion

    #endregion



    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
       
       
    }
    #endregion

    #region UI
    
    #endregion
}
