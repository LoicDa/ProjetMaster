using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum TextBoxMode { Icon, Read, Write};
public class TextBoxController : MonoBehaviour
{

    public Material normalMat;
    public Material highlightMat;


    public GameObject IconObjects;
    public GameObject Display;
    public GameObject WritingBalls;
    public GameObject ReadingBalls;

    public int NbColumns = 29;
    public int NbLines = 17;
    public int LineHeight = 80;

    public bool Locked = false;

    public textManager tManager;
    public textManager titleManager;

    public Text titleIcon;
    public Text titleRead;
    public TextMeshProUGUI textBody;

    public string textBeforeEdit;
    public string titleBeforeEdit;

    public string testObjective;

    public Renderer mesh;

    TextBoxMode mode;

    float countdown;



    public void HasCollided()
    {
        mesh.GetComponent<Renderer>().material = highlightMat;
    }

    public void ExitCollision()
    {
        mesh.GetComponent<Renderer>().material = normalMat;
    }

    public void Lock()
    {
        Locked = true;
    }
    public void Unlock()
    {
        Locked = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        textBeforeEdit = textBody.text;
        titleBeforeEdit = titleIcon.text;
        tManager = new textManager();
        tManager.loadText(textBeforeEdit);
        titleManager = new textManager();
        titleManager.loadText(titleIcon.text);

}

// Update is called once per frame
void Update()
    {
        if(mode == TextBoxMode.Write)
        {
            countdown += Time.deltaTime;

            if (countdown >= 0.5f)
            {
                if (Global.Instance.currentTarget == Global.WritingTarget.Text)
                {
                    textBody.text = tManager.Display(true);
                }
                else
                {
                    titleRead.text = titleManager.Display(true);
                }

                if (countdown >= 1f)
                {
                    countdown = 0f;
                }
            }
            else
            {
                if (Global.Instance.currentTarget == Global.WritingTarget.Text)
                {
                    textBody.text = tManager.Display(false);
                }
                else
                {
                    titleRead.text = titleManager.Display(false);
                }
            }
        }
    }

    void SetTitle(string newTitle)
    {
        string editedTitle = newTitle;
        if (newTitle.Length > 26)
        {
            editedTitle = newTitle.Substring(0, 23) + "...";
        }

        titleIcon.text = editedTitle;
        titleRead.text = editedTitle;
    }

    public void AddChar(char newChar)
    {
        if(Global.Instance.currentTarget == Global.WritingTarget.Text)
        {
            tManager.addChar(newChar);
            SetText(tManager.getSaisie());
        }
        else
        {
            titleManager.addChar(newChar);
            SetTitle(tManager.getSaisie());
        }
        
        
    }

    void SetText(string text) //permet d'éditer le texte
    {
        textBody.text = text;

        //CHECK IF TEXT IS TO LONG -> INCREASE HEIGHT
        //17 lines before needs to scroll
        //29 chars wide (CAPS ARE WIDER)
        //Split text at linebreaks then count length of each substring

        string[] wrappingLines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        int lineCount = wrappingLines.Length;


        foreach (string line in wrappingLines)
        {
            lineCount += line.Length/NbColumns; //will add if string is to long
        }

        int currentLineNb = (int)textBody.rectTransform.rect.height / LineHeight;

        textBody.rectTransform.sizeDelta = new Vector2(1142, (lineCount)*LineHeight);

    }

    public void SaveText() //permet d'éditer le texte
    {
        
        if (Global.Instance.currentTarget == Global.WritingTarget.Text)
        {
            textBeforeEdit = tManager.getSaisie(); //overwrite previous text and update one last time
            SetText(textBeforeEdit);
            tManager.loadText(textBeforeEdit);
            if (testObjective != "")
                Global.Instance.save(tManager.getSaisie(), "Success");
        }
        else
        {
            titleBeforeEdit = titleManager.getSaisie(); //overwrite previous text and update one last time
            SetTitle(titleBeforeEdit);
            titleManager.loadText(titleBeforeEdit);
            if (testObjective != "")
                Global.Instance.save(titleManager.getSaisie(), "Success");
        }
    }

    public void UndoChange()
    {
        //ignore changes made be reverting to what it was before
        if (Global.Instance.currentTarget == Global.WritingTarget.Text)
        {
            SetText(textBeforeEdit);
            tManager.loadText(textBeforeEdit);
            if (testObjective != "")
                Global.Instance.save(tManager.getSaisie(), "Abandonned");
        }
        else
        {
            SetTitle(titleBeforeEdit);
            titleManager.loadText(titleBeforeEdit);
            if (testObjective != "")
                Global.Instance.save(titleManager.getSaisie(), "Abandonned");
        }
    }


    public string GetText() //renvoie son texte
    {
        return tManager.getSaisie();
    }


