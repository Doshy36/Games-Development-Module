using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public GameManager gameManager;
    public int coins;
    public int health;

    void Start()
    {
        coins = gameManager.startingCoins;

        gameManager.shopManager.UpdateButtonColors();
    }

    public void Damage(int damage) 
    {
        health -= damage;

        if (health <= 0) {
            // End
        }
    }

    public void AddCoins(int coins) 
    {
        SetCoins(this.coins + coins);
    }

    public bool UseCoins(int coins) 
    {
        if (this.coins - coins < 0) {
            return false;
        }

        SetCoins(this.coins - coins);
        return true;
    }

    public bool HasCoins(int coins) 
    {
        if (coins <= this.coins) {
            return true;
        }
        return false;
    }

    private void SetCoins(int coins)
    {
        this.coins = coins;

        gameManager.coinText.text = coins + "";
        gameManager.shopManager.UpdateButtonColors();
    }
}
