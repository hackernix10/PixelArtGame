﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class StorageContainer : MonoBehaviour {

	public GameObject slot;
	public int slots;
	public int slotOffset;
	public int topOffset;
	public Player playerScript;

	int slotSize;

	public ItemSlot[] itemSlots;

	public GameObject parentObject;
	public GameObject inventoryObject;
	public RectTransform description;

	protected virtual void Start() {
		Initialize();

		CreateSlotGrid("slot");
		UpdateSlots();
	}

	protected void Initialize() {
		itemSlots = new ItemSlot[slots];
		slotSize = (int)slot.GetComponent<RectTransform>().sizeDelta.x;
		description.gameObject.SetActive(false);
	}

	protected void CreateSlotGrid(string slotName) {
		Vector2 pos = new Vector2(slotOffset, -topOffset);
		for (int i = 0; i < slots; i++) {
			itemSlots[i].slot = Instantiate(slot, Vector2.zero, Quaternion.identity, parentObject.transform);
			itemSlots[i].slot.name = slotName;

			SetupSlot(i);

			itemSlots[i].slotRectTransform.anchoredPosition = pos;

			pos.x += slotOffset + slotSize;
			if (pos.x + slotSize > parentObject.GetComponent<RectTransform>().sizeDelta.x) {
				pos.x = slotOffset;
				pos.y -= slotOffset + slotSize;
			}
			itemSlots[i].slot.transform.SetParent(inventoryObject.transform, true);
		}
	}

	public virtual void SetupSlot(int number) {
		itemSlots[number].slotRectTransform = itemSlots[number].slot.GetComponent<RectTransform>();
		itemSlots[number].slotItemImage = itemSlots[number].slot.transform.Find("Item").GetComponent<Image>();
		itemSlots[number].slotCounterText = itemSlots[number].slotItemImage.transform.Find("ItemCounter").GetComponent<Text>();
		itemSlots[number].invItem = itemSlots[number].slotItemImage.GetComponent<InventoryItem>();
		itemSlots[number].invItem.slotNumber = number;
		itemSlots[number].invItem.containerScript = this;
	}

	public virtual void UpdateSlots() {
		for (int i = 0; i < itemSlots.Length; i++) {
			if (itemSlots[i].item != null) {
				itemSlots[i].slotItemImage.gameObject.SetActive(true);

				itemSlots[i].slotItemImage.sprite = itemSlots[i].item.inventorySprite;
				itemSlots[i].slotCounterText.text = itemSlots[i].itemCount.ToString();

				if(itemSlots[i].itemCount < 2) {
					itemSlots[i].slotCounterText.enabled = false;
				}
				else {
					itemSlots[i].slotCounterText.enabled = true;
				}
			}
			else {
				itemSlots[i].slotItemImage.gameObject.SetActive(false);
				itemSlots[i].itemCount = 0;
			}
		}
	}
}
public struct ItemSlot {
	public GameObject slot;
	public RectTransform slotRectTransform;
	public Image slotItemImage;
	public Text slotCounterText;
	public Item item;
	public InventoryItem invItem;
	public int itemCount;
}
