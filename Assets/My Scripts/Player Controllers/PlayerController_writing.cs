using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController_writing : MonoBehaviour
{
    //  FONCTIONS
    

    void QuitWriting(GameObject parentTextBox)
    {
        if (Global.Instance.usingPizzaText)
            pizzaText.Deactivate();
        else
            roundBoard.Deactivate();
        parentTextBox.GetComponent<TextBoxController>().SetReadingMode();

        wanderingController.GetComponent<PlayerController_wandering>().ExecuteAfterTime();
        gameObject.SetActive(false);
        wanderingController.SetActive(true);
        
    }
    

    public void collisionDetected(Global.FnBall_enum colliderName, GameObject parentTextBox)
    {
        if (Global.Instance.usingPizzaText)
            pizzaText.ResetCountdown();
        else
            roundBoard.ResetCountdown();
        countdown = 0.5f;

        switch (colliderName)
        {
            case Global.FnBall_enum.Copy:
                if (!parentTextBox.GetComponent<TextBoxController>().GetCursorMode()) //is in  select mode (false) -> copy 
                {
                    Debug.Log("COPIER TEXTE");
                    affichText.Copy();
                    break;
                }
                else                                    //has text in paperclip -> paste
                {
                    Debug.Log("COLLER TEXTE");
                    affichText.Paste();
                    break;
                }
            case Global.FnBall_enum.Cursors:
                Debug.Log("MODE CURSEUR");
                affichText.ChangeSelectMode();
                break;
            case Global.FnBall_enum.Save:
                Debug.Log("SAUVER EDITION TEXTE");
                affichText.SaveText(); //set text to actual text (w/o cursors)
                QuitWriting(parentTextBox);
                break;
            case Global.FnBall_enum.ExitWrite:
                Debug.Log("QUITTER SANS SAUVER");
                affichText.UndoChange();
                QuitWriting(parentTextBox);
                break;
            default:
                Debug.Log("I don't get it");
                break;
        }
    }

    public void setTextBox(GameObject textBox)
    {
        if (Global.Instance.usingPizzaText)
            pizzaText.Activate();
        else
            roundBoard.Activate();
        locomotionSystem.SetActive(false);

        affichText = textBox.GetComponent<TextBoxController>(); //récup référence zone de texte
        affichText.SetWritingMode();


    }

    public void AddChar(char newInput)
    {
        if (newInput == '<')
        {
            affichText.DeleteChar();
            countdown = 0.5f;
            nbErreur++;
            saisieTotale += newInput;
        }
        else if (newInput == '>')
        {
            affichText.AddLineBreak();
            countdown = 0.5f;
            nbErreur++;
            saisieTotale += newInput;
        }
        else
        {
            //ADD INPUT TO TEXT
            affichText.AddChar(newInput);

            //VARIABLE FOR TESTING
            saisieTotale += newInput;
            hasStarted = true;
            nbClic++;
        }

        Global.Instance.saveInput(newInput); //save every input
        
    }


    //  VARIABLES

    public GameObject wanderingController;
    public GameObject locomotionSystem;
    public TextBoxController affichText;

    //      INPUT METHODS
    public RoundBoard roundBoard;
    public pizzaText pizzaText;


    
    float countdown;


    // ---- variables de test pour stats ---- \\
    
    public bool testing = true;
    bool hasStarted = false;
    float tpsTotal = 0;
    int nbClic = 0;
    int nbErreur = 0;
    public string objectif = "TEST SENTENCE";
    string saisieTotale = "";
    
    // ---- \\

    // Start is called before the first frame update
    void Start()
    {
        countdown = 0.5f;
    }




    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;

        //          CONTROLLER INPUTS
        
        if (countdown < 0f) //si le décompte est écoulé
        {
            /*
            if ((((Input.GetAxis("XRI_Left_Trigger") == 1) || (Input.GetAxis("XRI_Right_Trigger") == 1)) && (!Global.Instance.usingPizzaText))
                || (((Input.GetAxis("XRI_Left_Grip") == 1) || (Input.GetAxis("XRI_Right_Grip") == 1)) && (Global.Instance.usingPizzaText))) // DELETE LAST CHAR
            {
                affichText.DeleteChar();
                countdown = 0.5f;
                nbErreur++;
                saisieTotale += '<';  
            }
            */
            
                if ((Input.GetAxis("XRI_Left_Secondary2DAxis_Horizontal") != 0f)) // MOVE CURSOR 1
                {
                    affichText.MoveFirstCursor(Input.GetAxis("XRI_Left_Secondary2DAxis_Horizontal"), Input.GetAxis("XRI_Left_Secondary2DAxis_Vertical"));
                    countdown = 0.5f;
                }

                if ((Input.GetAxis("XRI_Right_Secondary2DAxis_Horizontal") != 0f))               //MOVE CURSOR 2
                {
                    affichText.MoveSecondCursor(Input.GetAxis("XRI_Right_Secondary2DAxis_Horizontal"), Input.GetAxis("XRI_Right_Secondary2DAxis_Vertical"));
                    countdown = 0.5f;
                }


        }


        //UPDATE DISPLAY
        //affichText.SetText(tManager.afficher(Time.fixedTime));
    }
}
