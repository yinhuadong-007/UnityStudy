using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGUI : MonoBehaviour
{
    public float moveSpeed = 0;

    private string m_StrText;
    private bool click;
    private bool _BoolCheck1;
    private int _IntSelectIndex;

    private float value = 0;
    private int min = 0;
    private int max = 100;

    private bool m_pause;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime);
    }

    private void OnGUI()
    {
        // print("OnGUI");
        // TestOnGUI();
        bool last = m_pause;
        if (m_pause == false && GUI.Button(new Rect(0, 0, 100, 30), "Pause"))
        {
            m_pause = true;
        }
        else if (m_pause == true && GUI.Button(new Rect(0, 0, 100, 30), "Continue"))
        {
            m_pause = false;
        }

        if (m_pause != last)
        {
            print("m_pause = " + m_pause);
        }
        if (m_pause)
        {
            moveSpeed = 0;
        }
        else
        {
            moveSpeed = 1;
        }
    }

    private void TestOnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 30), "I am the tabel");
        m_StrText = GUI.TextField(new Rect(0, 50, 100, 30), m_StrText);
        if (GUI.Button(new Rect(0, 100, 100, 30), "Sure"))
        {
            click = true;
        }
        // click = GUI.Button(new Rect(0, 100, 100, 30), "Sure");
        if (click)
        {
            // GUI.Label(new Rect(200, 100, 100, 30), "Sure is on click");
            GUI.Window(0, new Rect(100, 100, 200, 200), AddWindow, "MyWindow");
        }

        bool lastChe = _BoolCheck1;
        //单选框
        _BoolCheck1 = GUI.Toggle(new Rect(0, 260, 100, 50), _BoolCheck1, "zhuangbei");
        if (lastChe != _BoolCheck1)
        {
            Debug.Log("_BoolCheck1 = " + _BoolCheck1);
        }

        int last = _IntSelectIndex;
        //切换框
        _IntSelectIndex = GUI.Toolbar(new Rect(0, 200, 200, 30), _IntSelectIndex, new string[] { "Duty", "Equip", "Peopel" });

        if (last != _IntSelectIndex)
        {
            Debug.Log("_IntSelectIndex = " + _IntSelectIndex);
        }

        float lastSlider = value;
        value = GUI.HorizontalSlider(new Rect(0, 350, 200, 50), value, max, min);
        if (value != lastSlider) Debug.Log("value = " + value);


        ///自动布局 GUILayout :相当于布局一个盒子，盒子使用Rect进行定义，超出范围则不显示。
        GUILayout.BeginArea(new Rect(400, 300, 300, 400));
        GUILayout.Label("One");
        GUILayout.Label("Two");
        GUILayout.Label("Three-sdfjkskfjajksfks几十块的房间爱看到房价肯定会尽快发货时刻及时登记的呼唤我好久开始你的健康发你的就是");
        GUILayout.EndArea();
    }


    private void AddWindow(int winId)
    {
        if (GUI.Button(new Rect(0, 15, 100, 30), "Exit"))
        {
            click = false;
        }
    }
}
