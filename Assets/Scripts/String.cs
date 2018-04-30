using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class String : MonoBehaviour
{
    public KeyCode keyCode;

    public AudioSource audioSrc;
    private Color originColor;
    public bool isPressed;

    public GameObject currenNoteGO;
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        originColor = this.gameObject.GetComponent<MeshRenderer>().material.color;
        isPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            isPressed = true;
            PlaySound();
        }
        else if (Input.GetKeyUp(keyCode))
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = originColor;
            isPressed = false;
        }


        if (GameManager.Instance.isPlaying)
        {
            currenNoteGO = GameObject.Find(GameManager.Instance.currenNoteIdx.ToString());
        }

    }

    public void PlaySound()
    {
        audioSrc.Play();
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;


        if (this.tag == GameManager.Instance.currenNoteIdx.ToString())  // 敲对
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
            if (currenNoteGO)
            {
                float d = 0;
                d = Vector3.Distance(this.transform.position, currenNoteGO.transform.position);
                CheckDistance(d);
                ChangeCurrentNote();
                Destroy(currenNoteGO);
            }
            else
            {
                UIManager.Instance.UpdateTips("too early + 5!");
                ScoreManager.Instance.AddScore(5);
            }
        }
        else   //敲错
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            if (GameManager.Instance.isPlaying)
            {
                UIManager.Instance.UpdateTips("Wrong - 5!");
                ScoreManager.Instance.MinusScore(10);
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Note")
        {
            //miss
            ChangeCurrentNote();
            Destroy(other.gameObject);
            ScoreManager.Instance.MinusScore(10);
        }
    }

    void ChangeCurrentNote()
    {
        if (GameManager.Instance.noteIndexQueue.Count != 0)
        {
            GameManager.Instance.noteIndexQueue.Dequeue();
            GameManager.Instance.GetFirseNoteIdx();
        }
    }

    void CheckDistance(float _d)
    {
        if (_d >= 0.0 && _d < 0.5)
        {
            UIManager.Instance.UpdateTips("Perfect + 25!");
            ScoreManager.Instance.AddScore(25);
            Debug.Log("perfect");
        }
        else if (_d >= 0.5 && _d < 1.0)
        {
            UIManager.Instance.UpdateTips("Great + 20!");
            ScoreManager.Instance.AddScore(20);
            Debug.Log("Great");
        }
        else if (_d >= 1.0 && _d < 1.6)
        {
            UIManager.Instance.UpdateTips("Good + 15!");
            ScoreManager.Instance.AddScore(15);
            Debug.Log("Good");
        }
        else if (_d >= 1.6)
        {
            UIManager.Instance.UpdateTips("Too early + 5!");
            ScoreManager.Instance.AddScore(5);
            Debug.Log("too early");
        }
    }

}
