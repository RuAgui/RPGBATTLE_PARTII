using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI namePlayer;
    [SerializeField] TextMeshProUGUI hpText;
    public Slider timeSlider;
    public GameObject panelButtons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void SetHUD(Unit unit)
    {
        namePlayer.text = unit.unitName;
        timeSlider.maxValue = unit.unitTime;
        timeSlider.value = 0;
    }

    //Mostrar por pantalla la vida del personaje (cada vez que cvambie)
    public void SetHP(int currentHP, int maxHP)
    {
        hpText.text = currentHP.ToString() + " / " + maxHP.ToString();
    }
}
