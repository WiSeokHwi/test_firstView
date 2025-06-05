using UnityEngine;

public class PlayerHealth : Health
{
    private PlayerController player;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerController>();
        UIManager.Instance?.UpdatePlayerHealthSlider(currentHealth, maxHealth);

    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        UIManager.Instance?.UpdatePlayerHealthSlider(currentHealth, maxHealth);
        player.stateMachine.ChangeState(new PlayerHitState(player, player.stateMachine));
        
        // 여기에 UI 연동 등 추가 가능
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        UIManager.Instance?.UpdatePlayerHealthSlider(currentHealth, maxHealth);
    }

    protected override void Die()
    {
        base.Die();
       
        // 상태 머신이 있다면 죽음 상태로 전환
        player.stateMachine.ChangeState(new PlayerDeadState(player, player.stateMachine));
    }
}