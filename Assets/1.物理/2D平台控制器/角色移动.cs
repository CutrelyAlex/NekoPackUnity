using UnityEngine;

/// <summary>
/// ��Ҫǰ������
///     1. ��InputSystem���Ұ󶨺�����Actions��ֱ��ʹ���ļ����е�ControllerҲ�ɣ�
///         a. Move�а���WASD��Arrows��LeftStick ����action����Ϊ��PassThrough,Vector2,True��
///             aa.ʹ��Add UP/DOWN/LEFT/RIGHT Modifier������WASD��Arrows�İ�
///         b. Jump�а���Space��ButtonSouth
///         c. Run�а���Shift��ButtonWest
///     2. INPUT���󣬰���һ��InputManager�ű�
///         a. ����ʹ��10.����ϵͳģ���е�InputManager
///         b. �������Ҫ���ƶ����봦��Ŀ��(Qframework)
///     3. Player����
///         a. ����������ײ���Ӷ��󣬷ֱ��������ͽŲ�
///         b. ����Rigidibody�������������Ϊ0.0001(�����ܵͣ�����ҪΪ0)
///         c. ��������������Ϊ0(Angular Drag��LinearDrag��Gravity Scale)
///         d. Collision Dectection����ΪContinuous �����Է�ֹ�����ƶ������崩���������壩
///         e. Interpolate����ΪInterpolate(��ֵģʽ)����ֵģʽ����ƽ��������˶�
///         f. Freeze Rotation����ΪFreeze Rotation Z(����Z����ת)
///     4. �������
///         a. ����PhysicMaterial��Ħ��������Ϊ0����������Ϊ0
///         b. ����Player��Rigidibody�е�Material
/// </summary>
public class NewBehaviourScript : MonoBehaviour
{
    [Header("����")]
    public ��ɫ�ƶ�״̬ �ƶ�״̬;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private Collider2D _bodyColl;

    private Rigidbody2D _rb;

    // �ƶ�
    private Vector2 _moveVelocity;
    private bool _isFacingRight;

    // ��ײ���
    private RaycastHit2D _������;
    private RaycastHit2D _������;
    private bool _�Ƿ񴥵�;
    private bool _�Ƿ񴥶�;

    // ��Ծ
    public float ��ֱ�ٶ� { get; private set; }
    private bool _��Ծ��;
    private bool _��������;
    private bool _������;
    private float _��������ʱ��;
    private float _���������ʼ�ٶ�;
    private int _����Ծ����;

    // ��ߵ����
    private float _��ߵ����;
    private float _������ߵ���ֵʱ��;
    private bool _�Ƿ񳬹���ߵ���ֵ;

    // ��Ծ����
    private float _��Ծ�����ʱ��;
    private bool _��Ծ�����ڼ��ͷ���Ծ��;


