using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed;
    public float destoryTime;
    public bool isCurrent = false;
    void Start()
    {
        Destroy(gameObject, destoryTime);
    }
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
