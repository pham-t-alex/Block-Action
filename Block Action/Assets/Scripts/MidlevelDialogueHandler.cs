using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using System.Data;
using Unity.VisualScripting;
using UnityEngine.TextCore.Text;
using TextAsset = UnityEngine.TextAsset;
using Ink.Parsed;
using Story = Ink.Runtime.Story;

public class MidlevelDialogueHandler : MonoBehaviour
{
    // Dialogue necessities
    [SerializeField] public GameObject dialoguePanel;
    [SerializeField] public GameObject dialogueSpeaker;
    [SerializeField] public TextMeshProUGUI dialogueText;
    [SerializeField] public TextMeshProUGUI displayNameText;
    [SerializeField] public TextAsset _inkJSON;
    [SerializeField] public GameObject dialogueSystem;

    // Variables for time-based actions
    private float typingSpeed = 0.04f;
    private float alphaSpeed = 0.01f;
    private float movementSpeed = 0.04f;

    // Variables for most of the dialogue handling
    private static MidlevelDialogueHandler instance;
    private Story currentStory;
    private Coroutine displayLineCoroutine;
    private bool dialogueIsPlaying;
    private bool canContinueToNextLine = false;
    private bool spacePressedSameFrame = false;
    private bool actionIsPerforming = false;
    private DialogueCharacter[] characterArray;
    private GameObject[] locationArray;

    // Constants for handling tags in ink files
    private const string CHARACTER_TAG = "speaker";
    private const string CHARACTER_ENTER = "enter";
    private const string CHARACTER_EXIT = "exit";
    private const string CHARACTER_LOCATION = "move";

    void Awake()
    {
        instance = this; // Sets the dialogue instance as the current gameObject

        // Initialize arrays
        characterArray = GameObject.FindObjectsOfType<DialogueCharacter>();
        locationArray = GameObject.FindGameObjectsWithTag("Location");
    }

    void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueSpeaker.SetActive(false);
        displayNameText.text = "";

        foreach (DialogueCharacter character in characterArray)
        {
            character.spriteRenderer.color = new Color(1, 1, 1, 0);
        }

        //EnterDialogueMode(inkJSON); // Initiate Dialogue Sequence
        dialogueSystem.transform.position = new Vector3(0, dialogueSystem.transform.position.y, dialogueSystem.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueIsPlaying) // Checks if it is in dialogue mode
        {
            if (Input.GetKeyDown(KeyCode.Space)) // Bug fix: pressing space no longer continues dialogue and skips typing effect at the same time
            {
                spacePressedSameFrame = true;
            }

            if (!dialogueIsPlaying)
            {
                return;
            }

            if (canContinueToNextLine && spacePressedSameFrame && !actionIsPerforming)
            {
                spacePressedSameFrame = false;
                ContinueStory(); // Pressing space will move on to the next line of dialogue
            }
        }


    }

    public static MidlevelDialogueHandler GetInstance()
    {
        return instance;
    }

    public void EnterDialogueMode(TextAsset inkJSON) // Enters dialogue mode and brings up the dialogue panel
    {
        currentStory = new Story(inkJSON.text); // Initializes the current story text
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        dialogueSpeaker.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogueMode() // Exits dialogue mode and reverts the dialogue screens
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueSpeaker.SetActive(false);
        dialogueText.text = "";
    }

    public void ContinueStory()
    {
        if (currentStory.canContinue) // If there are additional lines of dialogue, then continue
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));

            HandleTags(currentStory.currentTags);
        }
        else // Stop and exit dialogue mode if no more lines of dialogue are present
        {
            ExitDialogueMode();
            if (GimmickController.gimmickController.effectPause)
            {
                GimmickController.UnpauseEffects();
            }
        }
    }

    public void ChangeTextAsset(string textAssetName)
    { 
        _inkJSON = Resources.Load<TextAsset>("Dialogue/" + textAssetName);
    }

    public IEnumerator DisplayLine(string line)
    {
        canContinueToNextLine = false;
        dialogueText.text = ""; // Sets the dialogue to an empty string
        bool isAddingRichTag = false;

        foreach (char letter in line.ToCharArray())
        {
            if (spacePressedSameFrame)
            {
                // set to false for good measure so that the input only happens once
                spacePressedSameFrame = false;
                dialogueText.text = line;
                break;
            }

            if (letter == '<' || isAddingRichTag)
            {
                isAddingRichTag = true;
                dialogueText.text += letter;
                if (letter == '>')
                {
                    isAddingRichTag = false;
                }
            }
            else
            {
                dialogueText.text += letter; // Adds a letter to dialogueText
                yield return new WaitForSeconds(typingSpeed); // This determines how long it should wait before adding the next letter
            }
        }

        canContinueToNextLine = true;
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags) // Loop through each tag and handle them individually
        {
            // Tag parsing
            string[] splitTag = tag.Split('=');
            if (splitTag.Length != 2)
            {
                // Debug.Log("Tag Error: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagAction = splitTag[1].Trim();

            // Tag handling
            switch (tagKey)
            {
                // Each case performs a different action

                case CHARACTER_TAG: // Changes the speaker header
                    displayNameText.text = tagAction;
                    break;
                case CHARACTER_ENTER: // Fades the specified character in if it is found
                    DialogueCharacter characterEnter;
                    foreach (DialogueCharacter sprite in characterArray)
                    {
                        if (sprite.name == tagAction)
                        {
                            characterEnter = sprite;
                            StartCoroutine(EnterCharacter(characterEnter));
                            break;
                        }
                    }
                    break;
                case CHARACTER_EXIT:
                    DialogueCharacter characterExit;
                    foreach (DialogueCharacter sprite in characterArray)
                    {
                        if (sprite.name == tagAction)
                        {
                            characterExit = sprite;
                            StartCoroutine(ExitCharacter(characterExit));
                            break;
                        }
                    }
                    break;
                case CHARACTER_LOCATION:
                    string location = tagAction.Substring(0, 1);
                    string character = tagAction.Substring(1);
                    DialogueCharacter characterToMove;

                    foreach (DialogueCharacter sprite in characterArray)
                    {
                        if (sprite.name == character)
                        {
                            characterToMove = sprite;
                            string destination = "Location" + location;

                            foreach (GameObject place in locationArray)
                            {
                                if (place.name == destination)
                                {
                                    StartCoroutine(Move(characterToMove, place));
                                    break;
                                }
                            }

                            break;
                        }
                    }
                    break;
                default: // Default Error Catcher
                    print("Nothng to handle current tag");
                    break;
            }
        }
    }

    private IEnumerator EnterCharacter(DialogueCharacter character)
    {
        while (character.spriteRenderer.color.a < 1)
        {
            actionIsPerforming = true;
            character.spriteRenderer.color += new Color(0, 0, 0, alphaSpeed);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        actionIsPerforming = false;
    }
    private IEnumerator ExitCharacter(DialogueCharacter character)
    {
        while (character.spriteRenderer.color.a > 0)
        {
            actionIsPerforming = true;
            character.spriteRenderer.color += new Color(0, 0, 0, -alphaSpeed);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        actionIsPerforming = false;
    }

    private IEnumerator Move(DialogueCharacter character, GameObject location)
    {
        while (character.transform.position.x != location.transform.position.x)
        {
            actionIsPerforming = true;
            character.transform.position = Vector2.MoveTowards(character.transform.position, location.transform.position, movementSpeed);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        actionIsPerforming = false;
    }
}