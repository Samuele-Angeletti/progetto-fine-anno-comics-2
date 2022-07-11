using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class ReadOnlyAttribute : PropertyAttribute
{
    
}
