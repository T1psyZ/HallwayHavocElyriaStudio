using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{

    public int damage = 1;
    public GameObject Player;
    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        collision.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
    //    }
    //}
    public void Attack()
    {
        Vector2 knockDirection = (Player.transform.position - transform.position).normalized;
        Player.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage, knockDirection * 10f);
    }
}
