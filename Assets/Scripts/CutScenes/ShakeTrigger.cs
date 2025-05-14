using UnityEngine;
using System.Collections;

public class ShakeTrigger : MonoBehaviour
{
    public GameObject exitTrigger; // Assign BoysRestroomToOutsideGym in inspector
    private bool hasShaken = false;
    private TriggerDialogue triggerDialogue;
    private Cinemachine.CinemachineImpulseSource impulse;

    [SerializeField] private float shakeDuration = 0.5f; // Time before dialogue shows

    private void Start()
    {
        triggerDialogue = GetComponent<TriggerDialogue>();
        impulse = GetComponent<Cinemachine.CinemachineImpulseSource>();

        // Disable exit trigger until shake is activated
        if (exitTrigger != null)
        {
            exitTrigger.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasShaken && other.CompareTag("Player"))
        {
            hasShaken = true;

            // Trigger the shake effect
            if (impulse != null)
            {
                impulse.GenerateImpulse();
            }

            // Start coroutine to show dialogue after the shake
            StartCoroutine(TriggerAfterShake());
        }
    }

    private IEnumerator TriggerAfterShake()
    {
        yield return new WaitForSeconds(shakeDuration);

        // Show dialogue after shake
        if (triggerDialogue != null)
        {
            triggerDialogue.DisplayDialogueStep();
        }

        // Enable exit trigger
        if (exitTrigger != null)
        {
            exitTrigger.SetActive(true);
        }
    }
}
