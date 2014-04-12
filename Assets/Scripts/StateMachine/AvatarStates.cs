using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class AvatarUnit 
{
	public enum AvatarState
	{
		Idling,
		Walking,
		Running,
		Dead,
		Incapacitated,
	}
	
	public class AvatarStates 
	{
		Dictionary<int, BaseState> _allStates;
		StateMachine _stateMachine;
		
		public AvatarStates(GameObject avatar) 
		{
			_allStates = new Dictionary<int, BaseState>();
			_allStates[(int) AvatarState.Idling] = new AvatarIdle(avatar);
			_allStates[(int) AvatarState.Walking] = new AvatarWalk(avatar);
			_allStates[(int) AvatarState.Running] = new AvatarRun(avatar);
			_allStates[(int) AvatarState.Dead] = new AvatarDead(avatar);
			
			_stateMachine = new StateMachine((int) AvatarState.Idling, _allStates);
		}
		
		public StateMachine GetStateMachine() 
		{
			return _stateMachine;
		}
	}
	
	public class AvatarBaseState : BaseState
	{
		protected AvatarUnit _avatarUnit;
		public AvatarBaseState(GameObject avatar, string enterAnimation) : base(avatar, enterAnimation) 
		{ 
			_avatarUnit = avatar.GetComponent<AvatarUnit>(); 
		}
		
		public void ChangeState(AvatarState state) 
		{
			_avatarUnit._stateMachine.ChangeState((int) state);
		}
	}
	
	public class AvatarIdle : AvatarBaseState
	{
		public AvatarIdle(GameObject avatar) : base(avatar, "idle") {}
		public override void OnEnter() { base.OnEnter(); }
		public override void Update() {
			if (_avatarUnit.IsMoving()) {
				if (_avatarUnit._moveQueue != null && _avatarUnit._moveQueue.Count > 1)
					ChangeState(AvatarState.Running);
				else
					ChangeState(AvatarState.Walking);
			}
		}
		public override void OnExit() {}
	}
	
	public class AvatarWalk : AvatarBaseState
	{
		public AvatarWalk(GameObject avatar) : base(avatar, "walk") {}
		public override void OnEnter() {
			base.OnEnter(); _avatarUnit._mover.moveSpeed = 3;
			if (Random.value < 0.25f)
				_avatarUnit._soundEffectPlayer.PlayWalkingSound(); 
		}
		public override void Update() {
			if (!_avatarUnit.IsMoving()) {
				ChangeState(AvatarState.Idling);
			}
		}
		public override void OnExit() {}
	}
	
	public class AvatarRun : AvatarBaseState
	{
		public AvatarRun(GameObject avatar) : base(avatar, "run") {}
		public override void OnEnter() { 
			base.OnEnter(); _avatarUnit._mover.moveSpeed = 4.5f; 
			if (Random.value < 0.25f)
				_avatarUnit._soundEffectPlayer.PlayWalkingSound(); 
		}
		public override void Update() {
			if (!_avatarUnit.IsMoving()) {
				ChangeState(AvatarState.Idling);
			}
		}
		public override void OnExit() {}
	}
	
	public class AvatarDead : AvatarBaseState
	{
		public AvatarDead(GameObject avatar) : base(avatar, "die") {}
		public override void OnEnter() { base.OnEnter(); _avatarUnit._soundEffectPlayer.PlayDeathSound(); }
		public override void Update() {}
		public override void OnExit() {}
	}
}