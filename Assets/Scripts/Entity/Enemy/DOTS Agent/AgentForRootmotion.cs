using System;
using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Hybrid;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
    
[RequireComponent(typeof(AgentAuthoring))]
[DisallowMultipleComponent]
public class AgentForRootmotion : MonoBehaviour
{
   /// <summary>
   /// I Just Not Update Position And Still Update Rotation, Next Position Need Update
   /// </summary>
   [Header("Default Value Not Apply In Editor Runtime")]
   [SerializeField] protected float speed = 1f;
   [SerializeField] protected float acceleration = 8;
   [SerializeField] protected float angularSpeed = 120;
   [SerializeField] protected float stoppingDistance = 0;
   [SerializeField] protected bool autoBreaking = true;
   
   private Animator _animator;
   private AgentCylinderShapeAuthoring _agentCylinderShapeAuthoring;
   
   public float Speed
   {
      get { return speed; }
      set
      {
         speed = value; 
         var data = GetRootMotionData();
         data.Speed = speed;
         SetRootMotionData(data);
      }
   }

   public float Acceleration
   {
      get { return acceleration; }
      set
      {
         acceleration = value;
         var data = GetRootMotionData();
         data.Acceleration = acceleration;
         SetRootMotionData(data);
      }
   }

   public float AngularSpeed
   {
      get { return angularSpeed; }
      set
      {
         angularSpeed = value;
         var data = GetRootMotionData();
         data.AngularSpeed = angularSpeed;
         SetRootMotionData(data);
      }
   }

   public float StoppingDistance
   {
      get { return stoppingDistance; }
      set
      {
         stoppingDistance = value;
         var data = GetRootMotionData();
         data.StoppingDistance = stoppingDistance;
         SetRootMotionData(data);
      }
   }

   public bool AutoBreaking
   {
      get { return autoBreaking; }
      set
      {
         autoBreaking = value;
         var data = GetRootMotionData();
         data.AutoBreaking = autoBreaking;
         SetRootMotionData(data);
      }
   }

   protected float _radius;
   protected float _height;

   public float Radius
   {
      get => _radius;
      set
      {
         AgentShape agentShape = GetAgentShape();
         agentShape.Radius = value;
         ChangeAgentShape(agentShape);
      }
   }

   public float Height
   {
      get => _height;
      set
      {
         AgentShape agentShape = GetAgentShape();
         agentShape.Height = value;
         ChangeAgentShape(agentShape);
      }
   }
   
   public Unity.Entities.Entity Entity { get; private set; }
   
   public AgentRootmotion DefaultRootmotion => new()
   {
      Speed = Speed,
      Acceleration = Acceleration,
      AngularSpeed = math.radians(AngularSpeed),
      StoppingDistance = StoppingDistance,
      AutoBreaking = AutoBreaking,
   };

   void Awake()
   {
      _animator = GetComponent<Animator>();
      
      var world = World.DefaultGameObjectInjectionWorld;
      Entity = GetComponent<AgentAuthoring>().GetOrCreateEntity();
      _agentCylinderShapeAuthoring = GetComponent<AgentCylinderShapeAuthoring>();
      world.EntityManager.AddComponentData(Entity, DefaultRootmotion);
   }

   private void Start()
   {
      AgentShape agentShape = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<AgentShape>(Entity);
      _radius = agentShape.Radius;
      _height = agentShape.Height;
   }

   private AgentShape GetAgentShape()
   {
      return World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<AgentShape>(Entity);
   } 
      

   private void ChangeAgentShape(AgentShape agentShape)
   {
      World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(Entity, agentShape);
   }
   
   private AgentRootmotion GetRootMotionData()
   {
      var world = World.DefaultGameObjectInjectionWorld;
      var manager = world.EntityManager;
      return manager.GetComponentData<AgentRootmotion>(Entity);
   }

   private void SetRootMotionData(AgentRootmotion data)
   {
      var world = World.DefaultGameObjectInjectionWorld;
      var manager = world.EntityManager;
      manager.SetComponentData(Entity, data);
   }
   
   private void Update()
   {
      var data = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<AgentBody>(Entity);
      var velMagnitude = math.length(data.Velocity);
      if(velMagnitude < 0.6f) velMagnitude = 0.6f;
      _animator.SetFloat(GlobalAnimation.MoveSpeed, velMagnitude);
   }

   private void OnAnimatorMove()
   {
      transform.position += _animator.deltaPosition;
      //transform.position = _animator.rootPosition;
   }

   void OnDestroy()
   {
      var world = World.DefaultGameObjectInjectionWorld;
      if (world != null)
          world.EntityManager.RemoveComponent<AgentRootmotion>(Entity);
   }

   internal class AgentForRootmotionBaker : Baker<AgentForRootmotion>
   {
      public override void Bake(AgentForRootmotion authoring)
      {
          AddComponent(GetEntity(TransformUsageFlags.Dynamic), authoring.DefaultRootmotion);
      }
   }
}