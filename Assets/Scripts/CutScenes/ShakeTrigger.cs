using UnityEngine;

public class ShakeTrigger : MonoBehaviour
{
    public GameObject exitTrigger; // Assign BoysRestroomToOutsideGym in inspector
    private bool hasShaken = false;
    private TriggerDialogue triggerDialogue;
    
  

    private void Start()
    {
        triggerDialogue = GetComponent<TriggerDialogue>();  
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
            triggerDialogue.DisplayDialogueStep();
            hasShaken = true;

            // Optional: trigger shake effect manually if needed
            Cinemachine.CinemachineImpulseSource impulse = GetComponent<Cinemachine.CinemachineImpulseSource>();
            if (impulse != null)
            {
                impulse.GenerateImpulse();
               
            }

            // Enable exit trigger
            if (exitTrigger != null)
            {
                exitTrigger.SetActive(true);
               
            }

            // Optional: Destroy this trigger to prevent retriggering
            //Destroy(gameObject);
        }
    }
}
