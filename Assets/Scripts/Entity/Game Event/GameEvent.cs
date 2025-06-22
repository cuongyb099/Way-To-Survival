using System;
using UnityEngine;

public static class GameEvent
{
    public static Action GameCompleteEvent;
    //Trả Về số kẻ dịch hiện tại
    public static Action<GameObject> EnemySpawnEvent;
    //Trả Về số kẻ dịch hiện tại
    public static Action<int> EnemyDeadEvent;
    //Trả về wave hiện tại
    public static Action<int> WaveDoneEvent;
    //Trả về wave hiện tại
    public static Action<int> NextWaveEvent;
    //Trả về thời gian đếm ngoại của shoppingTimer
    public static Action<float> ShoppingTimeChangeEvent;
    
    public static Action OnStartGameEvent;
    public static Action OnInitializedUI;
    public static Action OnStartShoppingState;
    public static Action OnStopShoppingState;
    public static Action OnStartWinState;
    public static Action OnStopWaveWinState;	
    public static Action OnStartCombatState;
    public static Action OnStopCombatState;
    public static Action<TimeOfTheDay> OnChangeTimeOfDay;
}