    public void SetWritingMode()
    {
        if(testObjective != "")
            Global.Instance.StartExercise(testObjective, gameObject); //start timer and clear arrays
        ReadingBalls.SetActive(false);
        WritingBalls.SetActive(true);
        mode = TextBoxMode.Write;
    }
    public void SetReadingMode()
    {
        IconObjects.SetActive(false);
        WritingBalls.SetActive(false);
        Display.SetActive(true);  
        ReadingBalls.SetActive(true);
        mode = TextBoxMode.Read;
    }
    public void SetIconMode()
    {
        Display.SetActive(false);
        ReadingBalls.SetActive(false);
        IconObjects.SetActive(true);
        mode = TextBoxMode.Icon;
    }

    public TextBoxMode GetMode()
    {
        return mode;
    }

    public void DeleteChar()
    {
        if (Global.Instance.currentTarget == Global.WritingTarget.Text)
        {
            tManager.removeChar();
            SetText(tManager.getSaisie());
        }
        else
        {
            titleManager.removeChar();
            SetTitle(titleManager.getSaisie());
        }
    }

    public void MoveFirstCursor(float valueX, float valueY)
    {
        if (Global.Instance.currentTarget == Global.WritingTarget.Text)
        {
            tManager.deplacerCurseurDebut(valueX, valueY);
        }
        else
        {
            titleManager.deplacerCurseurDebut(valueX, valueY);
        }
    }
    public void MoveSecondCursor(float valueX, float valueY )
    {
        if (Global.Instance.currentTarget == Global.WritingTarget.Text)
        {
            tManager.deplacerCurseurFin(valueX, valueY);
        }
        else
        {
            titleManager.deplacerCurseurFin(valueX, valueY);
        }
    }

    public void Copy()
    {
        if (Global.Instance.currentTarget == Global.WritingTarget.Text)
        {
            tManager.copieSelection();
        }
        else
        {
            titleManager.copieSelection();
        }
    }

    public void Paste()
    {
        if (Global.Instance.currentTarget == Global.WritingTarget.Text)
        {
            tManager.colleSelection();
        }
        else
        {
            titleManager.colleSelection();
        }
    }

    public void AddLineBreak()
    {
        if (Global.Instance.currentTarget == Global.WritingTarget.Text)
        {
            tManager.addLineBreak();
        }
        else
        {
            titleManager.addLineBreak();
        }
        
    }

    public void ChangeSelectMode()
    {
        if (Global.Instance.currentTarget == Global.WritingTarget.Text)
        {
            tManager.changeMode();
        }
        else
        {
            titleManager.changeMode();
        }
    }

    public bool GetCursorMode()
    {
        if (Global.Instance.currentTarget == Global.WritingTarget.Text)
        {
            return tManager.getCursorMode();
        }
        else
        {
            return titleManager.getCursorMode(); 
        }
        
    }

}



public class textManager
{
    bool OneCursorMode;

    string text;

    string textWithCursors;
    //int pos_cursor;

    //string m_objectif;

    int deb_selection;
    int fin_selection;

    string selection;

    string highLightStart = "<mark=#1FEFF5AF>";
    string highLightEnd = "</mark>";

    public textManager()
    {


        text = "";
        textWithCursors = "";
        selection = "";
        //pos_cursor = 0;
        fin_selection = 0;
        deb_selection = 0;
        OneCursorMode = true;
    }


    public void changeMode()
    {
        if (OneCursorMode)
            OneCursorMode = false;
        else
            OneCursorMode = true;
    }

    public void loadText(string newText)
    {
        text = newText;
        selection = "";
        fin_selection = 0;
        deb_selection = 0;
    }

    public void addChar(char newChar)
    {
        text = text.Insert(deb_selection, Char.ToString(newChar));
        deb_selection += 1;
        //Debug.Log(pos_curseur);
    }

    public void removeChar()
    {
        if ((deb_selection > 0) && (OneCursorMode))
        {
            text = text.Remove(deb_selection - 1, 1);
            deb_selection -= 1;
        }

    }

    public void addLineBreak()
    {
        Debug.Log("Line Break ?");
        if (OneCursorMode)
        {
            text = text.Insert(deb_selection, Environment.NewLine);
        }
    }


