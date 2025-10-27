using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public uint key = 0;
    public Text keyUI;

    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public CanvasGroup caughtBackgroundImageCanvasGroup;

    bool m_IsPlayerAtExit = false;
    float m_Timer;
    bool m_IsPlayerCaught;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Key")
        {
            key++;
            Debug.Log("Key found");
            Destroy(other.gameObject);
            keyUI.text = "Keys: " + key.ToString() + "/2";
        }
        else if (other.tag == "Enemy")
        {
            m_IsPlayerCaught = true;
        }
        else if (other.tag == "Goal")
        {
            if (key < 2)
            {
                Debug.Log("Plese Find Key");
            }
            else if(key >=2 && !m_IsPlayerAtExit)
            {
                m_IsPlayerAtExit = true;
                
            }
        }
    }

    void RestartGame()
    {

        m_Timer += Time.deltaTime;

        exitBackgroundImageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            Application.Quit();
        }
    }

    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart)
    {
        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                Application.Quit();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup, false);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true);
        }
    }

}
