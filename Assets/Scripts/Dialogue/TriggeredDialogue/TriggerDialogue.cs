using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TriggerDialogue : MonoBehaviour
{
    [Header("Trigger Dialogue UI")]
    [SerializeField] private GameObject triggerDialogueCanvas;
    [SerializeField] private TMP_Text triggerSpeakerText;
    [SerializeField] private Button triggerNextButton;
    [SerializeField] private TMP_Text triggerDialogueText;
    [SerializeField] private Image triggerPortraitImage;

    [Header("Trigger Dialogue Data")]
    [SerializeField] private string[] triggerSpeakers;
    [SerializeField][TextArea] private string[] triggerDialogueLines;
    [SerializeField] private Sprite[] triggerPortraits;
    [SerializeField] private AudioClip[] triggerDialogueAudio;

    [SerializeField] private float typingSpeed = 0.02f;

    [Header("Joystick Control")]
    [SerializeField] private GameObject joystickControl;
    public VirtualJoystick virtualJoystick; // Optional if you want to reset it

    private AudioSource triggerAudioSource;
    private Coroutine typingCoroutine;
    private bool isTyping;
    private bool dialogueActive;
    private int step;

    private GameObject player;
    private bool hasTriggered = false;
    void Start()
    {
        triggerAudioSource = gameObject.AddComponent<AudioSource>();
        triggerNextButton.onClick.AddListener(OnNextButtonClicked);

        player = GameObject.FindGameObjectWithTag("Player");

        if (virtualJoystick == null)
        {
            virtualJoystick = FindAnyObjectByType<VirtualJoystick>();
        }
    }

    private void OnNextButtonClicked()
    {
        triggerAudioSource.Stop(); // ✅ This is already good


        if (isTyping)
        {
            CompleteTyping();
            return;
        }

        if (step >= triggerSpeakers.Length)
        {
            EndDialogue();
        }
        else
        {
            DisplayDialogueStep();
        }
    }

    public void DisplayDialogueStep()
    {
        if (!dialogueActive)
        {
            dialogueActive = true;

            // Disable movement
            if (player.GetComponent<PlayerController>() != null)
                player.GetComponent<PlayerController>().enabled = false;
            else if (player.GetComponent<PlayerWalkOnly>() != null)
                player.GetComponent<PlayerWalkOnly>().enabled = false;

            // Disable joystick
            if (joystickControl != null)
                joystickControl.SetActive(false);

            if (virtualJoystick != null)
                virtualJoystick.ResetAnalog();
        }

        triggerDialogueCanvas.SetActive(true);
        triggerSpeakerText.text = triggerSpeakers[step];
        triggerAudioSource.Stop(); // ✅ Cut audio before playing new one
        if (step < triggerDialogueAudio.Length && triggerDialogueAudio[step] != null)
        {
            triggerAudioSource.PlayOneShot(triggerDialogueAudio[step]);
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeWriterEffect(triggerDialogueLines[step]));

        if (step < triggerPortraits.Length)
        {
            triggerPortraitImage.sprite = triggerPortraits[step];
        }

        step++;
    }

    private IEnumerator TypeWriterEffect(string line)
    {
        isTyping = true;
        triggerDialogueText.text = "";

        yield return new WaitForSeconds(0.5f);

        foreach (char letter in line)
        {
            triggerDialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);

            if (!isTyping)
                yield break;
        }

        isTyping = false;
    }

    private void CompleteTyping()
    {
        if (step - 1 < triggerDialogueLines.Length)
        {
            triggerDialogueText.text = triggerDialogueLines[step - 1];
        }

        isTyping = false;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    private void EndDialogue()
    {
        triggerDialogueCanvas.SetActive(false);
        dialogueActive = false;
        step = 0;

        // Re-enable movement
        if (player != null)
        {
            if (player.GetComponent<PlayerController>() != null)
                player.GetComponent<PlayerController>().enabled = true;
            else if (player.GetComponent<PlayerWalkOnly>() != null)
                player.GetComponent<PlayerWalkOnly>().enabled = true;
        }

        // Re-enable joystick
        if (joystickControl != null)
            joystickControl.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!dialogueActive && !hasTriggered && collision.CompareTag("Player"))
        {
            hasTriggered = true; // ✅ Prevent retriggering
            step = 0;
            DisplayDialogueStep();
        }
    }
    public void ResetDialogueTrigger()
    {
        hasTriggered = false;
    }
}
