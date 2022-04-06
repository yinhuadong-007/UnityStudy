using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移动基础控制脚本
/// </summary>
public class OperateControl : MonoBehaviour
{
    //写入编辑器的量
    [SerializeField, Tooltip("移动速度")]
    private float m_moveSpeed;

    [SerializeField, Tooltip("前后左右绑定的键")]
    private KeyCode[] keyCode = { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };


    //私有量
    private Rigidbody m_rb;

    private Vector3 m_direction = Vector3.zero;

    [HideInInspector]
    public bool useMoveSpeed = true;


    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!useMoveSpeed)
        {
            return;
        }

        m_direction = Vector3.zero;
        if (Input.GetKey(keyCode[0]))
        {
            m_direction.z = 1;
        }
        if (Input.GetKey(keyCode[1]))
        {
            m_direction.z = -1;
        }
        if (Input.GetKey(keyCode[2]))
        {
            m_direction.x = -1;
        }
        if (Input.GetKey(keyCode[3]))
        {
            m_direction.x = 1;
        }

        m_rb.velocity = m_direction * m_moveSpeed;

        Debug.Log("m_rb.velocity = " + m_rb.velocity);

    }
}
