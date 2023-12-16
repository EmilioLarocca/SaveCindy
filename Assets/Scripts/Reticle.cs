using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    private RectTransform reticle;

    private float restingSize = 75;
    private float maxSide = 500;
    private float speed = 10;
    private float currentSize;

    private void Start()
    {
        reticle = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isMoving)
        {
            currentSize = Mathf.Lerp(currentSize, maxSide, Time.deltaTime * speed);
        } else
        {
            currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * speed);
        }

        reticle.sizeDelta = new Vector2(currentSize, currentSize);
    }

    bool isMoving {
        get {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))

                return true;
            else 
                return false;
        }
    }
}
