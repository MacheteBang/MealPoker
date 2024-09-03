# Mealbot

## Meals
Operations relating to viewing and managing meals.

### Meal Health Check
Simple check to validate the inclusion of the Meal service.

### Request
```js
GET {{ApiHost}}/meals/health
```

### Response

```js
200 OK
```

### Create Meal
Create a new meal in the backing store.

#### Request
```js
POST {{ApiHost}}/meals
Content-Type: application/json

{
  "name": "Macaroni & Cheese",
  "description": "Mac's Famous Macaroni & Cheese"
}
```

#### Response

```js
200 OK
```
```json
{
  "mealId": "7fff53a0-9905-418c-bebd-a0bb14a8bf5b",
  "name": "Macaroni & Cheese",
  "description": "Mac's Famous Macaroni & Cheese"
}
```