using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
public class playercontroller : MonoBehaviour
{
    public Text GpsStatus;
    public Text latitudeValue;
    public Text longitudeValue;
    public Text Timer;
    public float speed = 10f;
    private Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        GpsStatus.text = "Started";
        Input.compass.enabled = true;
        Input.location.Start();
        latitudeValue.text = Input.location.lastData.latitude.ToString();
        StartCoroutine(GPSloc());
    }



    IEnumerator GPSloc()
    {



        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }


        //Timer.text = "Something";
        if (!Input.location.isEnabledByUser)
            yield break;


        int maxWait = 20;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
             Timer.text = maxWait.ToString();
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            GpsStatus.text = "Time out";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            GpsStatus.text = "Unable to connect to device";
            yield break;

        }

        //GpsStatus.text = "Running";
        InvokeRepeating("UpdateGpsData", 0f, 0.03f);
        //access granted
    }
    string prev;
    private void play(string z,string x)
    {
        double d = double.Parse(prev);

       if (d< double.Parse(z) )
        {
            float amountToMove = Time.deltaTime;
            transform.Translate(dir * amountToMove);
            dir = Vector3.forward;
        }
    }

    private void UpdateGpsData()
    { 
        int flag = 0;
        string longi, lati,prev;
        if (Input.location.status == LocationServiceStatus.Running)
        {
            GpsStatus.text = "Running";
            lati= Input.location.lastData.latitude.ToString();
            longi= Input.location.lastData.longitude.ToString();
            latitudeValue.text = lati;
            longitudeValue.text = longi;
            if (flag == 0)
            { prev = string.Copy(lati); }
            play(lati,longi);
            prev = string.Copy(lati);
            flag++;
        }
        else
        {
            GpsStatus.text = "Stop";
        }
    }


}
