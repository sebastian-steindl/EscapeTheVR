# API (level)-Documentation
*Aplies to API-version* :1.0

*Fileformat* : XML

*Filename pattern*: `level<levelId>.xml` in `Assets/Resources`

## How are the elements orderd?
The order in the XML file determens the order of the `element`-tags appear in the `program` section. There is only one exception: if an element has the attribute `program.element:isPartOtSolution` set to false, the element will be ignored.

## Fields
 Field | Value | Description |
--- | --- | --- |
apiversion | string | current API version. For now this has to be always set to `10`.|
levelId | int | identifier for the level
name | string | Name of the level, that may be displayed to the user.|
descr | string | A short description of the level that may be displayed to the user.|
output | string | The output of the puzzle if applicable.|
startFromCode | bool | Determens if the code should be displayed from the start. The user now has to "rebuild" the code with the given elements. Internally called "inverse mode".|
program.element:isPartOfSolution | bool | If set to true, this element has to be set on the solution (GameBoard) for the solution to be correct. *Default value: `false`*|
program.element:valueOfId | int | Only needed for all programmingElements that are child elements. (These are (`bool`,`number` and `text`) 
program.element:isLocked | bool | Only needs to be set, if this element should be locked on the GameBoard.|
program.element.type | string / programmingElement | What type the generated object should be of.|
program.element.positionX | float | X-Coordinate where the object should be placed.|
program.element.positionY | float | Y-Coordinate where the object should be placed.|
program.element.positionZ | float | Z-Coordinate where the object should be placed.|
program.element.text | string | Text that should be displayed on the Object.|
program.element.hintPath | string | Path, to audio file of the hint.|
code.codeElement | string | The code that should be displayed on the whiteboard.|
code.codeElement:elementId | int | The corrosponding element id. (Needs to be set for highlighting.)|
code.codeElement:newLine | bool | Marks *last* element of a row.|
hintFiles | list | The list of audio hints of the level. Will be played in order.
hintFiles.hintFile | string | The resource path for a audio-level-hint relative to the "Resources" folder.

## programmingElement Reference:
Currently there are the following programmingElement-types:
- `variable`: Creates a variable
- `for`: creates a for-loop
- `if`: creates a if-condition
- `while`: creates a while-loop
- `==`: equals
- `&&`: and
- `||`: or
- `!`: negation/not
- `print()`: represents the print function
- `text`: represents a text value
- `number`: represents a number value
- `bool`: represents a bool value
- `interval`: creates an interval (needs to number blocks as reference)
- `end`: marks the end of a sequence

