VERSION = v1

context:
	kubectl config use-context docker-desktop

build:
	cd ../rest_server; docker build -t tmather99/rest_server .
	cd ../dapr_client; docker build -t tmather99/dapr_client .
	cd ../dapr_server; docker build -t tmather99/dapr_server .

push:
	docker push tmather99/rest_server
	docker push tmather99/dapr_client
	docker push tmather99/dapr_server

dashboard:
	kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.7.0/aio/deploy/recommended.yaml
	kubectl apply -f https://gist.githubusercontent.com/dahlsailrunner/bbd453f3bb6259b66c08a70d0908283f/raw/5727723217e2df4b65d8933adf04d009cfb0fe3f/local-dashboard-account.yml

rm-dashboard:
	kubectl delete -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.7.0/aio/deploy/recommended.yaml
	kubectl delete -f https://gist.githubusercontent.com/dahlsailrunner/bbd453f3bb6259b66c08a70d0908283f/raw/5727723217e2df4b65d8933adf04d009cfb0fe3f/local-dashboard-account.yml

nginx:
	kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.4.0/deploy/static/provider/cloud/deploy.yaml

rm-nginx:
	kubectl delete -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.4.0/deploy/static/provider/cloud/deploy.yaml

ingress:
	kubectl apply -f dapr-ingress.yml
	kubectl apply -f kd-ingress.yml

rm-ingress:
	kubectl delete -f dapr-ingress.yml
	kubectl delete -f kd-ingress.yml

seq:
	kubectl apply -f seq.yml

rm-seq:
	kubectl delete -f seq.yml

dapr-dashboard:
	dapr dashboard -k -p 8888

pv-pvc:
	kubectl apply -f pv-pvc.yml

rm-pv-pvc:
	kubectl delete -f pv-pvc.yml

# helm repo add bitnami https://charts.bitnami.com/bitnami
# helm repo update
#	helm install redis --set "master.persistence.enabled=false" --set "slave.persistence.enabled=false" bitnami/redis
redis:
	helm install redis bitnami/redis

rm-redis:
	helm delete redis	

prometheus:
	helm install dapr-prom prometheus-community/prometheus -n dapr-monitoring \
		--set nodeExporter.hostRootfs=false \
		--set alertmanager.persistentVolume.enable=false \
		--set pushgateway.persistentVolume.enabled=false \
		--set server.persistentVolume.enabled=false

rm-prometheus::
	helm delete dapr-prom -n dapr-monitoring 

zipkin:
	kubectl apply -f zipkin.yml
	kubectl apply -f dapr/tracing.yml

rm-zipkin:
	kubectl delete -f zipkin.yml
	kubectl delete -f dapr/tracing.yml

dapr-pubsub:
	kubectl apply -f dapr/pubsub.yml

rm-dapr-pubsub:
	kubectl delete -f dapr/pubsub.yml

dapr-statestore:
	kubectl apply -f dapr/statestore.yml

rm-dapr-statestore:
	kubectl delete -f dapr/statestore.yml

dapr-cron:
	kubectl apply -f dapr/cron.yml

rm-dapr-cron:
	kubectl delete -f dapr/cron.yml
	
rest-server:
	kubectl apply -f rest-server-$(VERSION).yml
	kubectl rollout status deployment rest-server-app

undo-rest-server:
	kubectl rollout undo deployment rest-server-app
	kubectl rollout status deployment rest-server-app

rm-rest-server:
	kubectl delete -f rest-server.yml

dapr-client:
	kubectl apply -f dapr-client-$(VERSION).yml
	kubectl rollout status deployment dapr-client-app

undo-dapr-client:
	kubectl rollout undo deployment dapr-client-app
	kubectl rollout status deployment dapr-client-app

rm-dapr-client:
	kubectl delete -f dapr-client.yml

dapr-server:
	kubectl apply -f dapr-server-$(VERSION).yml
	kubectl rollout status deployment dapr-server-app

undo-dapr-server:
	kubectl rollout undo deployment dapr-server-app
	kubectl rollout status deployment dapr-server-app

rm-dapr-server:
	kubectl delete -f dapr-server.yml

deploy: dapr-client dapr-server rest-server

undo-deploy: undo-dapr-client undo-dapr-server undo-rest-server

rm-deploy: rm-dapr-client rm-dapr-server rm-rest-server

calculator:
	kubectl apply -f dapr/dotnet-subtractor.yaml
	kubectl apply -f dapr/go-adder.yaml
	kubectl apply -f dapr/node-divider.yaml
	kubectl apply -f dapr/python-multiplier.yaml
	kubectl apply -f dapr/react-calculator.yaml
	
rm-calculator:
	kubectl delete -f dapr/dotnet-subtractor.yaml
	kubectl delete -f dapr/go-adder.yaml
	kubectl delete -f dapr/node-divider.yaml
	kubectl delete -f dapr/python-multiplier.yaml
	kubectl delete -f dapr/react-calculator.yaml

#make scale REPLICAS=2
REPLICAS = 1

scale-dapr-client:
	kubectl scale deployment dapr-client-app --replicas=$(REPLICAS)

scale-dapr-server:
	kubectl scale deployment dapr-server-app --replicas=$(REPLICAS)

dapr-secrets:
	kubectl create secret generic mysecret --from-file dapr/secrets.json
	kubectl create secret generic eventcatalogdb \
          --from-literal=eventcatalogdb="Event Catalog Connection String from Kubernetes"
	
client-rollout-status:
	kubectl rollout status deployment dapr-client-app

server-rollout-status:
	kubectl rollout status deployment dapr-server-app

rm-dapr-secrets:
	kubectl delete secret mysecret
	kubectl delete secret eventcatalogdb

secrets:
	kubectl create secret tls seq-local-tls --key="certs/seq-local.key" --cert="certs/seq-local.crt"
	kubectl create secret tls zipkin-local-tls --key="certs/zipkin-local.key" --cert="certs/zipkin-local.crt"
	kubectl create secret tls rest-local-tls --key="certs/rest-local.key" --cert="certs/rest-local.crt"
	kubectl create secret tls ingress-local-tls --key="certs/ingress-local.key" --cert="certs/ingress-local.crt"
	kubectl create secret tls calculator-local-tls --key="certs/calculator-local.key" --cert="certs/calculator-local.crt"
	kubectl -n kubernetes-dashboard create secret tls kd-local-tls --key="certs/kd-local.key" --cert="certs/kd-local.crt"

rm-secrets:
	kubectl delete secret seq-local-tls
	kubectl delete secret zipkin-local-tls
	kubectl delete secret rest-local-tls
	kubectl delete secret ingress-local-tls
	kubectl delete secret calculator-local-tls
	kubectl delete secret -n kubernetes-dashboard kd-local-tls

get-token:
	kubectl -n kubernetes-dashboard create token admin-user

