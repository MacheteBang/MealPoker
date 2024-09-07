# MealBot

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
Create a new meal.

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

### Get Meals
Gets all of the meals.

#### Request
```js
GET {{ApiHost}}/meals
```

#### Response
```js
200 OK
```

```json
[
  {
    "mealId": "622623e3-2a26-44dc-aebc-2e3fdd3691be",
    "name": "Macaroni & Cheese",
    "description": "Mac's Famous Macaroni & Cheese"
  },
  {
    "mealId": "7311a7bc-7876-4ca8-a8ed-d64649418cba",
    "name": "Meatloaf",
    "description": "A hunk of ground beef cooked for longer than you'd expect."
  },
  {
    "mealId": "2dc70dd1-a1a7-4bd2-9b56-6c55d7a66f53",
    "name": "Spaghetti and Meatballs",
    "description": "A classic Italian dish."
  }
]
```

### Get Meal
Gets a single meal.

#### Request
```js
GET {{ApiHost}}/meals{{mealId}}
```

#### Response
```js
200 OK
```

```json
{
  "mealId": "622623e3-2a26-44dc-aebc-2e3fdd3691be",
  "name": "Macaroni & Cheese",
  "description": "Mac's Famous Macaroni & Cheese"
}
```