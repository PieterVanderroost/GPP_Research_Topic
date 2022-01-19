using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningHudTxt : MonoBehaviour
{
    [SerializeField]
    Text textObject;

    private float currentShowTime;
    private float maxShowTime = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        textObject.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (textObject.enabled == true)
        {
            currentShowTime += Time.deltaTime;
        }

        if (currentShowTime>=maxShowTime)
        {
            currentShowTime = 0.0f;
            textObject.enabled = false;
        }
    }
    public void EnableShowText()
    {
        textObject.enabled = true;
    }
}
