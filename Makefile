build:
	docker compose build

sql:
	docker compose up globosql

rm-sql:
	docker compose stop globosql

