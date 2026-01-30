# API Endpoints — ProductLocator

## Conventions
- IDs: `int`
- Errors: `{ message, errors? }`
- Status: `200`, `201`, `400`, `404`, `409`

---

## Product

**GET** `/api/product`
- 200 → list (can be empty)

**GET** `/api/product/{productId}`
- Validations: product exists
- 200 → product | 404

**POST** `/api/product`
- Validations: barcode unique
- 201 | 400 | 409

---

## Store

**GET** `/api/store`
- 200 → list (can be empty)

**GET** `/api/store/{storeId}`
- Validations: store exists
- 200 | 404

**POST** `/api/store`
- Validations: name + location unique
- 201 | 400 | 409

---

## StoreProduct

**GET** `/api/store/{storeId}/product`
- Validations: store exists
- 200 → list | 404

**GET** `/api/store/{storeId}/product/{productId}`
- Validations: store exists | product exists | aisle exists | store product exists
- 200 | 404

**POST** `/api/store/{storeId}/product`
- Validations: store + product exist, not duplicate
- 201 | 400 | 404 | 409

---

## StoreAisle

**GET** `/api/store/{storeId}/aisle`
- Validations: store exists
- 200 → list (can be empty) | 404

**GET** `/api/store/{storeId}/aisle/{aisleId}`
- Validations: store exists | store aisle exists
- 200 | 404

**POST** `/api/store/{storeId}/aisle`
- Validations: store exists | aisle name unique per store
- 201 | 400 | 409
