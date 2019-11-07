using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonflowerCarnivore.ShurikenSalvo;

namespace MoonflowerCarnivore.ShurikenSalvo {
	public class AutoTarget : MonoBehaviour {
		public float speed;
		float step;
		private GameObject[] targets;
        private List<Transform> enemyTrans = new List<Transform>();
		int count=0;
		int next=0;

		void Start ()
        {
            targets = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject target in targets)
            {
                Transform trans = target.transform;
                enemyTrans.Add(trans);
            }
            
			transform.position = enemyTrans[0].position;
		}
		void Update () {
			step = speed * Time.deltaTime;
			if (count == targets.Length-1) {
				next = 0;
			} else {
				next = count+1;
			}
			transform.position = Vector3.MoveTowards(transform.position,enemyTrans[next].position,step);
			if (transform.position == enemyTrans[next].position) {
				if (count == targets.Length-1) {
					count = 0;
				} else {
					count++;
				}
			}
		}
		
	}
}