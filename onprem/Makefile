VERSION = v1

context:
	kubectl config use-context kubernetes-admin@kubernetes

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

# kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.4.0/deploy/static/provider/cloud/deploy.yaml
nginx:
	kubectl apply -f ingress-nginx-controller-v1.4.0.yml

rm-nginx:
	kubectl delete -f ingress-nginx-controller-v1.4.0.yml

ingress:
	kubectl apply -f dapr-ingress.yml 
	kubectl apply -f dapr-dashboard-ingress.yml
	kubectl apply -f kd-ingress.yml
	kubectl apply -f grafana-ingress.yml

rm-ingress:
	kubectl delete -f dapr-ingress.yml
	kubectl delete -f dapr-dashboard-ingress.yml
	kubectl delete -f kd-ingress.yml
	kubectl delete -f grafana-ingress.yml

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

redis-commander:
	kubectl apply -f redis-commander.yml

rm-redis-commander:
	kubectl delete -f redis-commander.yml

# helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
# helm repo update
# kubectl create namespace dapr-monitoring
prometheus:
	helm install dapr-prom prometheus-community/prometheus -n dapr-monitoring \
		--set nodeExporter.hostRootfs=false \
		--set alertmanager.persistentVolume.enable=false \
		--set pushgateway.persistentVolume.enabled=false \
		--set server.persistentVolume.enabled=false

rm-prometheus::
	helm delete dapr-prom -n dapr-monitoring 

# helm repo add grafana https://grafana.github.io/helm-charts
# helm repo update
grafana:
	helm install grafana grafana/grafana -n dapr-monitoring

rm-grafana:
	helm delete grafana -n dapr-monitoring

grafana-password:
	kubectl get secret --namespace dapr-monitoring grafana -o jsonpath="{.data.admin-password}" | base64 --decode ; echo

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
	kubectl create secret tls dapr-onprem-tls --key="certs/dapr-onprem.key" --cert="certs/dapr-onprem.crt" -n dapr-system
	
client-rollout-status:
	kubectl rollout status deployment dapr-client-app

server-rollout-status:
	kubectl rollout status deployment dapr-server-app

rm-dapr-secrets:
	kubectl delete secret mysecret
	kubectl delete secret eventcatalogdb
	kubectl delete secret dapr-onprem-tls -n dapr-system

secrets:
	kubectl create secret tls seq-onprem-tls --key="certs/seq-onprem.key" --cert="certs/seq-onprem.crt"
	kubectl create secret tls zipkin-onprem-tls --key="certs/zipkin-onprem.key" --cert="certs/zipkin-onprem.crt"
	kubectl create secret tls rest-onprem-tls --key="certs/rest-onprem.key" --cert="certs/rest-onprem.crt"
	kubectl create secret tls ingress-onprem-tls --key="certs/ingress-onprem.key" --cert="certs/ingress-onprem.crt"
	kubectl create secret tls calculator-onprem-tls --key="certs/calculator-onprem.key" --cert="certs/calculator-onprem.crt"
	kubectl create secret tls redis-onprem-tls --key="certs/redis-onprem.key" --cert="certs/redis-onprem.crt"
	kubectl create secret tls rabbitmq-onprem-tls --key="certs/rabbitmq-onprem.key" --cert="certs/rabbitmq-onprem.crt"
	kubectl create secret tls react-onprem-tls --key="certs/react-onprem.key" --cert="certs/react-onprem.crt"
	kubectl create secret tls grafana-onprem-tls --key="certs/grafana-onprem.key" --cert="certs/grafana-onprem.crt"
	kubectl -n kubernetes-dashboard create secret tls kd-onprem-tls --key="certs/kd-onprem.key" --cert="certs/kd-onprem.crt"

rm-secrets:
	kubectl delete secret seq-onprem-tls
	kubectl delete secret zipkin-onprem-tls
	kubectl delete secret rest-onprem-tls
	kubectl delete secret ingress-onprem-tls
	kubectl delete secret calculator-onprem-tls
	kubectl delete secret redis-onprem-tls
	kubectl delete secret rabbitmq-onprem-tls
	kubectl delete secret react-onprem-tls
	kubectl delete secret grafana-onprem-tls
	kubectl delete secret -n kubernetes-dashboard kd-onprem-tls

get-token:
	kubectl -n kubernetes-dashboard create token admin-user

