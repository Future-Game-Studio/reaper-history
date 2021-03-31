﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    //  hint text (example 'press E')
    public GameObject hintText;
    //  npc menu
    public GameObject shopMenu;
    //  player reference
    private Player _player = null;
    //  flag is player near to npc
    private bool _playerInZone = false;

    //  current selected item
    private UpgradeItem _selected = null;

    //  description text of selected item
    public Text descriptionText;

    //  notification of not enough money to buy something
    private GameObject _notification;

    //  if has, play it before opening shop
    [SerializeField] private bool _hasActionAnimation = false;

    void Start()
    {
        //  hide hint text and menu
        hintText.SetActive(false);
        shopMenu.SetActive(false);
        _notification = shopMenu.transform.GetChild(1).gameObject;
        _notification.SetActive(false);
    }

    void Update()
    {
        //  if pressed E and player near - open shop window
        if(Input.GetKeyDown(KeyCode.E) && _playerInZone)
        {
            StartCoroutine(ShowShopMenu());
        }
    }

    private IEnumerator ShowShopMenu()
    {
        //  play animation if has
        if (_hasActionAnimation)
        {
            GetComponentInChildren<Animator>().ResetTrigger("Idle");
            GetComponentInChildren<Animator>().SetTrigger("Action");
            yield return new WaitForSeconds(1.8f);
        }
        yield return null;

        //  hiding hint text and showing shop window
        shopMenu.SetActive(true);
        hintText.SetActive(false);
    }

    public void ApplyItem()
    {
        //  if player is not near
        if (_player == null || _selected == null)
            return;

        if(GameSaving.instance.score < _selected.Cost)
        {
            //  show notification
            _notification.SetActive(true);
            return;
        }

        if(_selected.Identificator == UpgradeItem.STATS.HP)
            _player.AddHealth(_selected.Value);
        else if (_selected.Identificator == UpgradeItem.STATS.HP_HALF)
            _player.AddHealth(_player.MAXHP/2);
        else if (_selected.Identificator == UpgradeItem.STATS.HP_FULL)
            _player.AddHealth(_player.MAXHP);
        else if(_selected.Identificator == UpgradeItem.STATS.MAXHP)
            _player.AddMaxHP(_selected.Value);
        else if(_selected.Identificator == UpgradeItem.STATS.DAMAGE)
            _player.AddDamage(_selected.Value);

        else if (_selected.Identificator == UpgradeItem.STATS.SPHERE_DAMAGE)
            _player.IncreaseSphereDamage(_selected.Value);
        else if (_selected.Identificator == UpgradeItem.STATS.SPHERE_DELAY && _player.blackHoleDelay > 2f)
            _player.DecreaseSphereDelay(_selected.Value);
        else if (_selected.Identificator == UpgradeItem.STATS.SPHERE_RADIUS)
            _player.IncreaseSphereRadius(_selected.Value);

        GameSaving.instance.Buy(_selected.Cost);
    }

    public void HideShopMenu()
    {
        if (_hasActionAnimation)
        {
            GetComponentInChildren<Animator>().SetTrigger("Idle");
        }
        _notification.SetActive(false);
        shopMenu.SetActive(false);
        hintText.SetActive(true);
        SetDefaultValues();
    }

    private void SetDefaultValues()
    {
        descriptionText.text = "";
        _selected = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerTrigger"))
        {
            //  if player enter to npc collider 
            _playerInZone = true;
            hintText.SetActive(_playerInZone);
            _player = collision.GetComponentInParent<Player>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerTrigger"))
        {
            if (_hasActionAnimation)
            {
                GetComponentInChildren<Animator>().SetTrigger("Idle");
            }
            //  if player exit from npc collider
            _playerInZone = false;
            shopMenu.SetActive(false);
            hintText.SetActive(false);
            _notification.SetActive(false);
            SetDefaultValues();
            _player = null;
        }
    }

    public void onPressItem(UpgradeItem item)
    {
        _selected = item;
        descriptionText.text = $"{item.Description} - {item.Cost}$";
    }
}
