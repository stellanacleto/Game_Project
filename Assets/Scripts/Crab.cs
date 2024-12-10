using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    public int health = 1; // Vida do inimigo
    private Animator anim;
    private bool isDead;
    private float velMove = 2f;
    private Rigidbody2D rb;
    private bool moveE;
    [SerializeField]
    private Transform limite;
    public LayerMask groundLayer;

    void Start()
    {
        anim = GetComponent<Animator>();
        Physics2D.IgnoreLayerCollision(12,12);
        
        rb = GetComponent<Rigidbody2D>();
        moveE = true;
    }

    void Update()
    {
        if(moveE){
            rb.linearVelocity = new Vector2(-velMove, rb.linearVelocity.y);
        }
        else{
            rb.linearVelocity = new Vector2(velMove, rb.linearVelocity.y);
        }
        VerificaCol();
    }

    void VerificaCol(){
        if(!Physics2D.Raycast(limite.position, Vector2.down,0.5f, groundLayer)){
            Flip();
        } 
    }

    void Flip(){
        moveE = !moveE;
        Vector3 temp = transform.localScale;

        if(moveE){
            temp.x = Mathf.Abs(temp.x);
        }
        else{
            temp.x = -Mathf.Abs(temp.x);
        }
        transform.localScale = temp;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null && player.IsAttacking && !isDead)
            {
                health--;

                Debug.Log("Enemy Health: " + health);

                if (health <= 0)
                {
                    StartCoroutine(Die());
                }
            }
        }
    }

    IEnumerator Die()
    {
        isDead = true;
        anim.SetTrigger("die"); // Trigger da animação de morte

        yield return new WaitForSeconds(0.2f); // Aguarda a animação de morte

        Destroy(gameObject); // Remove o inimigo do jogo
    }
}
