# Sintering of ceramics
Описание проектов

1 - Entity 
  В нем хранится описание базы данных с помощью подхода Code first, также в нем хранятся изначальные данные, которые записываются в базу вместе с одной из миграций.

2 - Sintering of ceramics
  В нем находится визуализация интерфейсов администратора и оператора.

3 - Mathematics (Пока не сделано)
  В нем находится вся математика проекта, включая расчет ММ и графиков.

Как работать с базой

1 - С помощью Tools -> NuGet Package Manager -> Package Manager Console
  Для добавления миграции написать Migration-Add НазваниеМиграции;
  Для удаления Migration-Remove (нельзя удалить уже примененную миграцию, для этого другая команда).

2 - Открыть консоль в проекте Entity 
  Для добавления миграции dotnet ef migrations add НазваниеМиграции;
  Для удаления dotnet ef migrations remove (нельзя удалить уже примененную миграцию, для этого другая команда).

О проекте в общем

  В Program реализован Host, который отвечает за DI, в него надо добавлять новые сервисы, чтобы они были потом доступны во всем приложении, например Context базы, для его получения необходимо просто добавить его в параметр конструктора класса (который перед этим был добавлен в конфигуратор сервисов).
  В PostDeploymentScripts добавлены изначальные данные, которые написаны на SqLite для добавления в базу, если необходимо что-то изменить, то это надо сделать в запросе, потом создать новую миграцию и снова добавить нужную инициализацю, как в в миграции, которая инициализировала данные в самый первый раз.
  Если надо изменить название/тип переменной в базе, добавить что-то, то надо изменить класс в классе, создать миграцию и запустить приложение.
