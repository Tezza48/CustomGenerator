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
        RaycastHit ray;
        if (Physics.Raycast(transform.position, Vector3.right, out ray, BSGenerator.SPAWN_INTERVAL * 2))
        {
            if (ray.collider.tag != "Wall")
            {
                transform.position += Vector3.right * BSGenerator.SPAWN_INTERVAL * 2;
            }
        }
    }

    private void ActionLeft()
    {
        RaycastHit ray;
        if (Physics.Raycast(transform.position, -Vector3.right, out ray, BSGenerator.SPAWN_INTERVAL * 2))
        {
            if (ray.collider.tag != "Wall")
            {
                transform.position -= Vector3.right * BSGenerator.SPAWN_INTERVAL * 2;
            }
        }
    }

    private void ActionDown()
    {
        RaycastHit ray;
        if (Physics.Raycast(transform.position, -Vector3.up, out ray, BSGenerator.SPAWN_INTERVAL * 2))
        {
            if (ray.collider.tag != "Wall")
            {
                transform.position -= Vector3.up * BSGenerator.SPAWN_INTERVAL * 2;
            }
        }
    }

    private void ActionUp()
    {
        RaycastHit ray;
        if (Physics.Raycast(transform.position, Vector3.up, out ray, BSGenerator.SPAWN_INTERVAL * 2))
        {
            if (ray.collider.tag != "Wall")
            {
                transform.position += Vector3.up * BSGenerator.SPAWN_INTERVAL * 2;
            }
        }
    }
}
