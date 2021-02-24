using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    
    public GameManager gameManager;
    public TowerInfo[] towers;
    public Color cantAffordColor;

    [Header("Upgrade Menu")]
    public Canvas upgradeMenu;
    public Image upgradeImage;
    public Text titleText;
    public Text damageText;
    public Text fireRateText;
    public Text rangeText;
    public Text priceText;
    public Text refundText;
    public Button upgradeButton;
    public Button refundButton;

    // The tower info of the tower being dragged currently
    private TowerInfo currentlyPlacing;

    void Start()
    {
        foreach (TowerInfo info in towers) {
            info.priceText.text = info.name + "\n" + info.price + " Coins";
        }

        upgradeMenu.enabled = false;
    }

    void Update()
    {
        if (currentlyPlacing != null) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            if (hit.collider != null && hit.collider.gameObject.tag == "Tower") {
                Tower tower = hit.collider.gameObject.GetComponent<Tower>();

                if (tower.moving) {
                    return;
                }
                TowerInfo towerInfo = towers[tower.towerIndex];

                UpdateUpgradeMenu(tower, towerInfo);

                upgradeMenu.enabled = true;
            } else if (!EventSystem.current.IsPointerOverGameObject()) {
                upgradeMenu.enabled = false;
            }
        }

    }

    private void UpdateUpgradeMenu(Tower tower, TowerInfo towerInfo)
    {
        // Default updates
        upgradeImage.sprite = towerInfo.buyButton.image.sprite;
        upgradeImage.preserveAspect = true;
        titleText.text = towerInfo.name;
        damageText.text = "Damage: " + tower.damage;
        fireRateText.text = "Fire Rate " + tower.fireRate;
        rangeText.text = "Range: " + tower.range;

        // Refund specific stuff
        int refundPrice = tower.upgradeLevel == -1 ? Mathf.CeilToInt(tower.price / 2) : towerInfo.upgrades[tower.upgradeLevel].refundPrice;
        refundText.text = "Price: " + refundPrice;

        refundButton.onClick.RemoveAllListeners();
        refundButton.onClick.AddListener(() => {
            Destroy(tower.gameObject);

            gameManager.playerManager.AddCoins(refundPrice);
            upgradeMenu.enabled = false;
        });

        // If there is an upgrade available on the tower, add the extra data
        bool hasUpgrade = tower.upgradeLevel + 1 < towerInfo.upgrades.Length;
        if (hasUpgrade) {
            TowerUpgrade nextUpgrade = towerInfo.upgrades[tower.upgradeLevel + 1];
            
            damageText.text += " (-> " + nextUpgrade.newDamage + ")";
            fireRateText.text += " (-> " + nextUpgrade.newFireRate + ")";
            rangeText.text += " (-> " + nextUpgrade.newRange + ")";
            priceText.text = "Cost: " + nextUpgrade.upgradePrice;

            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => {
                if (gameManager.playerManager.UseCoins(nextUpgrade.upgradePrice)) {
                    tower.Upgrade(nextUpgrade);

                    UpdateUpgradeMenu(tower, towers[tower.towerIndex]);
                }
            });

            UpdateUpgradeButton();

            priceText.enabled = true;
            upgradeButton.enabled = true;
        } else {
            priceText.enabled = false;
            upgradeButton.enabled = false;
        }
    }

    private void UpdateUpgradeButton()
    {
        int price = int.Parse(priceText.text.Split(' ')[1]);
        if (!gameManager.playerManager.HasCoins(price)) {
            upgradeButton.interactable = false;
            upgradeButton.image.color = cantAffordColor;
        } else {
            upgradeButton.interactable = true;
            upgradeButton.image.color = Color.white;
        }
    }

    public void StartPlacingTower(int towerIndex) 
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

    public void StopPlacingTower() 
    {
        currentlyPlacing = null;

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
        if (upgradeMenu.enabled) {
            UpdateUpgradeButton();
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
    public TowerUpgrade[] upgrades;

}

[System.Serializable]
public class TowerUpgrade
{
    public GameObject upgradePrefab;
    public int upgradePrice;
    public int refundPrice;
    public float newDamage;
    public float newFireRate;
    public float newRange;
}