using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    private void Update()
    {
        // Force end dialogue if the panel is closed externally
        if (isDialogueActive && !dialoguePanel.activeSelf)
        {
            // We dont call PauseController.SetPause(false) here 
            // because the external mechanism likely already handled unpausing.
            ForceEndDialogueCleanup();
        }
    }

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        // If no dialogue data or the game is paused and no dialogue is active
        if (dialogueData == null || (PauseController.IsGamePaused && !isDialogueActive))
            return;

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);
        PauseController.SetPause(true);

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            // Skip typing animation and show the full line
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if(++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            //If another line, type next line
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            SoundEffectManager.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if(dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        SoundEffectManager.StopVoice();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        PauseController.SetPause(false);
    }

    // Ends dialogue without affecting the pause state (for external closing)
    private void ForceEndDialogueCleanup()
    {
        StopAllCoroutines();
        SoundEffectManager.StopVoice();
        isDialogueActive = false;
        // Don't modify dialogueText or dialoguePanel activation here,
        // as they were likely already handled externally.
    }
}
