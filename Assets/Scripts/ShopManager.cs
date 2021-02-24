using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    
    public GameManager gameManager;
    public TowerInfo[] towers;

    public Color cantAffordColor;

    // The tower info of the tower being dragged currently
    private TowerInfo currentlyPlacing;

    void Start()
    {
        foreach (TowerInfo info in towers) {
            info.priceText.text = info.name + "\n" + info.price + " Coins";
        }
    }

    public void PlaceTower(int towerIndex) 
    {
        if (towerIndex >= towers.Length) {
            return;
        }
        
        TowerInfo info = towers[towerIndex];

        if (!gameManager.playerManager.HasCoins(info.price)) {
            return;
        }

        Vector3 towerPosition = gameManager.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        towerPosition.z = 0;

        GameObject gameObject = Instantiate(info.prefab, towerPosition, new Quaternion());
        Tower tower = gameObject.GetComponent<Tower>();
        tower.price = info.price;
        tower.moving = true;
        
        currentlyPlacing = info;
        UpdateButtonColors();
    }

    public void PurchaseTower(Tower tower) 
    {
        currentlyPlacing = null;

        if (!gameManager.playerManager.UseCoins(tower.price)) {
            Destroy(tower);
        }

        UpdateButtonColors();
    }

    public void UpdateButtonColors() 
    {
        foreach (TowerInfo info in towers) {
            if (currentlyPlacing == info) {
                ChangeButtonColor(info, Color.white);
                info.buyButton.interactable = false;
            } else if (currentlyPlacing != null || !gameManager.playerManager.HasCoins(info.price)) {
                ChangeButtonColor(info, cantAffordColor);
                info.buyButton.interactable = false;
            } else {
                info.buyButton.interactable = true;
                ChangeButtonColor(info, Color.white);
            }
        }
    }

    public void ChangeButtonColor(TowerInfo info, Color color) 
    {
        ColorBlock colors = info.buyButton.colors;
        colors.normalColor = color;
        colors.highlightedColor = new Color(color.r - 0.1f, color.g - 0.1f, color.b - 0.1f, color.a);
        colors.selectedColor = color;
        colors.pressedColor = color;
        colors.disabledColor = color;
        info.buyButton.colors = colors;
    }
}

[System.Serializable]
public class TowerInfo 
{

    public string name;
    public GameObject prefab;
    public Button buyButton;
    public Text priceText;
    public int price;

}