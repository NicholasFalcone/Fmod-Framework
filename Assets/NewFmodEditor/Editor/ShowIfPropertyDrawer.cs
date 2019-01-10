using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //get the attribute data
        ShowIfAttribute showIfAtt = (ShowIfAttribute)attribute;
        //check if the propery we want to draw should be enabled
        bool enabled = GetShowIfAttributeResult(showIfAtt, property);

        //Enable/disable the property
        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;

        //Check if we should draw the property
        if (
            (property.propertyType == SerializedPropertyType.Boolean && !showIfAtt.boolValueNeeded || enabled) 
            )
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        //Ensure that the next property that is being drawn uses the correct settings
        GUI.enabled = wasEnabled;
    }

    private bool GetShowIfAttributeResult(ShowIfAttribute showIfAtt, SerializedProperty property)
    {
        bool enabled = true;
        //Look for the sourcefield within the object that the property belongs to
        string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
        string conditionPath = propertyPath.Replace(property.name, showIfAtt.ConditionalSourceField); //changes the path to the conditionalsource property path
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if(sourcePropertyValue != null) { 
            //SHOW IF SOURCEPROPERTY IS CORRECT BOOL VALUE
            if(sourcePropertyValue != null && sourcePropertyValue.propertyType == SerializedPropertyType.Boolean) { 
                enabled = sourcePropertyValue.boolValue;
            }
            //SHOW IF SOURCEPROPERTY IS CORRECT INT  VALUE (USED ALSO FOR ENUMERATORS)
            else if (sourcePropertyValue != null
                    &&
                    (sourcePropertyValue.propertyType == SerializedPropertyType.Integer 
                    || sourcePropertyValue.propertyType == SerializedPropertyType.Enum))
            {
                enabled = showIfAtt.intValuesNeeded.Contains(sourcePropertyValue.intValue);
            }
            /*else if (sourcePropertyValue != null && sourcePropertyValue.propertyType == SerializedPropertyType.Enum)
            {
                enabled = sourcePropertyValue.intValue == showIfAtt.intValueNeeded;
            }*/
            else
            {
                Debug.LogWarning("Property type is not managed Yet");
            }
        }
        else
        {
            Debug.LogWarning("WARNING: " + showIfAtt.ConditionalSourceField + " variable does not exist");
        }

        return enabled;
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIfAtt = (ShowIfAttribute)attribute;
        bool enabled = GetShowIfAttributeResult(showIfAtt, property);

        if (!showIfAtt.boolValueNeeded || enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            //The property is not being drawn
            //We want to undo the spacing added before and after the property
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
