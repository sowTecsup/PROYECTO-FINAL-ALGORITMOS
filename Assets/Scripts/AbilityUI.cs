using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AbilityUI : MonoBehaviour
{
    [Header("Configuración")]
    public string abilityKey = "Q";   // Q, W, E, R
    public Button upgradeButton;
    public TextMeshProUGUI levelText;

    public float waitTimeout = 5f;

    private GameManager gm;
    private PlayerStats playerStats;
    private Ability abilityRef;

    IEnumerator Start()
    {
        float timer = 0f;

        while ((GameManager.Instance == null ||
                GameManager.Instance.abilitySystem == null ||
                !GameManager.Instance.abilitySystem.abilities.ContainsKey(abilityKey) ||
                GameManager.Instance.playerStats == null)
               && timer < waitTimeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (GameManager.Instance == null || GameManager.Instance.abilitySystem == null || GameManager.Instance.playerStats == null)
        {
            Debug.LogError("AbilityUI: GameManager o PlayerStats no están listos después del timeout.");
            enabled = false;
            yield break;
        }

        gm = GameManager.Instance;
        playerStats = gm.playerStats;
        abilityRef = gm.abilitySystem.abilities[abilityKey];

        if (upgradeButton == null)
        {
            Debug.LogError("AbilityUI: upgradeButton no asignado.");
            enabled = false;
            yield break;
        }

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(OnUpgradeClicked);

        upgradeButton.gameObject.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (!enabled || abilityRef == null || playerStats == null) return;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (abilityRef == null || playerStats == null) return;

        bool hasPoints = playerStats.skillPoints > 0;
        bool notMax = abilityRef.Level < abilityRef.MaxLevel;
        bool canUpgrade = hasPoints && notMax;

        if (abilityKey == "R" && playerStats.playerLevel < 5)
            canUpgrade = false;

        upgradeButton.gameObject.SetActive(canUpgrade);

        if (levelText != null)
            levelText.text = "Lv " + abilityRef.Level.ToString();
    }

    private void OnUpgradeClicked()
    {
        if (playerStats == null) return;
        playerStats.SpendSkillPoint(abilityKey);
        UpdateUI();
    }

    void OnDestroy()
    {
        if (upgradeButton != null)
            upgradeButton.onClick.RemoveListener(OnUpgradeClicked);
    }
}