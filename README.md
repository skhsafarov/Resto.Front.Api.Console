# Resto Front API Console Logger

Кросс-процессная консоль для мониторинга и отладки плагинов iiko Resto Front API.
Показывает логи в реальном времени через Named Pipes с цветным форматированием.

- Компоненты:
  - `Resto.Front.Console.exe` — монитор (оконная консоль)
  - `Resto.Front.Api.Console.Client.dll` — клиентская библиотека для плагина
- Требования: .NET Framework 4.7.2

## Установка (в проект плагина)
- Установите NuGet-пакет: `Resto.Front.Api.Console`
- Добавьте алиас, чтобы избежать конфликта имен с `System.Console`:

```csharp
using Console = RestoFrontApiConsole.Console;
```

## Быстрый старт
В коде плагина просто пишите в `Console` — сообщения появятся в окне монитора.

```csharp
using Console = RestoFrontApiConsole.Console;

public class Plugin
{
    public void Initialize()
    {
        Console.WriteLine("[INFO] Плагин инициализирован");
        try
        {
            // ваш код
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ERROR] {0}", ex);
        }
    }
}
```

## Запуск монитора
- Запустите `Resto.Front.Console.exe` (входит в пакет).
- Клиентская библиотека попытается автоматически запустить монитор, если `Resto.Front.Console.exe` находится рядом с DLL плагина.
- Если автозапуск не сработал — запустите монитор вручную.

## Цвета и форматирование
- `[ERROR]` — красный
- `[WARN]` — желтый
- `[INFO]` или `success` — зеленый
- Строки с `===` или `***` — голубой

## Завершение работы
- Закройте окно монитора или `Ctrl+C`.
- В плагине при завершении можно вызвать `Console.Shutdown()` для корректного закрытия соединения.

## Решение проблем
- Нет вывода:
  - Убедитесь, что запущен `Resto.Front.Console.exe`.
  - Проверьте, что антивирус/политики не блокируют Named Pipes.
  - Запустите монитор от имени пользователя, под которым работает плагин.
- Конфликт имен с `System.Console` — используйте алиас: `using Console = RestoFrontApiConsole.Console;`.
- Автозапуск не работает — положите `Resto.Front.Console.exe` рядом с DLL плагина или запустите вручную.

## Лицензия
MIT
