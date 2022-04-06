using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 冲刺功能 脚本
/// </summary>
public class OperateDash : MonoBehaviour
{
    //写入编辑器的量
    [SerializeField, Tooltip("移动控制器")]
    private OperateControl m_moveControl;

    [SerializeField, Tooltip("冲刺的速度")]
    private float m_dashSpeed;

    [SerializeField, Tooltip("冲刺的时间")]
    private float m_dashTime;

    [SerializeField, Tooltip("绑定的冲刺键")]
    private KeyCode m_keyCode = KeyCode.L;


    //私有量
    private Rigidbody m_rb;

    private float m_resumeTime;//恢复时间
    private bool m_isDash = false;//是否正在冲刺
    private bool m_useGravity;//记录重力使用情况
    private Vector3 m_velocity;


    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isDash && Input.GetKeyDown(m_keyCode))
        {
            Debug.Log("冲刺");
            m_useGravity = m_rb.useGravity;
            m_velocity = m_rb.velocity;

            m_isDash = true;
            m_moveControl.useMoveSpeed = false;
            m_resumeTime = Time.time + m_dashTime;
            m_rb.useGravity = false;
            m_rb.velocity = Vector3.forward * m_dashSpeed;
        }

        //冲刺时间已到,恢复原先状态
        if (m_isDash && Time.time >= m_resumeTime)
        {
            m_isDash = false;
            m_moveControl.useMoveSpeed = true;
            m_rb.useGravity = m_useGravity;
            m_rb.velocity = m_velocity;
        }
    }

}