    private float _������Ծ��ʱ��;

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
        if (_�Ƿ񴥵�)
        {
            Move(�ƶ�״̬.������ٶ�, �ƶ�״̬.������ٶ�, InputManager.Movement);
        }
        else
        {
            Move(�ƶ�״̬.���м��ٶ�, �ƶ�״̬.���м��ٶ�, InputManager.Movement);
        }
    }

    #region �ƶ�
    private void Move(float ���ٶ�, float ���ٶ�, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            TrunCheck(moveInput); // ת����

            Vector2 Ŀ���ٶ� = Vector2.zero;
            if (InputManager.RunIsHeld)
            {
                Ŀ���ٶ� = new Vector2(moveInput.x, 0f) * �ƶ�״̬.����ܲ��ٶ�;
            }
            else
            {
                Ŀ���ٶ� = new Vector2(moveInput.x, 0f) * �ƶ�״̬.����߶��ٶ�;
            }

            _moveVelocity = Vector2.Lerp(_moveVelocity, Ŀ���ٶ�, ���ٶ� * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
        }
        else if (moveInput == Vector2.zero)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, ���ٶ� * Time.fixedDeltaTime);
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

    #region ��Ծ
    private void JumpCheck()
    {
        // ����
        if (InputManager.JumpWasPressed)
        {
            _��Ծ�����ʱ�� = �ƶ�״̬.��Ծ����ʱ��;
            _��Ծ�����ڼ��ͷ���Ծ�� = false;
        }
        // �ͷ�
        if (InputManager.JumpWasReleased)
        {
            if (_��Ծ�����ʱ�� > 0)
            {
                _��Ծ�����ڼ��ͷ���Ծ�� = true;
            }
            if (_��Ծ�� && ��ֱ�ٶ� > 0)
            {
                if (_�Ƿ񳬹���ߵ���ֵ)
                {
                    _�Ƿ񳬹���ߵ���ֵ = false;
                    _�������� = true;
                    _��������ʱ�� = �ƶ�״̬.��Ծȡ������;
                    ��ֱ�ٶ� = 0f;
                }
                else
                {
                    _�������� = true;
                    _���������ʼ�ٶ� = ��ֱ�ٶ�;
                }

            }
        }
        // ��ʼ����Ծ
        if (_��Ծ�����ʱ�� > 0f && !_��Ծ�� && (_�Ƿ񴥵� || _������Ծ��ʱ�� > 0))
        {
            InitiateJump(1);

            if (_��Ծ�����ڼ��ͷ���Ծ��)
            {
                _�������� = true;
                _���������ʼ�ٶ� = ��ֱ�ٶ�;
            }
        }
        // �༶��Ծ
        else if (_��Ծ�����ʱ�� > 0f && _��Ծ�� && _����Ծ���� < �ƶ�״̬.�����Ծ����)
        {
            _�������� = false;
            InitiateJump(1);
        }
        // ������Ծ
        else if (_��Ծ�����ʱ�� > 0f && _������ && _����Ծ���� < �ƶ�״̬.�����Ծ���� - 1)
        {
            InitiateJump(2);
            _�������� = false;
        }
        // ��½���
        if ((_��Ծ�� || _������) && _�Ƿ񴥵� && ��ֱ�ٶ� <= 0f)
        {
            _��Ծ�� = false;
            _������ = false;
            _�������� = false;
            _��������ʱ�� = 0f;
            _�Ƿ񳬹���ߵ���ֵ = false;
            _����Ծ���� = 0;

            ��ֱ�ٶ� = Physics2D.gravity.y;

        }
    }

    private void InitiateJump(int ������Ծ����)
    {
        if (!_��Ծ��)
        {
            _��Ծ�� = true;
        }
        _��Ծ�����ʱ�� = 0f;
        _����Ծ���� += ������Ծ����;
        ��ֱ�ٶ� = �ƶ�״̬.��ʼ��Ծ�ٶ�;
    }

    private void Jump()
    {
        // Ӧ������
        if (_��Ծ��)
        {
            // ��鴥��
            if (_�Ƿ񴥶�)
            {
                _�������� = true;
            }
            // ��������
            if (��ֱ�ٶ� >= 0f)
            {
                // ��ߵ����
                _��ߵ���� = Mathf.InverseLerp(�ƶ�״̬.��ʼ��Ծ�ٶ�, 0f, ��ֱ�ٶ�);
                if (_��ߵ���� > �ƶ�״̬.��ߵ���ֵ)
                {
                    if (!_�Ƿ񳬹���ߵ���ֵ)
                    {
                        _�Ƿ񳬹���ߵ���ֵ = true;
                        _������ߵ���ֵʱ�� = 0f;
                    }

                    if (_�Ƿ񳬹���ߵ���ֵ)
                    {
                        _������ߵ���ֵʱ�� += Time.fixedDeltaTime;
                        if (_������ߵ���ֵʱ�� < �ƶ�״̬.��ߵ�ͣ��ʱ��)
                        {
                            ��ֱ�ٶ� = 0f;
                        }
                        else
                        {
                            ��ֱ�ٶ� = -0.01f;
                        }
                    }
                }
                else
                {
                    ��ֱ�ٶ� += �ƶ�״̬.�������ٶ� * Time.fixedDeltaTime;
                    if (_�Ƿ񳬹���ߵ���ֵ)
                    {
                        _�Ƿ񳬹���ߵ���ֵ = false;
                    } 
                }
            }
            else if (!_��������)
            {
                ��ֱ�ٶ� += �ƶ�״̬.�������ٶ� * �ƶ�״̬.��Ծ�ͷ��������� * Time.fixedDeltaTime;
            }
            else if (��ֱ�ٶ� < 0f)
            {
                if (!_������)
                {
                    _������ = true;
                }
            }
        }
        // ��Ծȡ��
        if(_��������)
        {
            if(_��������ʱ�� >= �ƶ�״̬.��Ծȡ������)
            {
                ��ֱ�ٶ� += �ƶ�״̬.�������ٶ� * �ƶ�״̬.��Ծ�ͷ��������� * Time.fixedDeltaTime;
            }
            else if (_��������ʱ�� <= �ƶ�״̬.��Ծȡ������)
            {
                ��ֱ�ٶ� = Mathf.Lerp(_���������ʼ�ٶ�, 0f, (_��������ʱ��/ �ƶ�״̬.��Ծȡ������));
            }
            _��������ʱ�� += Time.fixedDeltaTime;
        }
        // ��������(����)
        if(!_�Ƿ񴥵� && !_��Ծ��)
        {
            if (!_������)
            {
                _������ = true;
            }

            ��ֱ�ٶ� += �ƶ�״̬.�������ٶ� * Time.fixedDeltaTime;
        }
        // Clamp

        ��ֱ�ٶ� = Mathf.Clamp(��ֱ�ٶ�, -�ƶ�״̬.��������ٶ�, 50f);

        _rb.velocity = new Vector2(_rb.velocity.x, ��ֱ�ٶ�);
    }
    #endregion

    #region ��ײ���
    private void isGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, �ƶ�״̬.�������߳���);

        _������ = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, �ƶ�״̬.�������߳���, �ƶ�״̬.�����);
        if (_������.collider != null)
        {
            _�Ƿ񴥵� = true;
        }
        else
        {
            _�Ƿ񴥵� = false;
        }

        #region Debug������
        if (�ƶ�״̬.Debug������ײ��)
        {

            Color rayColor;
            if (_�Ƿ񴥵�)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastOrigin.x / 2, boxCastOrigin.y), Vector2.down * �ƶ�״̬.�������߳���, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastOrigin.x / 2, boxCastOrigin.y), Vector2.down * �ƶ�״̬.�������߳���, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastOrigin.x / 2, boxCastOrigin.y - �ƶ�״̬.�������߳���), Vector2.right * boxCastSize.x, rayColor);
        }
        #endregion
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * �ƶ�״̬.�������, �ƶ�״̬.�������߳���);

        _������ = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, �ƶ�״̬.�������߳���, �ƶ�״̬.�����);
        if(_������.collider != null)
        {
            _�Ƿ񴥶� = true;
        }
        else
        {
            _�Ƿ񴥶� = false;
        }

        #region Debug
        if(�ƶ�״̬.Debugͷ����ײ��)
        {
            float headWidth = �ƶ�״̬.�������;
             Color rayColor;
            if (_�Ƿ񴥵�)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastOrigin.x / 2, boxCastOrigin.y),
                Vector2.up * �ƶ�״̬.�������߳���, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastOrigin.x / 2, boxCastOrigin.y), 
                Vector2.up * �ƶ�״̬.�������߳���, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastOrigin.x / 2, boxCastOrigin.y - �ƶ�״̬.�������߳���), 
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

    #region ��ʱ��
    private void CountTimer()
    {
        _��Ծ�����ʱ�� -= Time.deltaTime;
        if (!_�Ƿ񴥵�)
        {
            _������Ծ��ʱ�� -= Time.deltaTime;
        }
        else
        {
            _������Ծ��ʱ�� = �ƶ�״̬.������Ծ����ʱ��;
        }
    }
    #endregion
}
