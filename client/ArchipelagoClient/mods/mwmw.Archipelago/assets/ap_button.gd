extends Control

var Window = null


func _ready():
	Window = get_node("../ap_connect_menu")


func _on_ap_button_pressed():
	Window.visible = true
