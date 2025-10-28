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

    public bool hasWomen = false;

    public AudioSource exitAudio;
    public AudioSource caughtAudio;
    bool m_HasAudioPlayed = false;

    


    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Women" && !hasWomen)
        {
            hasWomen = true;
            collision.gameObject.GetComponent<Women>().playerTransform = transform;
            Debug.Log("w");
        }
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
            else if(key >=2 && !m_IsPlayerAtExit && hasWomen)
            {
                m_IsPlayerAtExit = true;
                
            }
        }
        
        else if (other.tag == "Fireworks")
        {
            other.gameObject.SetActive(false);
            Debug.Log("f");
        }
    }



    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
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

        if (!m_HasAudioPlayed)
        {
            m_HasAudioPlayed = true;
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
    }

}
