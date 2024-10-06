using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCInteract : MonoBehaviour
{
    [SerializeField] NPCInteractable npcInfo;

    [Header("Dialogue Panel")]
    [SerializeField] GameObject npcDialogueCanvas;
    [SerializeField] TextMeshProUGUI dialogueTextBox;
    [SerializeField] Button nextLineIndicator;

    [Space(10)]
    [SerializeField] float textSpeed = 0.1f;

    private int scriptLength = 0;
    private NPCDialogue currPrompt;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!npcInfo)
        {
            throw new System.Exception("NPC Info needed for Interact Script");
        }
        dialogueTextBox.text = string.Empty;
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueTextBox.text == currPrompt.textLine)
            {
                nextLineIndicator.gameObject.SetActive(false);
                DisplayNextPrompt();
            }
            else
            {
                StopAllCoroutines();
                dialogueTextBox.text = currPrompt.textLine;
                nextLineIndicator.gameObject.SetActive(true);
            }
        }
    }

    public void PlayDialogueScript()
    {
        currPrompt = npcInfo.dialoguePrompts[index];
        StartCoroutine(DisplayCurrentPrompt(currPrompt));
    }

    #region Enumerators for displaying prompts
    private IEnumerator DisplayCurrentPrompt(NPCDialogue prompt)
    {
        //speaker.sprite = prompt.speaker;
        foreach (char c in prompt.textLine.ToCharArray())
        {
            dialogueTextBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        if (dialogueTextBox.text == prompt.textLine)
        {
            nextLineIndicator.gameObject.SetActive(true);
        }
    }

    private void DisplayNextPrompt()
    {
        if (index < scriptLength - 1)
        {
            index++;
            dialogueTextBox.text = string.Empty;
            currPrompt = npcInfo.dialoguePrompts[index];
            StartCoroutine(DisplayCurrentPrompt(currPrompt));
        }
        else
        {
            npcInfo.SetHasBeenPlayed();
            npcDialogueCanvas.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
    #endregion
}
