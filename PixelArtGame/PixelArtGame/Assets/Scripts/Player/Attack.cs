﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public float hitTime, attackTime, dashForce;

	ParticleSystem attackParticleSystem;
	public List<Enemy> enemiesInRange = new List<Enemy>();
	Player playerScript;
	Rigidbody2D rb;
	CameraScript cameraScript;

	ParticleSystem.MainModule partMain;

	void Start() {
		attackParticleSystem = GetComponentInChildren<ParticleSystem>();
		playerScript = transform.parent.gameObject.GetComponent<Player>();
		rb = playerScript.GetComponent<Rigidbody2D>();
		cameraScript = Camera.main.GetComponent<CameraScript>();

		partMain = attackParticleSystem.main;
		partMain.startLifetime = attackTime;
	}

	public void SimpleAttack() {
		playerScript.playerState = Player.PlayerState.Attacking;

		SwordParticles();
		Invoke("HitEnemy", hitTime);
	}

	public void DashAttack() {
		playerScript.playerState = Player.PlayerState.Attacking;

		rb.AddForce(playerScript.facing * dashForce, ForceMode2D.Impulse);
	}

	private void SwordParticles() {
		float facingRad = Mathf.Atan2(playerScript.facing.x, playerScript.facing.y);

		partMain.startRotation = facingRad - 2.25f; //-2.25 rad ~ -130 deg, to to fix rotation

		attackParticleSystem.Play();
	}

	private void HitEnemy() {
		bool hit = false;
		for (int i = enemiesInRange.Count - 1; i > -1; i--) {
			if (enemiesInRange[i] == null) { //Removes enemy from list if it doesn't exist
				enemiesInRange.RemoveAt(i);
				break;
			}
			if (enemiesInRange[i].Damage(transform.position, 5)) { //Deals damage to enemy and removes from list if destroyed
				enemiesInRange.RemoveAt(i);
			}
			hit = true;
			cameraScript.StartShake(.5f, 5, .05f);
		}
		float attackDelay = hit ? attackTime * 0.5f : attackTime;
		Invoke("EndAttack", attackDelay);
	}

	void EndAttack() {
		playerScript.playerState = Player.PlayerState.Default;
	}
}
