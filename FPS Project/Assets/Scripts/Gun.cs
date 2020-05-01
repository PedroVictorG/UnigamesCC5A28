using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gun : MonoBehaviour
{
    [Header("Gun configuration")]
    public float damage;
    public float range;
    public float firerate;
    public float waitToFirerate;
    public Camera cam;
    public ParticleSystem ammoParticle;
    public ParticleSystem bulletEject;
    public GameObject impact;
    public bool hold = false;
    public GameObject BulletHole;
    [Space]
    [Header("Ammo")]
    public int maxAmmoInPaint;
    public int AmmoInPaint;
    public int Ammo;
    public int TimeToRecharge;
    [Space]
    [Header("canvas")]
    public Text ammotxt;
    public Slider rechargeShow;
    public GameObject rechargeGo;
        

    private bool rechargeb = false;
    private int timetr;
    private PlayerController pc;

    
    void Update()
    {
        rechargeShow.value = timetr;
        ammotxt.text = AmmoInPaint + "/" + Ammo;
        
        if (Input.GetButtonDown("Fire1") && pc.enableMouse == true)
            hold = true;
       
        if (Input.GetButtonUp("Fire1"))
            hold = false;
        
        if (hold == true)
            waitToFirerate += 1;
        
        if (waitToFirerate > firerate && AmmoInPaint > 0)
            Shoot();
        
        if (Input.GetButtonDown("Recharge") && AmmoInPaint != maxAmmoInPaint && Ammo != 0 && rechargeb == false && pc.enableMouse == true)
        {
            rechargeGo.SetActive(true);
            rechargeb = true;
        }
        
        if(rechargeb == true)
        {
            pc.anim.SetBool("reloading", true);
            if(timetr > TimeToRecharge)
            {
                for (int i = 0; i < maxAmmoInPaint; i++)
                {
                    if (AmmoInPaint < maxAmmoInPaint && Ammo > 0)
                    {
                        Ammo -= 1;
                        AmmoInPaint += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                rechargeb = false;
                timetr = 0;
                rechargeGo.SetActive(false);
            }
            else
            {
                timetr += 1;
            }
        } 
        else
        {
            pc.anim.SetBool("reloading", false);
        }
    }
    
    private void Start()
    {
        rechargeShow.maxValue = TimeToRecharge;
        pc = (FindObjectOfType(typeof(PlayerController)) as PlayerController);
    }
    void Shoot()
    {
            pc.anim.Play("shoot");
            bulletEject.Play();
            waitToFirerate = 0;
            AmmoInPaint -= 1;
            ammoParticle.Play();
            RaycastHit hit;
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
            {
                Debug.Log("Mirando em: " + hit.transform.name);

                ObjectDestroyable ob = hit.transform.GetComponent<ObjectDestroyable>();
                if (ob != null)
                    ob.takeDamage(damage);


                GameObject impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
                Instantiate(BulletHole, hit.point, Quaternion.LookRotation(hit.normal));
        }
       
    }
}
