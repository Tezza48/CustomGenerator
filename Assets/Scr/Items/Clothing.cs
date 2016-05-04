using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
abstract class Clothing : Item
{
    protected DamageTypes resistanceTypes;
    protected float resistance;
}
