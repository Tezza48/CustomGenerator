using UnityEngine;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour
{

    private Generator generator;

    private Glove gloveL;
    private Glove gloveR;
    private Boots boots;

    private int damage;//damage you deal
    private float dodge;//percentage chance that you'll dodge damage
    private float resistance; //resistance will divide the damage dealt

    private List<Item> Inventory = new List<Item>();

    private const int startingDamage = 5;
    private const float startingDodge = 0.01f;
    private const float startingResistance = 1.1f;

    #region Properties
    public Generator Generator { set { generator = value; } }

    #region Stats
    private int Damage { get { return gloveL.Damage + gloveR.Damage + damage; } }

    private float Dodge { get { return boots.Dodge + dodge; } }

    private DamageTypes DamageType { get { return gloveL.DamageType | gloveR.DamageType; } }

    private DamageTypes Resistances { get { return gloveL.Resistances | gloveR.Resistances | boots.Resistances; } }

    private float Resistance { get { return gloveL.Resistance + gloveR.Resistance + boots.Resistance + resistance; } }
    #endregion
    #endregion

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
        else if (Input.GetKeyUp(RogueInput.buttons["inventory"]))
        {
            PrintInventory();
        }
    }

    private void PrintInventory()
    {
        string items = gloveL.ToString() + gloveR.ToString() + boots.ToString() + Inventory.ToString();
        Debug.Log(items);
    }

    internal void InitNewPlayerCharacter()
    {
        gloveL = new Glove(DamageTypes.Physical, 1, false, DamageTypes.Physical | DamageTypes.Fire, 0);
        gloveR = new Glove(DamageTypes.Physical, 1, true, DamageTypes.Physical, 0);

        boots = new Boots(1, 1, DamageTypes.Physical, 1);

        damage = startingDamage;
    }

    #region Movement
    private void ActionRight()
    {
        RaycastHit ray;
        if (Physics.Raycast(transform.position, Vector3.right, out ray, Generator.SPAWN_INTERVAL * 2))
        {
            if (ray.collider.tag == "Exit")
                transform.position += Vector3.right * Generator.SPAWN_INTERVAL * 2;
        }
        else
        {
            transform.position += Vector3.right * Generator.SPAWN_INTERVAL * 2;
        }
    }

    private void ActionLeft()
    {
        RaycastHit ray;
        if (Physics.Raycast(transform.position, -Vector3.right, out ray, Generator.SPAWN_INTERVAL * 2))
        {
            if (ray.collider.tag == "Exit")
                transform.position -= Vector3.right * Generator.SPAWN_INTERVAL * 2;
        }
        else
        {
            transform.position -= Vector3.right * Generator.SPAWN_INTERVAL * 2;
        }
    }

    private void ActionDown()
    {
        RaycastHit ray;
        if (Physics.Raycast(transform.position, -Vector3.forward, out ray, Generator.SPAWN_INTERVAL * 2))
        {
            if (ray.collider.tag == "Exit")
                transform.position -= Vector3.forward * Generator.SPAWN_INTERVAL * 2;
        }
        else
        {

            transform.position -= Vector3.forward * Generator.SPAWN_INTERVAL * 2;
        }
    }

    private void ActionUp()
    {
        RaycastHit ray;
        if (Physics.Raycast(transform.position, Vector3.forward, out ray, Generator.SPAWN_INTERVAL * 2))
        {
            if (ray.collider.tag == "Exit")
                transform.position += Vector3.forward * Generator.SPAWN_INTERVAL * 2;
        }
        else
        {
            transform.position += Vector3.forward * Generator.SPAWN_INTERVAL * 2;
        }
    }
    #endregion

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Exit")
        {
            generator.SpawnNextDungeon();
        }
    }
}
