# Resto Front API Console Logger

![NuGet Version](https://img.shields.io/nuget/v/Resto.Front.Api.Console.svg)
![NuGet Downloads](https://img.shields.io/nuget/dt/Resto.Front.Api.Console.svg)

Кросс-процессная консоль для мониторинга и отладки плагинов iiko Resto Front API.
Показывает логи в реальном времени через Named Pipes с цветным форматированием.

## Компоненты

- **`Resto.Front.Console.exe`** — монитор (оконная консоль)
- **`Resto.Front.Api.Console.dll`** — клиентская библиотека для плагина
- **`Console.cs`** — упрощенный wrapper для удобного использования

## Требования

- .NET Framework 4.7.2 или выше
- Windows (Named Pipes)

## Установка

### Через NuGet Package Manager
```powershell
Install-Package Resto.Front.Api.Console
```

### Через dotnet CLI
```bash
dotnet add package Resto.Front.Api.Console
```

### Через PackageReference
```xml
<PackageReference Include="Resto.Front.Api.Console" Version="2.0.0" />
```

## Быстрый старт

### 1. Добавьте using с алиасом
```csharp
using RestoFrontApiConsole;
using Console = RestoFrontApiConsole.Console; // Избегаем конфликта с System.Console
```

### 2. Используйте в коде плагина
```csharp
public class Plugin
{
    public void Initialize()
    {
        Console.WriteLine("=== Plugin Started ===");
        Console.WriteInfo("Plugin initialized successfully");
        Console.WriteWarn("This is a warning");
        Console.WriteError("This is an error");
        
        // Форматированный вывод
        var version = "1.0.0";
        Console.WriteInfo("Plugin version: {0}", version);
        
        // Проверка подключения
        if (Console.IsConnected)
        {
            Console.WriteInfo("Console connected!");
        }
    }
    
    public void ProcessOrder(string orderId, decimal total)
    {
        try
        {
            Console.WriteLine("Processing order: {0}", orderId);
            // ... логика обработки заказа
            Console.WriteInfo("Order {0} completed: {1:C}", orderId, total);
        }
        catch (Exception ex)
        {
            Console.WriteException(ex); // Полная информация об ошибке
        }
    }
    
    public void Shutdown()
    {
        Console.WriteInfo("Plugin shutting down...");
        Console.Shutdown();
    }
}
```

## Автоматический запуск

При установке NuGet пакета консольное приложение автоматически копируется в папку сборки. При первом вызове логирования библиотека попытается запустить консоль автоматически.

## Методы логирования

### Основные методы
```csharp
Console.WriteLine("Simple message");
Console.WriteLine("Formatted: {0}", value);
Console.Write("Without newline");
```

### Уровни логирования
```csharp
Console.WriteInfo("Information message");     // [INFO] - зеленый
Console.WriteWarn("Warning message");         // [WARN] - желтый  
Console.WriteError("Error message");          // [ERROR] - красный
Console.WriteException(exception);            // Полная информация об исключении
```

### Различные типы данных
```csharp
Console.WriteLine(42);              // int
Console.WriteLine(true);            // bool
Console.WriteLine(3.14m);           // decimal
Console.WriteLine(DateTime.Now);    // DateTime
Console.WriteLine(someObject);      // object
```

## Управление подключением

```csharp
// Проверка состояния
if (Console.IsConnected)
{
    Console.WriteLine("Ready to log");
}

// Переподключение
Console.Reconnect();

// Завершение работы
Console.Shutdown();
```

## Цветовая схема в консоли

- **`[ERROR]`** — красный цвет
- **`[WARN]`** — желтый цвет
- **`[INFO]`** или `success` — зеленый цвет
- Строки с **`===`** или **`***`** — голубой цвет

## Конфигурация (опционально)

### Отключение автокопирования консоли
```xml
<PropertyGroup>
  <RestoConsoleAutoStart>false</RestoConsoleAutoStart>
</PropertyGroup>
```

### Принудительный запуск консоли после сборки
```xml
<PropertyGroup>
  <StartRestoConsole>true</StartRestoConsole>
</PropertyGroup>
```

## Структура пакета

```
Resto.Front.Api.Console/
├── lib/net472/
│   ├── Resto.Front.Api.Console.dll      # Основная библиотека
│   └── Resto.Front.Api.Console.pdb      # Символы отладки
├── tools/
│   ├── Resto.Front.Console.exe          # Консольное приложение
│   └── Resto.Front.Console.exe.config   # Конфигурация
└── build/
    └── Resto.Front.Api.Console.targets  # MSBuild targets
```

## Backward Compatibility

Для совместимости с предыдущими версиями также доступен класс `ConsoleLogger`:

```csharp
ConsoleLogger.WriteLine("Old API still works");
ConsoleLogger.WriteLine("Format: {0}", value);
if (ConsoleLogger.IsConnected) { /* ... */ }
```

## Устранение неполадок

### Нет вывода в консоли
1. Убедитесь, что запущен `Resto.Front.Console.exe`
2. Проверьте, что антивирус не блокирует Named Pipes
3. Запустите консоль от имени того же пользователя, что и плагин

### Конфликт имен с System.Console
Используйте алиас:
```csharp
using Console = RestoFrontApiConsole.Console;
```

### Автозапуск не работает
1. Убедитесь, что `Resto.Front.Console.exe` находится рядом с DLL плагина
2. Запустите консоль вручную из папки сборки
3. Проверьте права доступа к файлам

## Примеры использования

Полные примеры использования смотрите в:
- [`TestPlugin/Program.cs`](TestPlugin/Program.cs) - простой тестовый плагин
- [`Examples/ExamplePlugin.cs`](Resto.Front.Api.Console/Examples/ExamplePlugin.cs) - расширенный пример
- [`Console_Usage_Guide.md`](Resto.Front.Api.Console/Console_Usage_Guide.md) - подробное руководство

## Версии

### v2.0.0 (Текущая)
- ✅ Добавлен класс `Console.cs` для упрощенного использования
- ✅ Методы логирования разных уровней (`WriteInfo`, `WriteWarn`, `WriteError`)
- ✅ Поддержка различных типов данных
- ✅ Автоматический запуск консольного приложения
- ✅ MSBuild targets для автокопирования файлов
- ✅ Обратная совместимость с `ConsoleLogger`

### v1.x
- Базовая функциональность `ConsoleLogger`
- Named Pipes коммуникация
- Цветное форматирование

## Лицензия

MIT License - смотрите [LICENSE](LICENSE) файл для деталей.

## Поддержка

- **GitHub Issues**: [https://github.com/skhsafarov/Resto.Front.Api.Console/issues](https://github.com/skhsafarov/Resto.Front.Api.Console/issues)
- **NuGet Package**: [https://www.nuget.org/packages/Resto.Front.Api.Console](https://www.nuget.org/packages/Resto.Front.Api.Console)

---

## Contributing

1. Fork проект
2. Создайте feature branch (`git checkout -b feature/amazing-feature`)
3. Commit изменения (`git commit -m 'Add amazing feature'`)
4. Push в branch (`git push origin feature/amazing-feature`)
5. Откройте Pull Request
