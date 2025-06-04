using UnityEngine;

public class PlayerHealth : Health
{
    private PlayerController player;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerController>();

    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        player.stateMachine.ChangeState(new PlayerHitState(player, player.stateMachine));
        
        // 여기에 UI 연동 등 추가 가능
    }

    protected override void Die()
    {
        base.Die();
       
        // 상태 머신이 있다면 죽음 상태로 전환
        player.stateMachine.ChangeState(new PlayerDeadState(player, player.stateMachine));
    }
}