using UnityEngine;

/// <summary>
/// 必要前置需求：
///     1. 有InputSystem，且绑定好以下Actions（直接使用文件夹中的Controller也可）
///         a. Move中包含WASD、Arrows和LeftStick 其中action属性为（PassThrough,Vector2,True）
///             aa.使用Add UP/DOWN/LEFT/RIGHT Modifier来创建WASD和Arrows的绑定
///         b. Jump中包含Space和ButtonSouth
///         c. Run中包含Shift和ButtonWest
///     2. INPUT对象，包含一个InputManager脚本
///         a. 可以使用10.输入系统模板中的InputManager
///         b. 或包含必要的移动输入处理的框架(Qframework)
///     3. Player对象
///         a. 包含两个碰撞箱子对象，分别代表身体和脚部
///         b. 创建Rigidibody组件，质量设置为0.0001(尽可能低，但不要为0)
///         c. 阻力和重力设置为0(Angular Drag、LinearDrag和Gravity Scale)
///         d. Collision Dectection设置为Continuous （可以防止快速移动的物体穿过其他物体）
///         e. Interpolate设置为Interpolate(插值模式)，插值模式用于平滑物体的运动
///         f. Freeze Rotation设置为Freeze Rotation Z(冻结Z轴旋转)
///     4. 物理材质
///         a. 创建PhysicMaterial，摩擦力设置为0，弹力设置为0
///         b. 赋给Player的Rigidibody中的Material
/// </summary>
public class NewBehaviourScript : MonoBehaviour
{
    [Header("引用")]
    public 角色移动状态 移动状态;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private Collider2D _bodyColl;

    private Rigidbody2D _rb;

    // 移动
    private Vector2 _moveVelocity;
    private bool _isFacingRight;

    // 碰撞检测
    private RaycastHit2D _地射线;
    private RaycastHit2D _顶射线;
    private bool _是否触地;
    private bool _是否触顶;

    // 跳跃
    public float 竖直速度 { get; private set; }
    private bool _跳跃中;
    private bool _快速下落;
    private bool _下落中;
    private float _快速下落时间;
    private float _快速下落初始速度;
    private int _已跳跃次数;

    // 最高点变量
    private float _最高点进度;
    private float _超过最高点阈值时间;
    private bool _是否超过最高点阈值;

    // 跳跃缓冲
    private float _跳跃缓冲计时器;
    private bool _跳跃缓冲期间释放跳跃键;


    private float _悬空跳跃计时器;

    private void Awake()
    {
        _isFacingRight = true;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CountTimer();
        JumpCheck();
    }

    private void FixedUpdate()
    {
        CollisionCheck();
        Jump();
        if (_是否触地)
        {
            Move(移动状态.地面加速度, 移动状态.地面减速度, InputManager.Movement);
        }
        else
        {
            Move(移动状态.空中加速度, 移动状态.空中减速度, InputManager.Movement);
        }
    }

