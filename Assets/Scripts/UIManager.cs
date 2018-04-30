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
        tips.transform.DOScale(new Vector3(2, 2, 2),0.5f);
        StartCoroutine(HideTips());
    }

    IEnumerator HideTips()
    {
        yield return new WaitForSeconds(0.5f);
        tips.text = "";
        tips.transform.localScale = new Vector3(0, 0, 0);
    }
}
