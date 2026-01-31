extends Area2D

# Efeito que este item causa (personalizável)
@export var efeito = "ActivateFoxMask"

func _ready():
	# Conecta o sinal de área entrante automaticamente
	connect("body_entered", Callable(self, "_on_body_entered"))

func _on_body_entered(body):
	# Verifica se quem tocou é o jogador
	if body.is_in_group("player"):
		aplicar_efeito(body)
		#coletar()

func aplicar_efeito(player): player.ActivateFoxMask()
		
