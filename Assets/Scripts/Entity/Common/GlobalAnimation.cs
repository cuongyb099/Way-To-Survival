using UnityEngine;

public static class GlobalAnimation
{
    public static readonly int RandomPose = Animator.StringToHash("Random Pose"); 
    public static readonly int Movement = Animator.StringToHash("Move"); 
    public static readonly int MovementSpeed = Animator.StringToHash("Move Speed"); 
    public static readonly int Running = Animator.StringToHash("Is Running"); 
    public static readonly int Aim = Animator.StringToHash("Aim"); 
    public static readonly int MoveHorizontal = Animator.StringToHash("Move X"); 
    public static readonly int MoveVertical = Animator.StringToHash("Move Y"); 
    public static readonly int Attack = Animator.StringToHash("Attack"); 
    public static readonly int WeaponPose = Animator.StringToHash("Weapon Pose"); 
    public static readonly int Reload = Animator.StringToHash("Reload"); 
    
    public static readonly int MoveSpeed = Animator.StringToHash("Move Speed");
    public static readonly int IsAttackAnimationEnd = Animator.StringToHash("Is Attack End");
    public static readonly int IsDeadAnimationEnd = Animator.StringToHash("Is Dead End");
}