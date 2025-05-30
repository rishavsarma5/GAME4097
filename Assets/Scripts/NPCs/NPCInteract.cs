using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System;

public class NPCInteract : MonoBehaviour
{
    [SerializeField] GameNPC npcInfo;

    [Header("NPC Info")]
    [SerializeField] GameObject npcNameCanvas;
    [SerializeField] TextMeshProUGUI npcNameText;

    [Header("Prompt Panel Components")]
    [SerializeField] GameObject npcPromptCanvas;
    [SerializeField] TextMeshProUGUI promptHeader;
    [SerializeField] GameObject specialQuestionLockedButton;
    [SerializeField] TextMeshProUGUI specialQuestionLockedButtonText;
    [SerializeField] GameObject specialQuestionUnlockedButton;
    [SerializeField] TextMeshProUGUI specialQuestionUnlockedButtonText;

    [Header("Dialogue Panel Components")]
    [SerializeField] GameObject npcDialogueCanvas;
    [SerializeField] TextMeshProUGUI dialogueTextBox;
    [SerializeField] GameObject nextLineIndicator;

    [Space(10)]
    [SerializeField] float textSpeed = 0.1f;

    [SerializeField] InputActionReference continueActionRef;
    [SerializeField] InputActionReference closeActionRef;

    [SerializeField] private Transform cameraTransform;

    private int scriptLength = 0;
    private NPCDialogue currQuestionDialogue;
    private int lineIndex = 0;
    private int currQuestion = -1;

    private Coroutine promptHeaderCoroutine;

    private void Awake()
    {
        continueActionRef.action.started += UpdateDialogueText;
        closeActionRef.action.started += CloseDialoguePanel;
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;

        if (!npcInfo)
        {
            throw new System.Exception("NPC Info needed for Interact Script");
        }
        npcNameCanvas.SetActive(true);
        npcNameText.text = npcInfo.npcName;
        npcPromptCanvas.SetActive(false);
        npcDialogueCanvas.SetActive(false);
        promptHeaderCoroutine = null;
    }

    private void OnDestroy()
    {
        continueActionRef.action.started -= UpdateDialogueText;
        closeActionRef.action.started -= CloseDialoguePanel;
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueTextBox.text = string.Empty;
        lineIndex = 0;
    }

    public void OnHoverPromptChange()
    {
        if (promptHeaderCoroutine == null) {
            npcNameText.text = String.Empty;
            npcNameText.fontSize = 27;
            promptHeaderCoroutine = StartCoroutine(DisplayPromptChangeText());
        }

        Vector3 directionToCamera = cameraTransform.position - transform.position;

        directionToCamera.y = 0f;

        if (directionToCamera.sqrMagnitude > 0.001f)
        {
            // Rotate the NPC to look at the camera only on the Y-axis
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            transform.rotation = targetRotation;
        }
    }

