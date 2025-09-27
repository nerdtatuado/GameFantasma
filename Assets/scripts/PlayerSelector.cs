using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerSelector : MonoBehaviour
{
    public GameObject[] jogadores; // Lista com PlayerAzul, PlayerAmarelo e PlayerVermelho
    public GameObject setaIndicadora; // A seta que ficará sobre o personagem selecionado
    public CinemachineVirtualCamera virtualCamera;
    private int jogadorAtualIndex = 0;

    public float tempoVisivelSeta = 1f;
    private float tempoAtualSeta = 0f;
    private bool setaVisivel = true;
    public GameObject gameOverUI;  // arraste no inspector
    public float tempoParaReiniciar = 3f;
    private bool gameOver = false;

    public int vidasTotais = 3;//total de vidas

    void Start()
    {
        AtualizarSelecao();
        // Ignora colisão entre todos os jogadores
        for (int i = 0; i < jogadores.Length; i++)
        {
            for (int j = i + 1; j < jogadores.Length; j++)
            {
                Collider2D colA = jogadores[i].GetComponent<Collider2D>();
                Collider2D colB = jogadores[j].GetComponent<Collider2D>();

                if (colA != null && colB != null)
                {
                    Physics2D.IgnoreCollision(colA, colB);
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            jogadorAtualIndex++;
            if (jogadorAtualIndex >= jogadores.Length)
                jogadorAtualIndex = 0;

            AtualizarSelecao();
        }

    }

    void AtualizarSelecao()
    {
        for (int i = 0; i < jogadores.Length; i++)
        {
            bool ativo = i == jogadorAtualIndex;

            // Ativa/desativa controle do jogador
            var controller = jogadores[i].GetComponent<PlayerController>();
            if (controller != null)
            {
                //controller.enabled = ativo;
                if (!ativo)
{
    controller.PararAnimacao(); // força parar antes de desativar
}

controller.enabled = ativo;

                // Zera a velocidade do personagem desativado
                if (!ativo)
                {
                    Rigidbody2D rb = jogadores[i].GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.velocity = Vector2.zero;
                    }
                }
            }

            // Move a seta para cima do jogador atual
            if (ativo && setaIndicadora != null)
            {
                setaIndicadora.SetActive(true);
                setaVisivel = true;
                tempoAtualSeta = tempoVisivelSeta;

                setaIndicadora.transform.position = jogadores[i].transform.position + new Vector3(0, 1.5f, 0);
            }
            // Atualiza o alvo da Cinemachine para o jogador atual
            if (virtualCamera != null)
            {
                virtualCamera.Follow = jogadores[jogadorAtualIndex].transform;
            }
        }
    }

    void LateUpdate()
    {
        if (setaVisivel && setaIndicadora != null)
        {
            // Segue o jogador ativo
            setaIndicadora.transform.position = jogadores[jogadorAtualIndex].transform.position + new Vector3(0, 1.5f, 0);

            // Contador regressivo
            tempoAtualSeta -= Time.deltaTime;

            if (tempoAtualSeta <= 0f)
            {
                setaIndicadora.SetActive(false);
                setaVisivel = false;
            }
        }
    }

    public void MatarJogador(GameObject jogadorMorto)
    {
        // Desativa ou destrói o jogador
        jogadorMorto.SetActive(false); // ou: Destroy(jogadorMorto);

        // Remove da lista de jogadores ativos
        var novaLista = new System.Collections.Generic.List<GameObject>(jogadores);
        novaLista.Remove(jogadorMorto);
        jogadores = novaLista.ToArray();

        // Se ainda houver jogadores, atualiza a seleção
        if (jogadores.Length > 0)
        {
            jogadorAtualIndex %= jogadores.Length;
            AtualizarSelecao();
        }
        else
        {
            Debug.Log("Todos os jogadores morreram.");
            // Aqui você pode chamar Game Over ou algo do tipo
            // TODOS MORTOS — Inicia Game Over
            gameOver = true;
            if (gameOverUI != null)
                gameOverUI.SetActive(true);

            Invoke("ReiniciarFase", tempoParaReiniciar);
        }
    }

        void ReiniciarFase()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}


    public void PerderVida(GameObject jogador)
    {
        vidasTotais--;

        if (vidasTotais <= 0)
        {
            // Última vida acabou
            MatarJogador(jogador);
        }
        else
        {
            // Volta para posição inicial
            Rigidbody2D rb = jogador.GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.zero;

            jogador.transform.position = jogador.GetComponent<PlayerVida>().posicaoInicial;
        }

        Debug.Log("Vidas restantes: " + vidasTotais);
    }
}
