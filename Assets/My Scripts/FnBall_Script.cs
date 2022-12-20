using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FnBall_Script : MonoBehaviour
{
    public Global.FnBall_enum function; //what the ball does when touched

    public GameObject ParentTextBox;

    Material normalMat;
    Material clickedMat;

    bool isActivated;

    float countdown;

    public Global.FnBall_enum GetFunction()
    {
        if (isActivated)
            return function;
        else
            return Global.FnBall_enum.Null;
    }

    //CHANGE STATE AND MATERIAL WHEN COLLIDED
    public void hasCollided()
    {
        countdown = 0.5f;
        GetComponent<Renderer>().material = clickedMat;
        Deactivate();
    }

    public void Deactivate()
    {
        isActivated = false;
    }

    public void Activate()
    {
        isActivated = true;
    }

    void Start()
    {
        isActivated = true;

        //MUST PICK MATERIALS ACCORDING TO ENUM CHOICE
        switch (function)
        {
            case Global.FnBall_enum.ChangeTitle:
                normalMat = Resources.Load("B EDIT 1", typeof(Material)) as Material;
                clickedMat = Resources.Load("B EDIT 2", typeof(Material)) as Material;
                break;
            case Global.FnBall_enum.ChangeText:
                normalMat = Resources.Load("B EDIT 1", typeof(Material)) as Material;
                clickedMat = Resources.Load("B EDIT 2", typeof(Material)) as Material;
                break;
            case Global.FnBall_enum.Lock:
                normalMat = Resources.Load("B LOCK 1", typeof(Material)) as Material;
                clickedMat = Resources.Load("B LOCK 2", typeof(Material)) as Material;
                break;
            case Global.FnBall_enum.Delete:
                normalMat = Resources.Load("B DELETE 1", typeof(Material)) as Material;
                clickedMat = Resources.Load("B DELETE 2", typeof(Material)) as Material;
                break;
            case Global.FnBall_enum.Anchor:
                normalMat = Resources.Load("B ANCHOR 1", typeof(Material)) as Material;
                clickedMat = Resources.Load("B ANCHOR 2", typeof(Material)) as Material;
                break;
            case Global.FnBall_enum.ExitRead:
                normalMat = Resources.Load("B EXIT 1", typeof(Material)) as Material;
                clickedMat = Resources.Load("B EXIT 2", typeof(Material)) as Material;
                break;
            case Global.FnBall_enum.Copy:
                normalMat = Resources.Load("B COPY 1", typeof(Material)) as Material;
                clickedMat = Resources.Load("B COPY 2", typeof(Material)) as Material;
                break;
            case Global.FnBall_enum.Cursors:
                normalMat = Resources.Load("B SELECT 1", typeof(Material)) as Material;
                clickedMat = Resources.Load("B SELECT 2", typeof(Material)) as Material;
                break;
            case Global.FnBall_enum.LayoutChange:
                normalMat = Resources.Load("B LAYOUT 1", typeof(Material)) as Material;
                clickedMat = Resources.Load("B LAYOUT 2", typeof(Material)) as Material;
                break;
            case Global.FnBall_enum.Save:
                normalMat = Resources.Load("B SAVE 1", typeof(Material)) as Material;
                clickedMat = Resources.Load("B SAVE 2", typeof(Material)) as Material;
                break;
            case Global.FnBall_enum.ExitWrite:
                normalMat = Resources.Load("B EXIT 1", typeof(Material)) as Material;
                clickedMat = Resources.Load("B EXIT 2", typeof(Material)) as Material;
                break;
            default:
                normalMat = Resources.Load("B NOFUNCTION", typeof(Material)) as Material;
                clickedMat = Resources.Load("B NOFUNCTION", typeof(Material)) as Material;
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if ((countdown < 0)&&( countdown > -0.5))
        {
            GetComponent<Renderer>().material = normalMat;
            Activate();
        }
    }
}
