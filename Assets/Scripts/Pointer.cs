using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
    public Camera cam;

    private float horizontalInput;
    private float verticalInput;

    bool mouseMoves;

    private float speed = 2000;
    private float xRange = 960;
    private float yRange = 540;

    private void Start() 
    {
        Cursor.visible = false;
    }

    void Update()
    {
        // Player controller
        if (Input.GetAxis("Mouse X") != 0)
        {
            mouseMoves = true;
            transform.position = Input.mousePosition;
        }
        else if (Input.GetAxis("Horizontal") != 0 && !mouseMoves ||
                 Input.GetAxis("Vertical") != 0 && !mouseMoves)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);
            verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector3.up * Time.deltaTime * speed * verticalInput);
        }
        else
        {
            mouseMoves = false;
        }

        // Force on screen
        if (transform.localPosition.x < -xRange)
        {
            transform.localPosition = new Vector3(-xRange, transform.localPosition.y, transform.localPosition.z);
        }
        if (transform.localPosition.x > xRange)
        {
            transform.localPosition = new Vector3(xRange, transform.localPosition.y, transform.localPosition.z);
        }
        if (transform.localPosition.y < -yRange)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -yRange, transform.localPosition.z);
        }
        if (transform.localPosition.y > yRange)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, yRange, transform.localPosition.z);
        }
    }
}
