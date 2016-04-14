using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Flags]
public enum DamageType
{
    Physical = 0,
    Fire = 1,
    Electric = 2,
    Ice = 4
};

class Glove : Clothing
{
    /*
    Add properties for getting individual fields
    */

    private DamageType properties;
    private int damage;
    private bool ranged;

    public Glove()
    {
        properties = 0;
        damage = 0;
        ranged = false;
    }

    public Glove (DamageType _properties, int _damage, bool _ranged, DamageType _resistanceTypes, int _resistance)
    {
        properties = _properties;
        damage = _damage;
        ranged = _ranged;
        resistanceTypes = _resistanceTypes;
        resistance = _resistance;
    }
}