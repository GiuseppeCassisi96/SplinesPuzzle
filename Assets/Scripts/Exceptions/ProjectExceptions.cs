using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class IndexNotValid : Exception
{
    public IndexNotValid(String message) : base(message) { }
}

class ValueNotValid : Exception 
{ 
    public ValueNotValid(String message) : base(message) { }
}
