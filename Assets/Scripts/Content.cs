using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Content : MonoBehaviour
{
    private List<GameObject> _items;
    private int _currentIndexActivated;
    private APIConnector _api;


    // Start is called before the first frame update
    void Start()
    {
        _api = FindObjectOfType<APIConnector>();
        _items = new List<GameObject>();
        for (var i = 0; i < transform.childCount; i++)
        {
            _items.Add(transform.GetChild(i).gameObject);
            print(_items[i].name);
        }
        print($"itemList: {_items} - childCount = {transform.childCount.ToString()}");
        ActivateItem(0);
    }

    public void ActivateItem(int i)
    {
        if (i >= transform.childCount) i = 0;
        else if (i < 0) i = transform.childCount-1;
        
        _currentIndexActivated = i;
        var obj = transform.GetChild(_currentIndexActivated).gameObject;
        
        foreach (var n in _items)
        {
            n.SetActive(n==obj);
            var onoff = n == obj ? "On" : "Off";
            StartCoroutine(_api.SendRequest($"{onoff}Led{n.GetComponent<ObjectBehaviour>().ledNumber.ToString()}"));
            print($"{n.name} : {n.activeSelf.ToString()}");
        }
    }
    
    public int GetNextItem()
    {
        return _currentIndexActivated++ > transform.childCount ? 0 : _currentIndexActivated++;
    }
    public int GetPreviousItem()
    {
        return _currentIndexActivated-- > transform.childCount ? 0 : _currentIndexActivated--;
    }

    public ObjectBehaviour GetCurrentItem()
    {
        return _items[_currentIndexActivated].GetComponent<ObjectBehaviour>();
    } 
    
    void Update()
    {
        
    }
}
