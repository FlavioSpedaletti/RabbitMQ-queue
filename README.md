Baseado em: https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html

Rodar imagem docker do RabbitMQ 
```
docker run -d --hostname my-rabbitmq-server --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

Usuário e senha padrão do RabbitMQ:
guest/guest