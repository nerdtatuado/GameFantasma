using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float HorizontalInput;
    public float velocidade = 5f;
    public float forcaPulo = 10f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public Transform verificadorChao;
    public LayerMask camadaChao;
    public float raioChao = 0.2f;

    private int pulosRestantes = 1;
    private bool estaNoChao;

    //criação do animator
    private Animator animator;
    private int andandoHash = Animator.StringToHash("Andando");
    private int PulandoHash = Animator.StringToHash("Pulando");

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }



    void Update()
    {
        // Movimentação lateral
        HorizontalInput = Input.GetAxis("Horizontal");

        // valor animação
        //animator.SetBool(andandoHash, HorizontalInput != 0);
        if (animator != null)
{
    animator.SetBool(andandoHash, Mathf.Abs(rb.velocity.x) > 0.1f);
}
        // Verifica se está no chão
        estaNoChao = Physics2D.OverlapCircle(verificadorChao.position, raioChao, camadaChao);
        
        // Resetar pulos ao tocar no chão
        if (estaNoChao)
        {
            pulosRestantes = 1;
            
        }

        // Pular se tiver pulos disponíveis
        if (Input.GetKeyDown(KeyCode.Space) && pulosRestantes > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
            pulosRestantes--;
        }

        if (HorizontalInput < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (HorizontalInput > 0)
        {
            spriteRenderer.flipX = true;
        }
        //Debug.Log("Velocidade: " + rb.velocity);
        animator.SetBool(PulandoHash, !estaNoChao);
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(HorizontalInput * velocidade, rb.velocity.y);
    }

    // (opcional) desenha o círculo no editor
    void OnDrawGizmosSelected()
    {
        if (verificadorChao != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(verificadorChao.position, raioChao);
        }
    }

    public void PararAnimacao()
{
    if (animator != null)
    {
        animator.SetBool(andandoHash, false);
    }
}
    
    
}
