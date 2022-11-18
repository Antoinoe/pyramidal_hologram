using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Content : MonoBehaviour
{
    private List<GameObject> _items;
    [SerializeField] private int _currentIndexActivated;
    // Start is called before the first frame update
    void Start()
    {
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
            print($"{n.name} : {n.activeSelf.ToString()}");
        }
    }
    //
    // public GameObject GetNextItem(int i)
    // {
    //     if (i++ > transform.childCount) i = 0;
    //     return _items[i];
    // }
    // public GameObject GetPreviousItem(int i)
    // {
    //     if (i-- < 0) i = transform.childCount;
    //     return _items[i];
    // }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ActivateItem(_currentIndexActivated + 1);
            print("Next Item");
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            ActivateItem(_currentIndexActivated - 1);
            print("Previous Item");
        }
    }
}
