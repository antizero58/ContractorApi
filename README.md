# ContractorApi
Contractor REST API ASP.Net Core 3.1 

REST api на asp.net core 3.0 для управления контрагентами.

Контрагент содержит следующие атрибуты

  id: int

  name: string

  fullname: string (не указывается, будет получен из dadata)

  type: enum (Юр.лицо, ИП)

  inn: string

  kpp: string

В качестве БД LiteDB (https://www.litedb.org/). (standalone NoSql db )

Валидация и логика:

  - name, type, inn не могут быть пустыми, kpp не может быть пусто у Юр.лица

  - при создании контрагента проверяется его наличие в ЕГРЮЛ по полям inn kpp для Юр.лица и по inn для ИП, через сервис dadata.ru (https://dadata.ru/api/find-party/),

  - если организация с указанными inn kpp или ИП с указанным inn не существует, выдаётся ошибка

  - при создании из ответа dadata записывать full_with_opf — полное наименование с ОПФ в поле fullname

Добавлен swagger-ui к созданному api. Используется Swashbuckle https://github.com/domaindrivendev/Swashbuckle.AspNetCore в дефолтной конфигурации

Написан клиент для Contractor REST Api на RazorPages.
