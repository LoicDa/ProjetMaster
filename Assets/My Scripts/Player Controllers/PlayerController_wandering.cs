using System;
using System.Collections;
using UnityEngine;

public class PlayerController_wandering : MonoBehaviour
{
    //FUNCTIONS

   public void textBoxTouched(GameObject touchedTextBox)
    {

        this.touchedTextBox = touchedTextBox;
    }
    public void textBoxExited()
    {
        touchedTextBox = null;
    }


    public void ExecuteAfterTime()
    {
        countdown = 2f;
    }

    public GameObject writingController;
    public GameObject locomotionSystem;
    public GameObject TextBoxPrefab;
    public GameObject cameraObject;

    public GameObject LeftController;
    public GameObject RightController;




    GameObject touchedTextBox;
    GameObject selectedTextBox;




    float countdown;

    // Start is called before the first frame update
    void Start()
    {
        countdown = 0.5f;

        touchedTextBox = null;
        selectedTextBox = null;

    }

    // Update is called once per frame
    void Update()
    {
        //      TIMERS
        countdown -= Time.deltaTime;
        if (countdown < 0f && countdown > -0.5f)
        {
            locomotionSystem.SetActive(true);
            countdown = -1f;
        }

        

        //DONT EVEN CHECK FOR INPUTS IF POINTERS ARE HOVERING
        if (!Global.Instance.isHovering())
        {
            // IF HAS COLLIDED BUT NOT CLICKED YET, CHECK IF IS CLICKING.
            if ((countdown < 0f) && (touchedTextBox != null) && (selectedTextBox == null))
            {
                if ((Input.GetAxis("XRI_Right_Grip") == 1) || (Input.GetAxis("XRI_Left_Grip") == 1)) //  CLICK TO INTERACT 
                {
                    //CHANGE MODE OF THE TEXTBOX
                    touchedTextBox.GetComponent<TextBoxController>().SetReadingMode();
                    selectedTextBox = touchedTextBox;
                    touchedTextBox = null;
                    countdown = 0.5f;
                }
            }

            // INPUT TO PLACE PREFAB
            else if ((countdown < 0f)&&(selectedTextBox == null))//can't place prefab if has textbox opened
            {
                if (Input.GetAxis("XRI_Right_Grip") == 1) // TRIGGER FULLY PRESSED
                {
                    
                    Instantiate(TextBoxPrefab, RightController.transform.position, Quaternion.Euler(0f, RightController.transform.rotation.eulerAngles.y, 0f));
                    countdown = 0.5f;
                }
                if (Input.GetAxis("XRI_Left_Grip") == 1)
                {

                    Instantiate(TextBoxPrefab, LeftController.transform.position, Quaternion.Euler(0f, LeftController.transform.rotation.eulerAngles.y, 0f));
                    countdown = 0.5f;
                }
            }
        }
        

    }

    public void FnBall_touched(Global.FnBall_enum function, GameObject ParentTextBox)
    {
        //WHEN FnBall touched, check function 

        switch (function)
        {
            case Global.FnBall_enum.ChangeTitle:
                //not implemented yet
                if (!ParentTextBox.GetComponent<TextBoxController>().Locked)
                {
                    ActivateWriteMode();
                    Global.Instance.currentTarget = Global.WritingTarget.Title;
                }
                break;
            case Global.FnBall_enum.ChangeText:
                if (!ParentTextBox.GetComponent<TextBoxController>().Locked)
                {
                    ActivateWriteMode();
                    Global.Instance.currentTarget = Global.WritingTarget.Text;
                }
                break;
            case Global.FnBall_enum.Lock:
                if(ParentTextBox.GetComponent<TextBoxController>().Locked)
                    ParentTextBox.GetComponent<TextBoxController>().Unlock();
                else
                    ParentTextBox.GetComponent<TextBoxController>().Lock();
                break;
            case Global.FnBall_enum.Anchor:
                break;
            case Global.FnBall_enum.Delete:
                if (!ParentTextBox.GetComponent<TextBoxController>().Locked)
                {
                    selectedTextBox = null;
                    touchedTextBox = null;
                    Destroy(ParentTextBox);
                }
                
                break;
            case Global.FnBall_enum.ExitRead:
                ActivateIconMode();
                break;
        }
    }

    public void ActivateWriteMode()
    {
        //ouvrir mode ecriture, passer zone de texte en parametre, désactiver ce mode
        //Debug.Log("Mode ecriture");
        if(selectedTextBox != null)
        {
            gameObject.SetActive(false);
            writingController.SetActive(true);
            writingController.GetComponent<PlayerController_writing>().setTextBox(selectedTextBox);
        }
        
    }

    public void ActivateIconMode()
    {
        selectedTextBox.GetComponent<TextBoxController>().SetIconMode();
        selectedTextBox = null;
        countdown = 0.5f;
    }
    
}
