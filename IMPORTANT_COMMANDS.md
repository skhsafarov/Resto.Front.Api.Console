# Важные команды для работы с NuGet

## Упаковка NuGet пакета
```
nuget pack .\Resto.Front.Api.Console.nuspec
```

## Публикация NuGet пакета
```
dotnet nuget push .\Resto.Front.Api.Console.1.2.4.nupkg -s nuget.org -k <API_KEY>
```

Замените `<API_KEY>` на ваш актуальный ключ для публикации.

---

- Убедитесь, что файл `.nuspec` существует и корректен.
- Версию пакета (.nupkg) меняйте при необходимости.
- Для сборки используйте:
  ```
  dotnet build .\Resto.Front.Api.Console\Resto.Front.Api.Console.csproj -c Release
  ```
