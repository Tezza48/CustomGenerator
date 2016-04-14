using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Boots : Clothing
{

    /*
    Add properties for getting individual fields
    */

    //boots are the main source of resistance
    //they also allow you to make more moves in your turn

    private int dodge;
    private int speed;//number of spaces you can move in a turn (depending on the dungeon level)

    public Boots()
    {
        dodge = 0;
        speed = 0;
        resistanceTypes = 0;
        resistance = 0;
    }

    public Boots (int _dodge, int _speed, DamageType _resistanceTypes, int _resistance)
    {
        dodge = _dodge;
        speed = _speed;
        resistanceTypes = _resistanceTypes;
        resistance = _resistance;
    }
}
