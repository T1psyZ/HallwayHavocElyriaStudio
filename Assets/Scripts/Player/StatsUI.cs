using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public GameObject[] StatsSlot;
    public GameObject StatsPanel;

   

    private void Update()
    {
        if (StatsPanel != null && StatsPanel.activeSelf)
        {
            updateAllStats();
        }
    }

    public void updateHealth()
    {
        StatsSlot[0].GetComponentInChildren<TMP_Text>().text = "Health: " + Stats_Manager.instance.currentHealth;
    }

    public void updateDamage()
    {
        StatsSlot[1].GetComponentInChildren<TMP_Text>().text = "Damage: " + Stats_Manager.instance.attackDamage;
    }

    public void updateSpeed()
    {
        StatsSlot[2].GetComponentInChildren<TMP_Text>().text = "Speed: " + Stats_Manager.instance.moveSpeed;
    }

    public void updateKnockback()
    {
        StatsSlot[3].GetComponentInChildren<TMP_Text>().text = "Knockback: " + Stats_Manager.instance.knockbackForce;
    }

    public void updateAllStats()
    {
        updateHealth();
        updateDamage();
        updateSpeed();
        updateKnockback();
    }

   
}
