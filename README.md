# Gates API

ASP.NET Core Web API для проверки кодов доступа к вратам.

## Запуск

```
dotnet run --project GatesApi
```

Swagger открывается по адресу `http://localhost:5277/swagger` (профиль http) или `https://localhost:7277/swagger` (профиль https).

## Endpoint

### POST /api/gates/validate

Пример запроса:

```json
{
  "accessCode": "Dragon7!"
}
```

Успешный ответ (200):

```json
{
  "isValid": true,
  "errors": [],
  "message": "Код принят. Ворота открываются."
}
```

Если код не прошёл проверку, возвращается 200 со списком всех нарушений:

```json
{
  "isValid": false,
  "errors": [
    "Нет заглавной латинской буквы.",
    "Нет специального символа.",
    "Обнаружена последовательность из трёх или более подряд идущих цифр."
  ],
  "message": "Код отклонён."
}
```

Отсутствующий, пустой или некорректный код — 400 Bad Request.

## Тесты

```
dotnet test
```
