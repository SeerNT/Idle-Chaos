using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TalantMove : MonoBehaviour, IDragHandler
{
    private float zoom;
    public float zoomSpeed;
    public Image map;

    public float zoomMin;
    public float zoomMax;

    void Update()
    {
        if (this.name == "BG")
        {
            zoom = (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed);
            map.transform.localScale += new Vector3(map.transform.localScale.x * zoom, map.transform.localScale.y * zoom, 0);
            Vector3 scale = map.transform.localScale;
            scale = new Vector3(Mathf.Clamp(map.transform.localScale.x, zoomMin, zoomMax), Mathf.Clamp(map.transform.localScale.y, zoomMin, zoomMax), 0);
            map.transform.localScale = scale;
        }
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.name == "BG")
            if ((this.transform.localPosition.x + eventData.delta.x) <= (2860 * this.transform.localScale.x))
            {
                if ((this.transform.localPosition.x + eventData.delta.x) >= (-2515 * this.transform.localScale.x))
                {
                    if ((this.transform.localPosition.y + eventData.delta.y) >= (-2470 * this.transform.localScale.x))
                    {
                        if ((this.transform.localPosition.y + eventData.delta.y) <= (2367 * this.transform.localScale.x))
                        {
                            this.transform.position += (Vector3)eventData.delta;
                        }
                    }
                }   
            }
        else
        {
            if ((map.transform.localPosition.y >= -730 && map.transform.localPosition.y <= 626) && (map.transform.localPosition.x >= -793 && map.transform.localPosition.x <= 1142))
            {
                map.transform.position += (Vector3)eventData.delta;
            }
        }
    }

    public void SaveMap()
    {
        SaveSystem.SaveTalantMap(transform);
    }

    public void LoadMap()
    {
        TalantMapData data = SaveSystem.LoadTalantMap();

        if (data != null)
        {
            this.transform.localPosition = new Vector3(data.posX, data.posY, 0);
            this.transform.localScale = new Vector3(data.scale, data.scale, data.scale);
        }
        else
        {
            this.transform.localPosition = new Vector3(79.59204f, -188.3408f, 0);
            this.transform.localScale = new Vector3(0.7969532f, 0.7969532f, 0);
        }
    }
}
