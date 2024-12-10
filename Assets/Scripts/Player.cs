using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float Speed;
    private Rigidbody2D rig;
    public float JumpForce;

    public bool isJumping;
    public bool doubleJump;

    public int playerHealth = 3; // Vida do jogador
    public float attackDuration = 0.2f; // Duração do ataque
    private bool isAttacking; // Verifica se o jogador está atacando
    public bool IsAttacking => isAttacking;
    public float invulnerabilityDuration = 2f; // Tempo de invulnerabilidade após dano
    private bool isInvulnerable; // Indica se o jogador está invulnerável
    private Animator anim;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        Move();
        Jump();
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }
    void Move(){
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"),0f, 0f);
        transform.position += movement * Time.deltaTime * Speed;

        if (Input.GetAxis("Horizontal") > 0f)
        {
            anim.SetBool("walk", true);
            transform.eulerAngles = new Vector3(0f,0f,0f);
        }
        if (Input.GetAxis("Horizontal") < 0f)
        {
            anim.SetBool("walk", true);
            transform.eulerAngles = new Vector3(0f,180f,0f);
        }
        if (Input.GetAxis("Horizontal") == 0f)
        {
            anim.SetBool("walk", false);
        }
        
    }
    void Jump(){
        if(Input.GetButtonDown("Jump")){
            if(!isJumping){
                rig.AddForce(new Vector2(0f,JumpForce), ForceMode2D.Impulse);
                anim.SetBool("jump", true);
                doubleJump = true;
            }
            else {
                if(doubleJump){
                    rig.AddForce(new Vector2(0f,JumpForce), ForceMode2D.Impulse);
                    anim.SetBool("jump", false);
                    doubleJump = false;
                }
            }
            
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        anim.SetTrigger("attack"); // Trigger da animação de ataque

        yield return new WaitForSeconds(attackDuration); // Aguarda a duração do ataque

        isAttacking = false;
    }

    IEnumerator TakeDamage()
    {
        // Jogador recebe dano e fica invulnerável por um tempo
        isInvulnerable = true;

        playerHealth--;
        anim.SetTrigger("hit"); // Animação de dano

        Debug.Log("Player Health: " + playerHealth);

        if (playerHealth <= 0)
        {
            anim.SetTrigger("die");
            Debug.Log("Player Died!");
            yield return new WaitForSeconds(2f); // Pequeno atraso antes de reiniciar
            RestartScene();
        }

        yield return new WaitForSeconds(invulnerabilityDuration); // Aguarda o tempo de invulnerabilidade

        isInvulnerable = false;
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.layer == 8){
            anim.SetBool("jump", false);
            isJumping = false;
        }
        if (collision.gameObject.CompareTag("Enemy") && !isAttacking && !isInvulnerable)
        {
            StartCoroutine(TakeDamage());
        }
    }

    void OnCollisionExit2D(Collision2D collision){
        if(collision.gameObject.layer == 8){
            anim.SetBool("jump", true);
            isJumping = true;
        }
    }
    void RestartScene()
    {
        // Recarrega a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
