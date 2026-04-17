using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimentacoesVariadas : MonoBehaviour
{
    public enum TipoDeForcaNoCorpo
    {
        UsandoTranslate,
        UsandoRigidbody
    }

    public enum TipoDeEixoDeMovimentacao
    {
        Horizontal,
        Vertical,
        Ambos
    }

    public enum AcoesEspeciais
    {
        Pular,
        Atacar,
        Defender
    }


    #region Variaveis
    [Header("Configurações de Movimentação")]
    public TipoDeForcaNoCorpo tipoDeMovimentacao = TipoDeForcaNoCorpo.UsandoRigidbody;
    public TipoDeEixoDeMovimentacao eixoDeMovimentacao = TipoDeEixoDeMovimentacao.Horizontal;
    public float velocidade = 5f;

    [Header("Configurações de Ação Especial")]
    public AcoesEspeciais acaoEspecialAtual = AcoesEspeciais.Pular;
    public float forcaPulo = 10f;
    public GameObject bullet;
    public float duracaoDefesa = 1f;


    Vector2 MovementInput;
    Rigidbody2D rb;

    bool isPausedGame = false;

    #endregion
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(isPausedGame)
            return;

        switch (tipoDeMovimentacao)
        {
            case TipoDeForcaNoCorpo.UsandoTranslate:
                MovimentoTranslate();
                break;
            case TipoDeForcaNoCorpo.UsandoRigidbody:
                MovimentoComRigidbody2D();
                break;
        }
    }

    #region TRANSLATE 
    public void MovimentoTranslate()
    {
        if(MovementInput == Vector2.zero)
            return;

        Vector3 input = eixoDeMovimentacao switch
        {
            TipoDeEixoDeMovimentacao.Horizontal => new Vector3(MovementInput.x, 0, 0),
            TipoDeEixoDeMovimentacao.Vertical => new Vector3(0, MovementInput.y, 0),
            _ => new Vector3(MovementInput.x, MovementInput.y, 0)
        };

        transform.position += input * Time.deltaTime * velocidade;
    }
    #endregion

    #region RIGIDBODY

    public void MovimentoComRigidbody2D()
    {
        rb.velocity = eixoDeMovimentacao switch
        {
            TipoDeEixoDeMovimentacao.Horizontal => new Vector2(MovementInput.x * velocidade, rb.velocity.y),
            TipoDeEixoDeMovimentacao.Vertical => new Vector2(rb.velocity.x, MovementInput.y * velocidade),
            _ => new Vector2(MovementInput.x * velocidade, MovementInput.y * velocidade)
        };
    }

    #endregion

    #region Pular

    public void Pular()
    {
        if (acaoEspecialAtual != AcoesEspeciais.Pular)
            return;
        rb.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
    }

    #endregion

    #region Atacar

    public void Atacar()
    {
        if (acaoEspecialAtual != AcoesEspeciais.Atacar)
            return;
        // Instanciar a bala na posição do personagem
        Instantiate(bullet, transform.position, Quaternion.identity);
    }

    #endregion

    #region GetInputs

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnAcaoEspecial(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Ação especial ativada!");

            switch (acaoEspecialAtual)
            {
                case AcoesEspeciais.Pular:
                    Pular();
                    break;
                case AcoesEspeciais.Atacar:
                    Atacar();
                    break;
                case AcoesEspeciais.Defender:
                    // Implementar lógica de defesa
                    Debug.Log("Defendendo!");
                    break;
            }
        }
    }

    #endregion

}