    #region 移动
    private void Move(float 加速度, float 减速度, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            TrunCheck(moveInput); // 转向检测

            Vector2 目标速度 = Vector2.zero;
            if (InputManager.RunIsHeld)
            {
                目标速度 = new Vector2(moveInput.x, 0f) * 移动状态.最大跑步速度;
            }
            else
            {
                目标速度 = new Vector2(moveInput.x, 0f) * 移动状态.最大走动速度;
            }

            _moveVelocity = Vector2.Lerp(_moveVelocity, 目标速度, 加速度 * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
        }
        else if (moveInput == Vector2.zero)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, 减速度 * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
        }
    }

    private void TrunCheck(Vector2 moveInput)
    {
        if (_isFacingRight && moveInput.x < 0)
        {
            Trun(false);
        }
        else if (!_isFacingRight && moveInput.x > 0)
        {
            Trun(true);
        }
    }

    private void Trun(bool turnRight)
    {
        if (turnRight)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            _isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }
    #endregion

    #region 跳跃
    private void JumpCheck()
    {
        // 按下
        if (InputManager.JumpWasPressed)
        {
            _跳跃缓冲计时器 = 移动状态.跳跃缓冲时间;
            _跳跃缓冲期间释放跳跃键 = false;
        }
        // 释放
        if (InputManager.JumpWasReleased)
        {
            if (_跳跃缓冲计时器 > 0)
            {
                _跳跃缓冲期间释放跳跃键 = true;
            }
            if (_跳跃中 && 竖直速度 > 0)
            {
                if (_是否超过最高点阈值)
                {
                    _是否超过最高点阈值 = false;
                    _快速下落 = true;
                    _快速下落时间 = 移动状态.跳跃取消窗口;
                    竖直速度 = 0f;
                }
                else
                {
                    _快速下落 = true;
                    _快速下落初始速度 = 竖直速度;
                }

            }
        }
        // 初始化跳跃
        if (_跳跃缓冲计时器 > 0f && !_跳跃中 && (_是否触地 || _悬空跳跃计时器 > 0))
        {
            InitiateJump(1);

            if (_跳跃缓冲期间释放跳跃键)
            {
                _快速下落 = true;
                _快速下落初始速度 = 竖直速度;
            }
        }
        // 多级跳跃
        else if (_跳跃缓冲计时器 > 0f && _跳跃中 && _已跳跃次数 < 移动状态.最大跳跃次数)
        {
            _快速下落 = false;
            InitiateJump(1);
        }
        // 空中跳跃
        else if (_跳跃缓冲计时器 > 0f && _下落中 && _已跳跃次数 < 移动状态.最大跳跃次数 - 1)
        {
            InitiateJump(2);
            _快速下落 = false;
        }
        // 着陆检测
        if ((_跳跃中 || _下落中) && _是否触地 && 竖直速度 <= 0f)
        {
            _跳跃中 = false;
            _下落中 = false;
            _快速下落 = false;
            _快速下落时间 = 0f;
            _是否超过最高点阈值 = false;
            _已跳跃次数 = 0;

            竖直速度 = Physics2D.gravity.y;

        }
    }

    private void InitiateJump(int 已用跳跃次数)
    {
        if (!_跳跃中)
        {
            _跳跃中 = true;
        }
        _跳跃缓冲计时器 = 0f;
        _已跳跃次数 += 已用跳跃次数;
        竖直速度 = 移动状态.初始跳跃速度;
    }

    private void Jump()
    {
        // 应用重力
        if (_跳跃中)
        {
            // 检查触顶
            if (_是否触顶)
            {
                _快速下落 = true;
            }
            // 上升重力
            if (竖直速度 >= 0f)
            {
                // 最高点控制
                _最高点进度 = Mathf.InverseLerp(移动状态.初始跳跃速度, 0f, 竖直速度);
                if (_最高点进度 > 移动状态.最高点阈值)
                {
                    if (!_是否超过最高点阈值)
                    {
                        _是否超过最高点阈值 = true;
                        _超过最高点阈值时间 = 0f;
                    }

                    if (_是否超过最高点阈值)
                    {
                        _超过最高点阈值时间 += Time.fixedDeltaTime;
                        if (_超过最高点阈值时间 < 移动状态.最高点停留时间)
                        {
                            竖直速度 = 0f;
                        }
                        else
                        {
                            竖直速度 = -0.01f;
                        }
                    }
                }
                else
                {
                    竖直速度 += 移动状态.重力加速度 * Time.fixedDeltaTime;
                    if (_是否超过最高点阈值)
                    {
                        _是否超过最高点阈值 = false;
                    } 
                }
            }
            else if (!_快速下落)
            {
                竖直速度 += 移动状态.重力加速度 * 移动状态.跳跃释放重力倍率 * Time.fixedDeltaTime;
            }
            else if (竖直速度 < 0f)
            {
                if (!_下落中)
                {
                    _下落中 = true;
                }
            }
        }
        // 跳跃取消
        if(_快速下落)
        {
            if(_快速下落时间 >= 移动状态.跳跃取消窗口)
            {
                竖直速度 += 移动状态.重力加速度 * 移动状态.跳跃释放重力倍率 * Time.fixedDeltaTime;
            }
            else if (_快速下落时间 <= 移动状态.跳跃取消窗口)
            {
                竖直速度 = Mathf.Lerp(_快速下落初始速度, 0f, (_快速下落时间/ 移动状态.跳跃取消窗口));
            }
            _快速下落时间 += Time.fixedDeltaTime;
        }
        // 正常下落(掉落)
        if(!_是否触地 && !_跳跃中)
        {
            if (!_下落中)
            {
                _下落中 = true;
            }

            竖直速度 += 移动状态.重力加速度 * Time.fixedDeltaTime;
        }
        // Clamp

        竖直速度 = Mathf.Clamp(竖直速度, -移动状态.最大下落速度, 50f);

        _rb.velocity = new Vector2(_rb.velocity.x, 竖直速度);
    }
    #endregion

    #region 碰撞检测
    private void isGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, 移动状态.地面射线长度);

        _地射线 = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, 移动状态.地面射线长度, 移动状态.地面层);
        if (_地射线.collider != null)
        {
            _是否触地 = true;
        }
        else
        {
            _是否触地 = false;
        }

        #region Debug地面检测
        if (移动状态.Debug地面碰撞箱)
        {

            Color rayColor;
            if (_是否触地)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastOrigin.x / 2, boxCastOrigin.y), Vector2.down * 移动状态.地面射线长度, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastOrigin.x / 2, boxCastOrigin.y), Vector2.down * 移动状态.地面射线长度, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastOrigin.x / 2, boxCastOrigin.y - 移动状态.地面射线长度), Vector2.right * boxCastSize.x, rayColor);
        }
        #endregion
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * 移动状态.顶部宽度, 移动状态.顶部射线长度);

        _顶射线 = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, 移动状态.顶部射线长度, 移动状态.地面层);
        if(_顶射线.collider != null)
        {
            _是否触顶 = true;
        }
        else
        {
            _是否触顶 = false;
        }

        #region Debug
        if(移动状态.Debug头部碰撞箱)
        {
            float headWidth = 移动状态.顶部宽度;
             Color rayColor;
            if (_是否触地)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastOrigin.x / 2, boxCastOrigin.y),
                Vector2.up * 移动状态.顶部射线长度, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastOrigin.x / 2, boxCastOrigin.y), 
                Vector2.up * 移动状态.顶部射线长度, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastOrigin.x / 2, boxCastOrigin.y - 移动状态.顶部射线长度), 
                Vector2.right * boxCastSize.x * headWidth, rayColor);
        }
        #endregion
    }
    private void CollisionCheck()
    {
        isGrounded();
        BumpedHead();
    }
    #endregion

    #region 计时器
    private void CountTimer()
    {
        _跳跃缓冲计时器 -= Time.deltaTime;
        if (!_是否触地)
        {
            _悬空跳跃计时器 -= Time.deltaTime;
        }
        else
        {
            _悬空跳跃计时器 = 移动状态.悬空跳跃缓冲时间;
        }
    }
    #endregion
}
