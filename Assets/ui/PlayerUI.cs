using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] HealthBar healthBar;


    // Update is called once per frame
    void Update()
    {
        healthBar.Health = GameManager.Player.Lives;
    }
}
