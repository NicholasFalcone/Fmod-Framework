using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ShowIfAttribute : PropertyAttribute
{
    //The name of the bool field that will be in control
    public string ConditionalSourceField = "";
    //TRUE = Hide in inspector / FALSE = Disable in inspector 
    public bool boolValueNeeded = false;
    public int intValueNeeded = 0;
    public int[] intValuesNeeded;

    public ShowIfAttribute(string conditionalSourceField)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.boolValueNeeded = false;
    }

    public ShowIfAttribute(string conditionalSourceField, bool hideInInspector)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.boolValueNeeded = hideInInspector;
    }

    public ShowIfAttribute(string conditionalSourceField, int intValue)
    {
        this.ConditionalSourceField = conditionalSourceField;
        intValuesNeeded = new int[] {intValue};
        //Serialized.serializedObject.FindProperty(conditionPath)
        //this.HideInInspector = hideInInspector;
    }

    public ShowIfAttribute(string conditionalSourceField, params int[] par)
    {
        this.ConditionalSourceField = conditionalSourceField;
        intValuesNeeded = par;
    }
}
