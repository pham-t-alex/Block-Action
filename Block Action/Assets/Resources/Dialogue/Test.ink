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

bgm
- bgm format: #bgm=(name of bgm)
- name should be exactly as indicated in the files
- will print out the inputted audio name in the console for precautionary measures
- if no audio is played, that means the name might not exist or is typed incorrectly
This method will set the background music to the name that has been inputted. It will return a debug log along with what was inputted if not found.

Ex. #bgm=darkforesttheme

sprite
- sprite format: #sprite=(gameObject),(sprite)
- gameObject will be the name of the game object whose sprite will be changed
- sprite will be the new sprite that the game object will change into.
- will return debug logs if one of the parameters are not found
This method will take the game object name and the sprite, separated by a comma, to change the sprite of the specified game object with the specified sprite. Sprites must be added to the "sprites" array in DialogueHandler in order for this tp function correctly.

Ex. #sprite=GuidanceSoul=FProtagSheet_0

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

ow that hurt #speaker=Protagonist