    private IEnumerator DisplayPromptChangeText()
    {
        foreach (char c in npcInfo.npcInteractInfo.promptCanvasHeaderText.ToCharArray())
        {
            npcNameText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        promptHeaderCoroutine = null;
    }

    public void ExitHoverPromptChange()
    {
        if (promptHeaderCoroutine != null)
        {
            StopCoroutine(promptHeaderCoroutine);
            promptHeaderCoroutine = null;
        }
        npcNameText.fontSize = 30;
        npcNameText.text = npcInfo.npcName;
    }

    public void UpdateSpecialButton()
    {
        if (npcInfo.whatUnlocksNPCQuestion == "Clue1")
        {
            if (npcInfo.clues[0].isFound)
            {
                specialQuestionLockedButton.gameObject.SetActive(false);
                specialQuestionUnlockedButton.gameObject.SetActive(true);
                specialQuestionUnlockedButtonText.text = $"Confront about {npcInfo.clues[0].clueName}";

            }
            else
            {
                specialQuestionLockedButton.gameObject.SetActive(true);
                specialQuestionUnlockedButton.gameObject.SetActive(false);
                specialQuestionLockedButtonText.text = $"Locked: {npcInfo.clues[0].clueName} Not Found";
            }
        } else if (npcInfo.whatUnlocksNPCQuestion == "Clue2")
        {
            if (npcInfo.weapon.isFound)
            {
                specialQuestionLockedButton.gameObject.SetActive(false);
                specialQuestionUnlockedButton.gameObject.SetActive(true);
                specialQuestionUnlockedButtonText.text = $"Confront about {npcInfo.clues[1].clueName}";

            }
            else
            {
                specialQuestionLockedButton.gameObject.SetActive(true);
                specialQuestionUnlockedButton.gameObject.SetActive(false);
                specialQuestionLockedButtonText.text = $"Locked: {npcInfo.clues[1].clueName} Not Found";
            }
        } else if (npcInfo.whatUnlocksNPCQuestion == "Weapon")
        {
            if (npcInfo.weapon.isFound)
            {
                specialQuestionLockedButton.gameObject.SetActive(false);
                specialQuestionUnlockedButton.gameObject.SetActive(true);
                specialQuestionUnlockedButtonText.text = $"Confront about {npcInfo.weapon.weaponName}";

            }
            else
            {
                specialQuestionLockedButton.gameObject.SetActive(true);
                specialQuestionUnlockedButton.gameObject.SetActive(false);
                specialQuestionLockedButtonText.text = $"Locked: {npcInfo.weapon.weaponName} Not Found";
            }
        } else
        {
            Debug.LogError("Unknown param for npcInfo: what unlocks special question: should be either Clue1, Clue2, or Weapon");
        }
    }

    public void OpenNPCPromptCanvas()
    {
        dialogueTextBox.text = string.Empty;
        lineIndex = 0;
        npcNameCanvas.SetActive(false);
        npcPromptCanvas.SetActive(true);
        promptHeader.text = npcInfo.npcInteractInfo.promptCanvasHeaderText;
        UpdateSpecialButton();
    }

    private void UpdateDialogueText(InputAction.CallbackContext context)
    {
        Debug.Log("Skipped text");
        if (npcDialogueCanvas.activeSelf) UpdateDialogueDisplay();
    }

    private void CloseDialoguePanel(InputAction.CallbackContext context)
    {
        npcPromptCanvas.SetActive(false);
        npcDialogueCanvas.SetActive(false);
        npcNameCanvas.SetActive(true);
        dialogueTextBox.text = string.Empty;
        lineIndex = 0;
        npcNameText.fontSize = 30;
        npcNameText.text = npcInfo.npcName;
    }

    private void UpdateDialogueDisplay()
    {
        if (dialogueTextBox.text == currQuestionDialogue.textLines[lineIndex])
        {
            nextLineIndicator.SetActive(false);
            DisplayNextPrompt();
        }
        else
        {
            StopAllCoroutines();
            dialogueTextBox.text = currQuestionDialogue.textLines[lineIndex];
            nextLineIndicator.SetActive(true);
        }
    }

    public void PlayDialogueForQuestion(int question)
    {
        currQuestion = question;
        lineIndex = 0;
        dialogueTextBox.text = string.Empty;
        currQuestionDialogue = npcInfo.npcInteractInfo.dialoguePrompts[currQuestion];
        scriptLength = currQuestionDialogue.textLines.Count;
        if (scriptLength == 0)
        {
            throw new System.Exception("NPC Info needs text lines for each question");
        }
        npcPromptCanvas.SetActive(false);
        npcDialogueCanvas.SetActive(true);
        StartCoroutine(DisplayCurrentQuestionLine(currQuestionDialogue));
    }

    #region Enumerators for displaying prompts
    private IEnumerator DisplayCurrentQuestionLine(NPCDialogue prompt)
    {
        foreach (char c in prompt.textLines[lineIndex].ToCharArray())
        {
            dialogueTextBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        if (dialogueTextBox.text == prompt.textLines[lineIndex])
        {
            nextLineIndicator.SetActive(true);
        }
    }

    private void DisplayNextPrompt()
    {
        if (lineIndex < scriptLength - 1)
        {
            lineIndex++;
            dialogueTextBox.text = string.Empty;
            StartCoroutine(DisplayCurrentQuestionLine(currQuestionDialogue));
        }
        else
        {
            //npcInfo.SetHasBeenPlayed();
            npcDialogueCanvas.SetActive(false);
        }
    }
    #endregion
}
