using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField, Tooltip("跟随的对象")]
    private Transform m_follower;

    private Vector3 m_disVector;

    // Start is called before the first frame update
    void Start()
    {
        m_disVector = transform.position - m_follower.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = m_follower.position + m_disVector;

        transform.position = pos;
    }
}
