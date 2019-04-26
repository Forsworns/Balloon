using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    public GameObject m_menu;

    // Start is called before the first frame update
    void Start()
    {
        m_menu = this.transform.Find("menu").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pause() {
        Time.timeScale = 0.0f;
        m_menu.SetActive(true);
    }

    public void resume() {
        m_menu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void quit() {
        Application.Quit();
    }
}
