using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that stores data needed for different weapon types
[System.Serializable]
public class PlayerWeapon
{
    public GameObject BulletPrefab = null;

    public float BaseFireRate = 1.0f;
    public int ProjectileCount = 1;

    public float SpreadAngle = 15.0f;
    public float RandomSpread = 0.0f;

    public Vector2 ProjectileSpeedRange = new Vector2(10, 11);
}


public class PlayerShooting : MonoBehaviour
{
    public PlayerWeapon CurrentWeapon;

    public GameObject WeaponObject;

    public float WeaponLerp = 0.5f;

    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDir = mousePos - transform.position;
        float mouseAngle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;

        Vector3 angle = WeaponObject.transform.eulerAngles;
        angle.z = Mathf.LerpAngle(angle.z, mouseAngle, WeaponLerp * Time.deltaTime);

        WeaponObject.transform.eulerAngles = angle;

        //Check for lmb down
        if (Input.GetMouseButton(0))
        {
            if(timer <= 0.0f)
            {
                Shoot();

                ResetTimer();
            }
        }
    }

    void Shoot()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDir = mousePos - transform.position;
        float mouseAngle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;

        //Calculate initial angle 
        float startAngle = mouseAngle + ((float)(CurrentWeapon.ProjectileCount - 1) / 2) * -CurrentWeapon.SpreadAngle;

        for(int i = 0; i < CurrentWeapon.ProjectileCount; ++i)
        {
            float currAngle = startAngle + (CurrentWeapon.SpreadAngle * i);
            currAngle += Random.Range(-CurrentWeapon.RandomSpread, CurrentWeapon.RandomSpread);

            Vector2 dir;
            dir.x = Mathf.Cos(currAngle * Mathf.Deg2Rad);
            dir.y = Mathf.Sin(currAngle * Mathf.Deg2Rad);

            GameObject bullet = Instantiate(CurrentWeapon.BulletPrefab, transform.position, Quaternion.Euler(0, 0, currAngle));
            bullet.GetComponent<Rigidbody2D>().velocity = dir * Random.Range(CurrentWeapon.ProjectileSpeedRange.x, CurrentWeapon.ProjectileSpeedRange.y);
        }
    }

    //Set this up as a function so I can run through upgrades later
    void ResetTimer()
    {
        timer = CurrentWeapon.BaseFireRate;
    }
}
