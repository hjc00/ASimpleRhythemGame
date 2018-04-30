using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Queue<int> noteIndexQueue = new Queue<int>();
    public Queue<GameObject> noteGoQueue = new Queue<GameObject>();

    public int currenNoteIdx = 0; //当前音符的序号
    public GameObject currenNoteGO;

    public bool isRollBacking = false;
    public bool isRecording = false;

    public bool isPlaying = false;

    public static GameManager Instance;

    List<Vector3> rollbackPosList = new List<Vector3>();

    void Start()
    {
        Instance = this;
    }

    public void EnterLearningMode()
    {
        SceneManager.LoadScene(2);
    }

    public void EnterRecordingMode()
    {
        SceneManager.LoadScene(1);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GetFirseNoteIdx()
    {
        if (noteIndexQueue.Count != 0)
            currenNoteIdx = noteIndexQueue.Peek();
    }

    public void GetFirstNoteGo()
    {
        if (noteGoQueue.Count != 0)
            currenNoteGO = noteGoQueue.Peek();
    }

    public void Quit()
    {
        Application.Quit();
    }


}
