using UnityEngine;
using System.Linq;
using System.Collections.Generic;
//[ExecuteInEditMode]

namespace MoonflowerCarnivore.ShurikenSalvo {
	public class ParticleHomingMultiTarget : MonoBehaviour {
		[Tooltip("Target objects. If this parameter is undefined it will assume the attached object itself which creates self chasing particle effect.")]
		private GameObject[] targets;
		[Tooltip("How fast the particle is guided to the index target.")]
		public float speed = 10f;
		[Tooltip("Cap the maximum speed to prevent particle from being flung too far from the missed target.")]
		public float maxSpeed = 50f;
		[Tooltip("How long in the projectile begins being guided towards the target. Higher delay and high particle start speed requires greater distance between attacker and target to avoid uncontrolled orbitting around the target.")]
		public float homingDelay = 1f;
		public enum TSOP{random=0,closest=1};
		[Tooltip("How each particle selects the target.")]
		public TSOP targetSelection = TSOP.random;
		//[Tooltip("Distance from target position which kills the particle. This has the similar effect of using sphere collider for the target combing with world collision or trigger in Particle System.")]
		//public float dyingRange = 0f;
		private ParticleSystem _ps_system;
		private ParticleSystem.TriggerModule _ps_trigger;
		private ParticleSystem.Particle[] _ps_particles;
		private int index;        

        private List<Transform> enemyTrans = new List<Transform>();
       
        public float minDist;
        public float maxDist = 1000;

        void OnEnable() {

            targets = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject target in targets)
            {                
                Transform trans = target.transform;                
                enemyTrans.Add(trans);                
            }

            _ps_system = GetComponent<ParticleSystem>();
			_ps_trigger = _ps_system.trigger;
			//_ps_particles = new ParticleSystem.Particle[_ps_system.maxParticles];// Before Unity 5.5
			_ps_particles = new ParticleSystem.Particle[_ps_system.main.maxParticles];// Since Unity 5.5
			for (int i = 0; i < enemyTrans.Count; i++) {
				_ps_trigger.SetCollider(i, enemyTrans[i].GetComponent<Collider>());
			}
		}
		
		void LateUpdate() {
			if (enemyTrans[0] == null)
				return;            
			
			//If you are not changing target during runtime, skip this:            
			for (int i = 0; i < enemyTrans.Count; i++)
            {                          
                _ps_trigger.SetCollider(i, enemyTrans[i].GetComponent<Collider>());                               
            }
			
			_ps_particles = new ParticleSystem.Particle[_ps_system.main.maxParticles];
			int numParticlesAlive = _ps_system.GetParticles(_ps_particles);

			for (int i = 0; i < numParticlesAlive; i++)
            {                
				float[] dist = new float[enemyTrans.Count];

				switch (targetSelection) {
					case TSOP.random:

						index = Mathf.Abs((int) _ps_particles[i].randomSeed) % enemyTrans.Count;
						break;
						
					case TSOP.closest:
						for (int j = 0; j < enemyTrans.Count; j++) {
							dist[j] = Vector3.Distance(_ps_particles[i].position, enemyTrans[j].position);                            
						}
						//index = System.Array.IndexOf(dist, dist.Min());// slower than comparing in foreach.
						float minValue = float.MaxValue;
						int minindex = -1;
						index = -1;
						foreach (float num in dist) {
							minindex++;                            
							if (num < minValue) {
								minValue = num;
								index = minindex;                                
							}                               
                        }
						break;
				}
				//Debug.Log(index);
				float ted = (enemyTrans[index].position - this.transform.position).sqrMagnitude + 0.001f;
				Vector3 diff = enemyTrans[index].position - _ps_particles[i].position;
				float diffsqrm = diff.sqrMagnitude;
				float face = Vector3.Dot(_ps_particles[i].velocity.normalized, diff.normalized);
				float f = Mathf.Abs((ted - diffsqrm)/ted) * ted * (face + 1.001f);
				//_ps_particles[i].velocity = Vector3.ClampMagnitude(_ps_particles[i].velocity + diff * speed * 0.01f * f, maxSpeed);
				float t=0;
                t += Time.deltaTime / (homingDelay * 0.01f + 0.0001f);

                if (diff.magnitude < maxDist)
                {
                    
                    _ps_particles[i].velocity = Vector3.ClampMagnitude(Vector3.Slerp(_ps_particles[i].velocity, _ps_particles[i].velocity + diff * speed * 0.01f * f, t), maxSpeed);
                }
                else
                {
                    _ps_particles[i].velocity = Vector3.ClampMagnitude(Vector3.Slerp(_ps_particles[i].velocity, transform.forward * speed, t), maxSpeed);
                }
                
				/*
				if (Vector3.Distance(_ps_particles[i].position, target[index].position) < dyingRange) {
					//_ps_particles[i].lifetime = 0f;// Before Unity 5.5
					_ps_particles[i].remainingLifetime = 0f;// Since Unity 5.5
				}
				*/
			}
			_ps_system.SetParticles(_ps_particles, numParticlesAlive);
		}   

        /*
		void OnDrawGizmosSelected() {
			Gizmos.color = Color.yellow;
			foreach (Transform i in target) {
				Gizmos.DrawSphere(i.position, dyingRange);
			}
		}
		*/
    }
}