    public void deplacerCurseurDebut(float moveX, float moveY)
    {
        if (Math.Abs(moveX) > 0.2) //deadzone at 
        {
            if (moveX > 0)
            {
                deb_selection += 1;
            }
            if (moveX < 0)
            {
                deb_selection -= 1;
            }
        }
        
        /*
        if(Math.Abs(moveY) > 0.2)
        {
            int move = 0;
            if (moveY > 0)
            {
                move = 27;
            }
            if (moveY < 0)
            {
                move = -27;
            }

            //one line is 27 char or one linebreak, index mod 27
            string[] wrappingLines = textWithCursors.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i_i = 0; i_i < wrappingLines.Length; i_i++)
            {
                if (wrappingLines[i_i].Contains("¤")) //FIND THE CORRECT LINE
                {
                    int index = wrappingLines[i_i].IndexOf("¤");
                    if (index - 27 >= 0) // one long line
                    {
                        wrappingLines[i_i].Remove(index, 1); //remove cursor
                        wrappingLines[i_i].Insert(index - 27, "¤");//place it elsewher
                    }
                    else if ((i_i > 0 && move < 0) || (i_i < wrappingLines.Length && move > 0)) //line too short -> put on the previous line
                    {
                        if (wrappingLines[i_i + (move / 27)].Length > index) //long enough to put cursor
                        {
                            wrappingLines[i_i].Remove(index, 1); //remove cursor
                            wrappingLines[i_i + (move / 27)].Insert(index, "¤");//place it elsewhere
                        }
                        else //place cursor at the end of the string
                        {
                            wrappingLines[i_i].Remove(index, 1); //remove cursor
                            wrappingLines[i_i + (move / 27)].Insert(wrappingLines[i_i + (move / 27)].Length, "¤");//place it elsewhere
                        }
                    }
                }
            }
            textWithCursors = "";
            //reconstitute textWithCursors 
            for (int i_i = 0; i_i < wrappingLines.Length; i_i++)
            {
                textWithCursors += wrappingLines[i_i];
                if (i_i < wrappingLines.Length - 1)
                {
                    textWithCursors += Environment.NewLine;
                }
            }
            Debug.Log("Deplacement Vertical");
            deb_selection = textWithCursors.IndexOf("¤");
        }
        
        */

        if (deb_selection > text.Length)
        {
            deb_selection = text.Length;
        }
        if (deb_selection < 0)
        {
            deb_selection = 0;
        }
    }

    public void deplacerCurseurFin(float moveX, float moveY)
    {
        if (!OneCursorMode)
        {
            if (moveX > 0)
            {
                fin_selection += 1;
            }
            if (moveX < 0)
            {
                fin_selection -= 1;
            }

            if (fin_selection > text.Length)
            {
                fin_selection = text.Length;
            }
            if (fin_selection < 0)
            {
                fin_selection = 0;
            }
        }
    }


    public bool copieSelection()
    {
        bool returnValue = false;
        if (!OneCursorMode) //must have 2 cursors
        {
            returnValue = true; //will return true if was able ot copy text
            int I_start = 0;
            int I_end = 0;
            if (fin_selection < deb_selection) //afficher curseur par ordre décroissant
            {
                I_start = fin_selection;
                I_end = deb_selection;
            }
            else if (fin_selection > deb_selection)
            {
                I_start = deb_selection;
                I_end = fin_selection;
            }

            selection = text.Substring(I_start, I_end - I_start);
            OneCursorMode = true;
            Debug.Log("Copie : " + selection);
        }
        return returnValue;
    }

    public void colleSelection()
    {
        if (OneCursorMode && selection != "")
        {
            text = text.Insert(deb_selection, selection);
            deb_selection += selection.Length;
            Debug.Log("Colle !");
        }
    }

    public string Display(bool displayCursor)
    {
        string curseur = " ";

        
        if (displayCursor)
        {
            curseur = "|";
        }
        else
        {
            curseur = " ";
        }

        textWithCursors = text.Insert(deb_selection, "¤").Insert(fin_selection, "~");

        string texteAAfficher = "Text splicing failed";

        if (OneCursorMode)
        {
            texteAAfficher = text.Insert(deb_selection, curseur);
        }
        else
        {
            if (displayCursor)
            {
                if (fin_selection < deb_selection) //the first to appear in the string has to be added, to not change index
                    texteAAfficher = text.Insert(deb_selection, highLightEnd).Insert(fin_selection, highLightStart);
                else if (fin_selection > deb_selection)
                    texteAAfficher = text.Insert(fin_selection, highLightEnd).Insert(deb_selection, highLightStart);
                else
                    texteAAfficher = text.Insert(deb_selection, highLightStart + curseur + highLightEnd);
            }
            else if (fin_selection == deb_selection)
                texteAAfficher = text.Insert(deb_selection, curseur);
            else
                texteAAfficher = text;
        }
        //return "Essayez d'écrire : \n" + m_objectif + "\n\n Saisie: \n" + texteAAfficher;
        
        return texteAAfficher;
    }

    public bool getCursorMode()
    {
        return OneCursorMode;
    }

    public string getSaisie() //WRITTEN TEXT WITHOUT MARKS OR CURSORS
    {
        return text;
    }
}
