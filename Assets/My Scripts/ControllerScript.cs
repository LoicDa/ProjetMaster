using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    public GameObject Writing_playerController;
    public GameObject Wandering_playerController;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
/*
    void OnCollisionEnter(Collision collision)
    {
         Debug.Log(collision);
        
    }


    void OnCollisionStay(Collision collision)
    { 
        //Check to see if the Collider's name is "Chest"
        Debug.Log(collision.collider.name);


        playerController.GetComponent<PlayerController_writing>().collisionDetected(collision.collider.name);
    }
*/

    private void OnTriggerEnter(Collider collider)
    {

        string colliderName = collider.GetComponent<Collider>().name;

        if (colliderName.Contains("TextBox")) //if is a textbox
        {
            collider.gameObject.GetComponent<TextBoxController>().HasCollided(); // change textBox color
            Wandering_playerController.GetComponent<PlayerController_wandering>().textBoxTouched(collider.gameObject); //textbox has been touched
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        //playerController.GetComponent<PlayerController_writing>().collisionDetected(other.GetComponent<Collider>().name);
        string colliderName = collider.GetComponent<Collider>().name;

        if (colliderName.Contains("TextBox"))
        {
            collider.gameObject.GetComponent<TextBoxController>().ExitCollision(); //change textbox color to normal
            Wandering_playerController.GetComponent<PlayerController_wandering>().textBoxExited(); //textbox no longer touching
        }

    }



}
