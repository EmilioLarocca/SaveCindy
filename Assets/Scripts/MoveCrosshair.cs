using UnityEngine;
public class MoveCrosshair : MonoBehaviour
{
    public Camera cam;

    private float horizontalInput;
    private float verticalInput;

    bool mouseMoves;

    private float speed = 2000;
    private float fieldOfView = 80;
    private float xRange = 960;
    private float yRange = 540;


    void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {

        // player controller
        if (Input.GetAxis("Mouse X") != 0)
        {
            mouseMoves = true;
            transform.position = Input.mousePosition;
        } else if (Input.GetAxis("Horizontal") != 0 && !mouseMoves || 
                    Input.GetAxis("Vertical") != 0 && !mouseMoves)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);
            verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector3.up * Time.deltaTime * speed * verticalInput);

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed *= 2;
            } if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed /= 2;
            }  
        } else
        {
            mouseMoves = false;
        }

        // force on screen
        if (transform.localPosition.x < -xRange)
        {
            transform.localPosition = new Vector3(-xRange, transform.localPosition.y, transform.localPosition.z);
        } if (transform.localPosition.x > xRange)
        {
            transform.localPosition = new Vector3(xRange, transform.localPosition.y, transform.localPosition.z);
        }
        if (transform.localPosition.y < -yRange)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -yRange, transform.localPosition.z);
        } if (transform.localPosition.y > yRange)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, yRange, transform.localPosition.z);
        }

        // restict cam
        if (Input.GetKeyDown(KeyCode.C))
        {
            cam.fieldOfView = fieldOfView / 2;
        } if (Input.GetKeyUp(KeyCode.C))
        {
            cam.fieldOfView = 80;
        }

        // hide mouse pointer
        if (Input.GetKeyDown(KeyCode.T))
        {
            Cursor.visible = true;
        } if (Input.GetKeyDown(KeyCode.Y))
        {
            Cursor.visible = false;
        }
    }
}
