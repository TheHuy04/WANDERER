using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public TMP_Text manaBarText;
    public Slider manaBarSlider;

    private ManaAble playerManaAble;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("No player found with tag 'Player'");
            return;
        }

        playerManaAble = player.GetComponent<ManaAble>();

        if (playerManaAble == null)
        {
            Debug.LogError("No ManaAble component found on player");
            return;
        }
    }

    private void Start()
    {
        if (playerManaAble != null)
        {
            manaBarSlider.value = CalculateSliderPercentage(playerManaAble.Mana, playerManaAble.MaxMana);
            manaBarText.text = "MP " + playerManaAble.Mana + " / " + playerManaAble.MaxMana;
        }
    }

    private void OnEnable()
    {
        if (playerManaAble != null)
        {
            playerManaAble.manaChanged.AddListener(OnPlayerManaChanged);
        }
    }

    private void OnDisable()
    {
        if (playerManaAble != null)
        {
            playerManaAble.manaChanged.RemoveListener(OnPlayerManaChanged);
        }
    }

    private float CalculateSliderPercentage(float currentMana, float maxMana)
    {
        return currentMana / maxMana;
    }

    private void OnPlayerManaChanged(int newMana, int maxMana)
    {
        manaBarSlider.value = CalculateSliderPercentage(newMana, maxMana);
        manaBarText.text = "MP " + newMana + " / " + maxMana;
    }
}
