using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
[Serializable]
class Boots : Clothing
{

    /*
    Add properties for getting individual fields
    */

    //boots are the main source of resistance
    //they also allow you to make more moves in your turn

    private int dodge;
    private int grip;

    public DamageTypes Resistances
    {
        get
        {
            return resistanceTypes;
        }
    }

    public float Resistance
    {
        get
        {
            return resistance;
        }
    }

    public int Dodge
    {
        get
        {
            return dodge;
        }
    }

    public Boots()
    {
        dodge = 0;
        grip = 0;
        resistanceTypes = 0;
        resistance = 0;
    }

    public Boots (int _dodge, int _grip, DamageTypes _resistanceTypes, float _resistance)
    {
        dodge = _dodge;
        grip = _grip;
        resistanceTypes = _resistanceTypes;
        resistance = _resistance;
    }

    public override string ToString()
    {
        return "Boots| Dodge: " + dodge + ", Grip: " + grip + ", Res: " + resistance.ToString("N2") + " " + resistanceTypes.ToString() + "\n";
    }
}
