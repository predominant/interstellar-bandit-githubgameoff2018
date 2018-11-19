using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	[RequireComponent(typeof(Animator))]
	public class EnemyController2d : PersonController2d
	{
		private enum EnemyControllerState
		{
			Patrol,
			Chasing,
			Dead
		}
		
		private Animator _animator;

		private EnemyControllerState _state = EnemyControllerState.Patrol;

		private float _startX = 0f;
		[SerializeField]
		private float _roamSize = 2.5f;

		private float _patrolTargetX = 0f;
		private float _lastRoam = 0f;
		[SerializeField]
		private float _roamDelay = 5f;
		[SerializeField]
		private float _walkSpeed = 1.5f;

		private new void Awake()
		{
			base.Awake();
			this._animator = this.GetComponent<Animator>();
			this._startX = this.transform.position.x;
			this._patrolTargetX = this._startX;
		}

		private void Update()
		{
			if (this._state == EnemyControllerState.Dead)
				return;
			
			if (this.ShouldRoam())
				this.Roam();

			this.FlipPlayer(this._patrolTargetX > this.transform.position.x);
			
			switch (this._state)
			{
				case EnemyControllerState.Patrol:
					var targetPos = this.transform.position;
					targetPos.x = this._patrolTargetX;
					this.transform.Translate((targetPos - this.transform.position) * Time.deltaTime * this._walkSpeed);
					break;
				case EnemyControllerState.Chasing:
					// TODO: Implement chasing
					break;
			}
		}

		private bool ShouldRoam()
		{
			return (Time.time > this._lastRoam + this._roamDelay) &&
			       Mathf.Abs(this.transform.position.x - this._patrolTargetX) < 0.1f;
		}

		private void Roam()
		{
			this._lastRoam = Time.time;
			this._state = EnemyControllerState.Patrol;
			var posx = this.transform.position.x;
			this._patrolTargetX = Random.Range(posx - this._roamSize, posx + this._roamSize);
		}
		
		protected override void Die()
		{
			this._animator.SetBool("Dead", true);
			this._state = EnemyControllerState.Dead;
		}
	}
}