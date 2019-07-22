# WizardsCode.Tools.DocGen.ExampleMonoBehaviour
DocGen adds a DocGen attribute that allows more documentation text to be added to an attribute. This text will be displayed in the inspector via a PropertyDrawer as well as in the generated documentation.


## Public String (String)

This is a public string field with a tooltip (you are reading it now).

### Details

Using the DocGen attribute you can add additional documentation to a field that doesn't fit into a ToolTip. This content will only be visible if the field is expanded. The content will also appear in the generated documentation. 

Default Value     : "This is the default value of this string."


## Float Field (Single)

Field with a range.

Default Value     : 0.5
Range             : 0 and 1.5


## Public But Undocumented String (String)

No tooltip provided.

Default Value     : "This public string does not have a tooltip."


## Private Serialized String (String)

This is a private field, but it has the SerializeField attribute. This text comes from the tooltip for the field.

Default Value     : "This is the default value."

