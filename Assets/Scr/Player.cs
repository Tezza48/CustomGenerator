using UnityEngine;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour {

    private Glove gloveL;
    private Glove gloveR;
    private Boots boots;

    private List<Item> Inventory = new List<Item>();

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyUp(RogueInput.buttons["up"]))
        {
            ActionUp();
        }
        else if (Input.GetKeyUp(RogueInput.buttons["down"]))
        {
            ActionDown();
        }
        else if (Input.GetKeyUp(RogueInput.buttons["left"]))
        {
            ActionLeft();
        }
        else if (Input.GetKeyUp(RogueInput.buttons["right"]))
        {
            ActionRight();
        }
    }

    private void ActionRight()
    {
        if (!Physics.Raycast(transform.position, Vector3.right, BSGenerator.SPAWN_INTERVAL * 2))
        {
            transform.position += Vector3.right * BSGenerator.SPAWN_INTERVAL * 2;
        }
    }

    private void ActionLeft()
    {
        if (!Physics.Raycast(transform.position, -Vector3.right, BSGenerator.SPAWN_INTERVAL * 2))
        {
            transform.position -= Vector3.right * BSGenerator.SPAWN_INTERVAL * 2;
        }
    }

    private void ActionDown()
    {
        if (!Physics.Raycast(transform.position, -Vector3.forward, BSGenerator.SPAWN_INTERVAL * 2))
        {
            transform.position -= Vector3.forward * BSGenerator.SPAWN_INTERVAL * 2;
        }
    }

    private void ActionUp()
    {
        if (!Physics.Raycast(transform.position, Vector3.forward, BSGenerator.SPAWN_INTERVAL * 2))
        {
            transform.position += Vector3.forward * BSGenerator.SPAWN_INTERVAL * 2;
        }
    }
}
