using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControls : MonoBehaviour
{

    public GameObject Button;
    public GameObject Joystick;

    bool buttonHeld = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown("space"))
        {
            Button.transform.Translate(0, -0.02f, 0);
        }
        if (Input.GetKeyUp("space"))
        {
            Button.transform.Translate(0, 0.02f, 0);
        }

        if (Input.GetKey("d"))
        {
            //Joystick.transform.rotation = Quaternion.Euler(0,0,45);
            //Vector3 leftDir = Vector3.RotateTowards(Joystick.transform.eulerAngles, new Vector3(0, 0, -45), 2, 0.0f);
            //Joystick.transform.rotation = Quaternion.LookRotation(leftDir);

            Vector3 currentAngle = Joystick.transform.eulerAngles;

            Debug.Log(currentAngle.z.ToString());

            Joystick.transform.eulerAngles = new Vector3(
                Mathf.LerpAngle(currentAngle.x, currentAngle.x, Time.deltaTime),
                Mathf.LerpAngle(currentAngle.y, currentAngle.y, Time.deltaTime),
                Mathf.LerpAngle(currentAngle.z, 45f, Time.deltaTime));

        }

        if (Input.GetKeyUp("d"))
        {
            Joystick.transform.rotation = Quaternion.Euler(0, 0, 0);
        }



        if (Input.GetKey("a"))
        {
            Vector3 currentAngle = Joystick.transform.eulerAngles;

            Joystick.transform.eulerAngles = new Vector3(
                Mathf.LerpAngle(currentAngle.x, currentAngle.x, Time.deltaTime),
                Mathf.LerpAngle(currentAngle.y, currentAngle.y, Time.deltaTime),
                Mathf.LerpAngle(currentAngle.z, -45f, Time.deltaTime));
        }
        if (Input.GetKeyUp("a"))
        {
            Joystick.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
