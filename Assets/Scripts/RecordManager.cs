using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RecordManager : MonoBehaviour
{
    public InputField inputField;
    public GameObject saveDialog;
    public GameObject fileDialog;
    public Dropdown fileDropDown;

    public GameObject[] strings;

    public Text tips;

    string tmpFileName;
    string tmpPath;   //临时存放回放数据的文件

    StreamWriter sw;
    float timer = 0;

    public bool isRecord = false;

    void Start()
    {
        tmpFileName = "tmp.rep";
        tmpPath = Application.dataPath + "/RecodingFile/" + tmpFileName;
        if (File.Exists(tmpPath))
        {
            File.Delete(tmpPath);
        }
    }

    void Update()
    {
        if (isRecord)
        {
            // string pos = Handle1.transform.position.x + "," + Handle1.transform.position.y + "," + Handle1.transform.position.z;
            CheckKeyPressed();
            //sw.WriteLine(pos);
            timer += Time.deltaTime;

            if (timer > 60.0f)
            {
                sw.Close();
                isRecord = false;
                timer = 0;
            }
        }
        else
        {
            if (sw != null)
            {
                sw.Close();
            }
            return;
        }
    }

    private void CheckKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            sw.WriteLine(1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            sw.WriteLine(2);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            sw.WriteLine(3);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            sw.WriteLine(4);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            sw.WriteLine(5);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            sw.WriteLine(6);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            sw.WriteLine(7);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            sw.WriteLine(8);
        }
        else
        {
            sw.WriteLine(0);
        }
    }

    public void ActiveRecord()
    {
        sw = new StreamWriter(tmpPath);
        isRecord = true;
    }

    public void AcitiveRollBack()   //试听
    {
        if (File.Exists(tmpPath))  //若存在临时录制好的文件就播放录制好的文件
        {
            isRecord = false;
            StartCoroutine(RollBack(tmpPath));
        }
    }

    public void ActiveSaveDialog()
    {
        if (!File.Exists(tmpPath))
        {
            Debug.Log("你还没进行录制！");
            StartCoroutine(ShowTipsUI("你还没进行录制"));
        }
        else
        {
            saveDialog.gameObject.SetActive(true);
        }
    }

    public void Save()
    {
        if (sw != null)
        {
            sw.Close();
        }

        if (string.IsNullOrEmpty(inputField.text))
        {
            Debug.Log("请输入文件名！");
            StartCoroutine(ShowTipsUI("请输入文件名！"));
        }
        else
        {
            string fileName = inputField.text + ".rep";
            string path = Application.dataPath + "/RecodingFile/" + fileName;

            if (File.Exists(path))
            {
                Debug.Log("文件名重复！");
                StartCoroutine(ShowTipsUI("文件名重复！"));
            }

            else
            {
                File.Move(tmpPath, path);
                //把临时文件名重命名为输入框的名字
                StartCoroutine(ShowTipsUI("保存成功！"));
                Debug.Log("保存成功！");
                saveDialog.SetActive(false);
            }
        }
    }


    IEnumerator RollBack(string _path)
    {
        if (sw != null)
            sw.Close();
        StreamReader sr = new StreamReader(_path);

        while (sr.Peek() >= 0)
        {
            //string[] pos = sr.ReadLine().Split(new char[] { ',' });
            //Handle2.transform.position = new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
            int stringToPlay = int.Parse(sr.ReadLine());
            if (stringToPlay != 0)
            {
                strings[stringToPlay - 1].GetComponent<String>().audioSrc.Play();
            }
            yield return new WaitForEndOfFrame();
        }

        sr.Close();
    }

    public void StopRecord()
    {
        isRecord = false;
        if (sw != null)
        {
            sw.Close();
        }
    }

    public void ActiveFileDialog()
    {
        fileDialog.SetActive(true);
        // Time.timeScale = 0;
        fileDropDown.options.Clear();
        //更新dropdown内容
        string filePath = Application.dataPath + "/RecodingFile";

        DirectoryInfo folder = new DirectoryInfo(filePath);

        foreach (FileInfo file in folder.GetFiles("*.rep"))
        {
            Dropdown.OptionData op = new Dropdown.OptionData();
            op.text = file.Name;
            fileDropDown.options.Add(op);
        }
    }

    public void PlayFile()  //播放已经存储的录制的文件
    {
        if (!string.IsNullOrEmpty(fileDropDown.captionText.text))
        {
            fileDialog.SetActive(false);
            //Time.timeScale = 1;

            string repFilename = fileDropDown.captionText.text;
            string path = Application.dataPath + "/RecodingFile/" + repFilename;

            StartCoroutine(RollBack(path));
        }
    }

    IEnumerator ShowTipsUI(string s)
    {
        tips.gameObject.SetActive(true);
        tips.text = s;
        yield return new WaitForSeconds(0.5f);
        tips.gameObject.SetActive(false);
    }
}
