using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NoteSpawner : MonoBehaviour
{

    public Transform[] spawners;  //音符的发射器

    /// <summary>
    /// 音符表示形式（1,1）表示该音符为do，生成时间为1S
    /// </summary>
    private Vector2[] noteOrder;
    public GameObject note;
    private StreamReader songText;

    private const string path = "Assets/SongFiles/";
    public string[] fileName;

    public Dropdown dropSelect;
    public Material fastMaterial;
    private string songLine;
    private string[] songXY;
    private float songX;
    private float songY;
    private int order;
    // bool playing = false;


    //播放txt
    public void StartPlayNoteInText()
    {
        if (!GameManager.Instance.isPlaying)
        {
            ScoreManager.Instance.currentScore = 0;
            GameManager.Instance.noteIndexQueue.Clear();

            string filePath = path + fileName[dropSelect.value] + ".txt";
            songText = File.OpenText(filePath);

            songLine = songText.ReadLine();

            if (!float.TryParse(songLine, out songX))
                Debug.Log("errol");

            noteOrder = new Vector2[(int)songX];

            for (int i = 0; i <= noteOrder.Length - 1; i++)
            {
                songLine = songText.ReadLine();

                songXY = songLine.Split(new char[] { ',' });

                if (!float.TryParse(songXY[0], out songX))
                    Debug.Log("errol");

                if (!float.TryParse(songXY[1], out songY))
                    Debug.Log("errol");


                noteOrder[i].x = songX;
                noteOrder[i].y = songY;
                GameManager.Instance.noteIndexQueue.Enqueue((int)noteOrder[i].x);
            }

            StartCoroutine(NoteInTextCreate());
            order = 0;
        }
        GameManager.Instance.GetFirseNoteIdx();
    }

    IEnumerator NoteInTextCreate()
    {

        while (order < noteOrder.Length)
        {
            GameManager.Instance.isPlaying = true;
            if ((int)noteOrder[order].x != 0)
            {
                if ((int)noteOrder[order].y < 0.5)
                {
                    GameObject g = Instantiate(note, spawners[(int)noteOrder[order].x - 1].position,
                       Quaternion.identity);

                    g.GetComponent<MeshRenderer>().material = fastMaterial;
                    g.name = noteOrder[order].x.ToString();

                    foreach (var item in g.GetComponentsInChildren<MeshRenderer>())
                    {
                        item.material = fastMaterial;
                    }
                }
                else
                {
                    GameObject g = Instantiate(note,
                          spawners[(int)noteOrder[order].x - 1].transform.position,
                          spawners[(int)noteOrder[order].x - 1].transform.rotation);
                    g.name = noteOrder[order].x.ToString();

                }
            }
            yield return new WaitForSeconds(noteOrder[order].y);
            order++;
        }


        GameManager.Instance.isPlaying = false;
        order = 0;
        GameManager.Instance.currenNoteIdx = 0;
        ScoreManager.Instance.UpdateRank(); //更新当前得分
        StopAllCoroutines();
    }
}
