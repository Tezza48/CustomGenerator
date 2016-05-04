using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Flags]
public enum DamageTypes
{
    Physical = 1,
    Fire = 2,
    Ice = 4
};

[Serializable]
class Glove : Clothing
{
    /*
    Add properties for getting individual fields
    */

    private int damage;
    private bool ranged;
    private DamageTypes damageType;

    public DamageTypes DamageType
    {
        get
        {
            return damageType;
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }
    }

    public bool Ranged
    {
        get
        {
            return ranged;
        }
    }

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

    public Glove()
    {
        damageType = 0;
        damage = 0;
        ranged = false;
    }

    public Glove (DamageTypes _properties, int _damage, bool _ranged, DamageTypes _resistanceTypes, float _resistance)
    {
        damageType = _properties;
        damage = _damage;
        ranged = _ranged;
        resistanceTypes = _resistanceTypes;
        resistance = _resistance;
    }

    public override string ToString()
    {
        return "Glove| Damage: " + damage + ((ranged) ? ", Ranged " : " ") + DamageType.ToString() + ", Res: " + resistance.ToString("N2") + " " + resistanceTypes.ToString() + "\n";
    }
}