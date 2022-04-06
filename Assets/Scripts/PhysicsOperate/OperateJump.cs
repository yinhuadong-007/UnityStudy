using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 跳跃功能脚本
/// </summary>
public class OperateJump : MonoBehaviour
{
    [SerializeField, Tooltip("跳跃的推力")]
    private float m_thrust;

    [SerializeField, Tooltip("跳跃键")]
    private KeyCode keyCode = KeyCode.Space;


    private Rigidbody rb;
    private bool isJump = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isJump && Input.GetKeyDown(keyCode))
        {
            Debug.Log("跳跃");
            isJump = true;
            rb.AddForce(transform.up * m_thrust, ForceMode.Impulse);
        }

        if (isJump && rb.velocity.y == 0)
        {
            isJump = false;
        }
    }
}
