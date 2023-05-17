/*
METHODS:
speaker
- speaker format: #speaker=(text)
- text sets the speaker panel text
This method will change the speaker name in the speaker panel.

Ex. #speaker=Character


enter
- enter format: #enter=(name)
- name will be the name of the GameObject that will be entering (type name exactly)
This method will fade in the specified character.

Ex. #enter=character


exit
- exit format: #exit=(name)
- name will be the name of the GameObject that will be exiting (type name exactly)
This method will fade out the specified character.

Ex. #exit=character

move
- move format: #move=(locationNumber + name)
- locationNumber will be the location GameObject will move to
- name will be the name of the GameObject that will be moving (type name exactly)
This method will move the specified character to one of the preset locations already in the scene. The method will start with just a number to indicate the location and then the rest will be the name of the GameObject.

Ex. #move=3character

*/

/*
Test Dialogue
All methods to perform the following actions can be found in the "DialogueTest" dialogue ink file in the dialogue folder.
Just type in a seperate line to create a line of dialogue.
Behind the scenes setup... #move=1Protagonist #move=4Enemy
Changing the speaker name to Narrator... #speaker=Narrator
Bringing in the protagonist character...#enter=Protagonist
Bringing in the enemy character... #enter=Enemy
Shifting enemy character to Location3... #move=3Enemy
Shifting protagonist character to Location2... #move=2Protagonist
Shifting protagonist character back to Location1... #move=1Protagonist
Shifting enemy character back to Location4... #move=4Enemy
Bringing out the protagonist character... #exit=Protagonist
Bringing out the enemy character... #exit=Enemy
End of Dialogue.
*/

#bgm=darkforesttheme
#move=1Protagonist #sprite=Enemy,lost_hog_hunter_shades_0 #move=4Enemy #enter=Protagonist #enter=Enemy
Huff huff (I can't run anymore!!!) #speaker=Protagonist 
No! I don't want to die! #speaker=Protagonist
This is getting hard to look at. #speaker=Guiding Soul
If you don't want to die then you better listen! Take this lantern! #speaker=???