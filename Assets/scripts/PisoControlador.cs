using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PisoControlador : MonoBehaviour
{
    [Tooltip("Tag do jogador que ativa este piso (ex: PlayerAmarelo, PlayerAzul, PlayerVermelho)")]
    public string tagJogadorControlador;

    [Tooltip("Tags dos outros jogadores que devem ser bloqueados se o controlador não estiver presente")]
    public string[] tagsOutrosJogadores = { "PlayerAmarelo", "PlayerAzul", "PlayerVermelho" };

    private HashSet<Collider2D> controladoresNoPiso = new HashSet<Collider2D>();
    private HashSet<Collider2D> jogadoresNoPiso = new HashSet<Collider2D>();

    private Collider2D pisoCol;

    void Awake()
    {
        pisoCol = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var tag = collision.gameObject.tag;

        if (tag == tagJogadorControlador)
        {
            controladoresNoPiso.Add(collision.collider);
        }

        if (System.Array.Exists(tagsOutrosJogadores, t => t == tag) && tag != tagJogadorControlador)
        {
            AtualizarPermissao(collision.collider);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var tag = collision.gameObject.tag;

        if (System.Array.Exists(tagsOutrosJogadores, t => t == tag) && tag != tagJogadorControlador)
        {
            AtualizarPermissao(collision.collider);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var tag = collision.gameObject.tag;

        if (tag == tagJogadorControlador)
        {
            controladoresNoPiso.Remove(collision.collider);

            // Se nenhum controlador mais estiver presente, bloqueia os outros
            if (controladoresNoPiso.Count == 0)
            {
                foreach (var jogador in jogadoresNoPiso)
                {
                    if (jogador != null)
                        Physics2D.IgnoreCollision(jogador, pisoCol, true);
                }

                jogadoresNoPiso.Clear();
            }
        }

        if (System.Array.Exists(tagsOutrosJogadores, t => t == tag) && tag != tagJogadorControlador)
        {
            jogadoresNoPiso.Remove(collision.collider);
        }
    }

    private void AtualizarPermissao(Collider2D jogador)
    {
        if (controladoresNoPiso.Count > 0)
        {
            Physics2D.IgnoreCollision(jogador, pisoCol, false);
            jogadoresNoPiso.Add(jogador);
        }
        else
        {
            Physics2D.IgnoreCollision(jogador, pisoCol, true);
        }
    }
}
