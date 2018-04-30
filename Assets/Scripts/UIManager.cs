using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    public Text tips;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void UpdateTips(string content)
    {
        tips.text = content;
        StartCoroutine(HideTips());
    }

    IEnumerator HideTips()
    {
        yield return new WaitForSeconds(0.2f);
        tips.text = "";
    }
}
