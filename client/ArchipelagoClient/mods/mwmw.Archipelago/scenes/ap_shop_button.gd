extends ShopButton

export  var item_id = ""
export  var item_no_duplicates = false
export  var max_item_owned = - 1

func _setup():
	ap_button = true
	if get_node("/root/mwmwArchipelago").shop_hints != null:
		var data = get_node("/root/mwmwArchipelago").get_shop_hint(item_id)
		slot_name = data.name
		slot_desc = "For " + data.player + "!"


func _custom_update():
	if get_node("/root/mwmwArchipelago").has_shop_check(item_id):
		owned = true
	else:
		owned = false


func _custom_purchase():
	PlayerData._send_notification("Check sent!")
	get_node("/root/mwmwArchipelago").send_shop_check(item_id)
