using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Properties")]
    public int maxAmmo;
    public int currentAmmo;
    public float maxDistance = Mathf.Infinity;
    public float fireRate; //cadencia
    public float reloadTime;
    public bool auto;
    public int hitDamage = 5;
    public float hitForce;

    public LayerMask layerMask;

    private bool canShot; //si puede disparar o no
    private bool reloading;

    public Animator anim;

    public AudioSource shotSource;
    public AudioSource reloadSound;

    void Start ()
    {
        currentAmmo = maxAmmo;
        canShot = true;
        reloading = false;
	}

    void Shot()
    {
        //RESTAR AMMO
        currentAmmo--;

        //SONIDO
        shotSource.volume = Random.Range(0.12f, 0.17f);
        shotSource.pitch = Random.Range(0.98f, 1.03f);
        shotSource.Play();

        //ANIMACIÓN
        anim.SetTrigger("Shot");

        // vfx

        //CREAR RAYO, GUARDARLO Y LANZARLO
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hit.transform.name);
            Debug.DrawLine(ray.origin, hit.point, Color.red, 3);

            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                hit.transform.gameObject.GetComponent<EnemyBehaviour>().SetDamage(hitDamage);
                //Debug.LogError("Enemy Detected"); //imprime error
            }
        }

        //LLAMAR CORUTINA
        if (canShot) StartCoroutine("CooldownShot");
    }

    void Reload()
    {
        anim.SetTrigger("Reload");
        reloadSound.Play();
        if (!reloading) StartCoroutine("CooldownReload");
    }

    //CO RUTINA
    IEnumerator CooldownShot()
    {
        canShot = false;
        yield return new WaitForSeconds(fireRate);
        canShot = true;
    }
    IEnumerator CooldownReload()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
        currentAmmo = maxAmmo;
    }

    public void TryShot()
    {
        if(canShot && !reloading)
        {
            if (currentAmmo > 0) Shot();
            else Reload();
        }
    }
}
