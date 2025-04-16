using System.Collections;
using System.Drawing;
using System.Threading;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [Header("Info Unit")]
    public string unitName;
    public float unitTime; //Tiempo que va a tardar el personaje en atacar

    [Header("HP")]
    public int maxHP;
    public int currentHP;

    [Header("Attack Variables")]
    [SerializeField] float timeAnimationAttack; //tiempo que tarda la anim d ataque en ejecutarse
    [SerializeField] float speed; //La velocidad ala que se va a mover el personaje cuando va a atacar
    [SerializeField] float offset; //la distancia a la que se va a quedar el personaje de su rival

    public int damage;
    public int healAmount;

    Vector3 startedPosition;
    Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        startedPosition = transform.position;
    }

    //Metodo que se va a encargar de mover al personaje hacia su rival y ejecutar el ataque
    //Point representa la posicion del enemigo
    public IEnumerator Attacking(Vector3 point)
    {
        Debug.Log("ME MUEVO!");
        anim.SetBool("Moving", true);
        while (Vector3.Distance(transform.position, point) >= offset)
        {
            transform.position = Vector3.MoveTowards(transform.position, point, speed * Time.deltaTime);
            yield return null;
        }
        //Ha llegado hasta donde esta su rival
        Debug.Log("TE PEGO!");

        anim.SetTrigger("Attack"); //ejecuto la anim de ataque
        yield return new WaitForSeconds(timeAnimationAttack);  //espero el tiempo de la anim de ataque
    }

    public IEnumerator MovingToInitPosition()
    {
        while (Vector3.Distance(transform.position, startedPosition) >= 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startedPosition, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = startedPosition;
        anim.SetBool("Moving", false);
    }

    //este metodo devuelve un booleano y me dice si el personaje esta meurto o no
    //vamos a llmarlo desde el script que controla la batalla
    public bool TakeDamage (int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0) return true;
        else return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }
}
