docker exec -it productlocator-postgres-dev \
psql -U productlocator -d productlocator_dev \
-c "TRUNCATE TABLE store_products, store_members, products, stores, users, audit_logs RESTART IDENTITY CASCADE;"
