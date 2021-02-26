using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    
    public GameManager gameManager;
    private TowerManager towerManager;
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

    void Awake()
    {
        towerManager = gameManager.towerManager;
    }

    void Start()
    {
        foreach (TowerInfo info in towerManager.towers) {
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
                TowerInfo towerInfo = towerManager.towers[(int) tower.type];

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
        fireRateText.text = "Fire Rate " + tower.fireRate + "/s";
        rangeText.text = "Range: " + tower.range;

        // Refund specific stuff
        int refundPrice = Mathf.CeilToInt(tower.price / 2);
        for (int i = 0; i < tower.upgradeLevel; i++) {
            refundPrice += Mathf.CeilToInt(towerInfo.upgrades[i].upgradePrice / 2);
        }

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
            
            damageText.text += " (-> " + nextUpgrade.damage + ")";
            fireRateText.text += " (-> " + nextUpgrade.fireRate + ")";
            rangeText.text += " (-> " + nextUpgrade.range + ")";
            priceText.text = "Cost: " + nextUpgrade.upgradePrice;

            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => {
                if (gameManager.playerManager.UseCoins(nextUpgrade.upgradePrice)) {
                    tower.Upgrade(nextUpgrade);

                    UpdateUpgradeMenu(tower, towerManager.towers[(int) tower.type]);
                }
            });

            UpdateUpgradeButton();

            priceText.enabled = true;
            upgradeButton.gameObject.SetActive(true);
        } else {
            priceText.enabled = false;
            upgradeButton.gameObject.SetActive(false);
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

    public void StartPlacingTower(TowerInfo towerInfo) 
    {
        currentlyPlacing = towerInfo;
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
            Destroy(tower.gameObject);
        }

        UpdateButtonColors();
    }

    public void UpdateButtonColors() 
    {
        foreach (TowerInfo info in towerManager.towers) {
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