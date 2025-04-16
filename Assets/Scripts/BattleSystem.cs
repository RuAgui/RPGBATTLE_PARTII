using System.Collections;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{

    public enum BattleState {Start, PlayerTurn, EnemyTurn, Won, Lost};
    public BattleState battleState;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject enemyPrefab;

    [SerializeField] Transform playerInitPos;
    [SerializeField] Transform enemyInitPos;

    [SerializeField] BattleHUD battleHUD;

    Unit playerUnit;
    Unit enemyUnit;

    void Start()
    {
        battleState = BattleState.Start;
        StartCoroutine(nameof(SetUpBattle));
    }

   IEnumerator SetUpBattle()
    {
        GameObject player = Instantiate(playerPrefab, playerInitPos.position, new Quaternion());
        playerUnit = player.GetComponent<Unit>();

        GameObject enemy = Instantiate(enemyPrefab, enemyInitPos.position, new Quaternion());
        enemyUnit = enemy.GetComponent<Unit>();

        battleHUD.SetHUD(playerUnit);
        battleHUD.SetHP(playerUnit.currentHP, playerUnit.maxHP);

        //llama a la corrutina y hasta que no termine no sigue leyendo

        yield return StartCoroutine(nameof(PlayerTime));

        //Aqui pdriamos poner algo mas que se ejecutaria despues de la corrrutina.
    }
    //Metodo que se encarga de cargar el tiempo de ataque (slider)

    IEnumerator PlayerTime()
    {
        float timePlayer = 0;

        while (timePlayer < playerUnit.unitTime)
        {
            timePlayer += Time.deltaTime;
            battleHUD.timeSlider.value = timePlayer;
            yield return null;
        }
        battleState = BattleState.PlayerTurn;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        Debug.Log("TURNO DE ZACK");
        battleHUD.panelButtons.SetActive(true);
    }

    public void OnAttackButtton()
    {
        if (battleState != BattleState.PlayerTurn)
            return;
        StartCoroutine(nameof(PlayerAttack));
    }

    public void OnHealButton()
    {

    }

    IEnumerator PlayerAttack()
    {
        ResetAttackPlayer();

        //El player ataca al rival
        yield return StartCoroutine(playerUnit.Attacking(enemyUnit.transform.position));

        //¿Ha muerto elrival?
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        yield return StartCoroutine(playerUnit.MovingToInitPosition());

        Debug.Log("El player ataca: " + enemyUnit.currentHP);

        if (isDead)
        {
            battleState = BattleState.Won;
            Debug.Log("Batalla ganada");
        }
        else
        {
            battleState = BattleState.EnemyTurn;
            Debug.Log("Turno del enemigo");
            //llamada a la corrutina del enemigo
        }

    }

    void ResetAttackPlayer()
    {
        battleHUD.panelButtons.SetActive(false);
        battleHUD.timeSlider.value = 0;
    }

}
