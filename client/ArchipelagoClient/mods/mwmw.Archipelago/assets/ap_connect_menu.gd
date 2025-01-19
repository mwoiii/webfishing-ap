extends Control

var status
var APMain
var _url
var _port
var _slot_name
var _password

func _ready():
	set_process(false)
	status = get_node("Panel/Panel/VBoxContainer/connection_status")
	APMain = get_node("/root/mwmwArchipelago")
	_url = get_node("Panel/Panel/VBoxContainer/server_url/serverurl")
	_port = get_node("Panel/Panel/VBoxContainer/server_port/serverport")
	_slot_name = get_node("Panel/Panel/VBoxContainer/slot_name/slotname")
	_password = get_node("Panel/Panel/VBoxContainer/server_password/serverpassword")
	
	self.focus_mode = Control.FOCUS_ALL
	
	var cache_dict = APMain.APClient.get_connection_cache()
	if cache_dict:
		_url.text = cache_dict.url
		_port.text = cache_dict.port
		_slot_name.text = cache_dict.slot_name

func _on_close_pressed():
	self.visible = false


func _on_connect_pressed():
	APMain.connect_to_server(_url.text, _port.text, _slot_name.text, _password.text)
	


