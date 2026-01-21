docker exec -it productlocator-postgres \
psql -U productlocator -d productlocator \
-c "TRUNCATE TABLE store_products, store_members, products, stores, users, user_global_roles, audit_logs RESTART IDENTITY CASCADE;"
