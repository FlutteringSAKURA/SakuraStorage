using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//# NOTE: This is the script for Mangement Particle Effect
public class ParticleMgr : MonoBehaviour
{
    public Transform kuronaTr;
    public Transform zombiegirlTr;
    public GameObject kuronaAtkExpParticle;

    public GameObject hitBloodsplatter;
	public GameObject hitBloodSpltrZombie;
    //public ParticleSystem kuronaExpParticle;

    public void KuronaAtkExplosion()
    {
        //kuronaExpParticle.Play();
        GameObject kuronaAtkEft = Instantiate(kuronaAtkExpParticle, zombiegirlTr.position, Quaternion.identity);
        Destroy(kuronaAtkEft, 3.5f);
    }
    public void HitBloodEffect()
    {
        GameObject bloodEft = Instantiate(hitBloodsplatter, kuronaTr.position, Quaternion.identity);
        Destroy(bloodEft, 3.5f);
    }
	public void HitBloodEffectZombieGirl()
	{
		GameObject bloodEft1 = Instantiate(hitBloodSpltrZombie, zombiegirlTr.position, Quaternion.identity);
		Destroy(bloodEft1, 3.5f);
	}
	
}
