DOCKER_IMAGE_PREFIX ?=pubsub-
APPS                ?=node-subscriber python-subscriber csharp-subscriber react-form

include ../docker.mk
include ../validate.mk

TARGET_DOTNET_PLATFORM = $(TARGET_ARCH)
ifeq ($(TARGET_DOTNET_PLATFORM),amd64)
  TARGET_DOTNET_PLATFORM = x64
endif

build-csharp-subscriber-local:
	cd csharp-subscriber && dotnet restore -r linux-$(TARGET_DOTNET_PLATFORM)
	cd csharp-subscriber && dotnet publish -c Release -o out

build-csharp-subscriber: build-csharp-subscriber-local

# helm repo add bitnami https://charts.bitnami.com/bitnami
# helm repo update
#	helm install redis --set "master.persistence.enabled=false" --set "slave.persistence.enabled=false" bitnami/redis
redis:
	helm install redis bitnami/redis

rm-redis:
	helm delete redis	

redis-commander:
	kubectl apply -f deploy/redis-commander.yml

rm-redis-commander:
	kubectl delete -f deploy/redis-commander.yml

dapr-pubsub-redis:
	kubectl apply -f deploy/dapr-pubsub-redis.yml

rm-dapr-pubsub-redis:
	kubectl delete -f deploy/dapr-pubsub-redis.yml

dapr-pubsub-rabbitmq:
	kubectl apply -f deploy/dapr-pubsub-rabbitmq.yml

rm-dapr-pubsub-rabbitmq:
	kubectl delete -f deploy/dapr-pubsub-rabbitmq.yml

csharp-sub:
	kubectl apply -f deploy/csharp-subscriber.yml
	kubectl rollout status deploy/csharp-subscriber

rm-csharp-sub:
	kubectl delete -f deploy/csharp-subscriber.yml

node-sub:
	kubectl apply -f deploy/node-subscriber.yml
	kubectl rollout status deploy/node-subscriber

rm-node-sub:
	kubectl delete -f deploy/node-subscriber.yml

python-sub:
	kubectl apply -f deploy/python-subscriber.yml
	kubectl rollout status deploy/python-subscriber

rm-python-sub:
	kubectl delete -f deploy/python-subscriber.yml

react-pub:
	kubectl apply -f deploy/react-form.yml
	kubectl rollout status deploy/react-form

rm-react-pub:
	kubectl delete -f deploy/react-form.yml

deploy: react-pub csharp-sub node-sub python-sub

rm-deploy: rm-react-pub rm-csharp-sub rm-node-sub rm-python-sub

rabbit-rbac:
	kubectl create -f deploy/rabbit-rbac.yml

rm-rabbit-rbac:
	kubectl delete -f deploy/rabbit-rbac.yml

# helm repo add bitnami https://charts.bitnami.com/bitnami
rabbitmq:
	helm install rabbitmq \
	--set auth.username=admin,auth.password=admin,auth.erlangCookie=secretcookie,replicaCount=3 \
	bitnami/rabbitmq
	
rm-rabbitmq:
	helm delete rabbitmq

rabbitmq-management:
	kubectl port-forward --namespace default svc/rabbitmq 15672:15672
	