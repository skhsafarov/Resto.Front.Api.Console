@echo off
echo =====================================================
echo Testing Resto.Front.Api.Console Package v2.0.0
echo =====================================================
echo.

echo Starting console monitor...
start "Console Monitor" Resto.Front.Console.exe

echo Waiting for console to initialize...
timeout /t 3 /nobreak >nul

echo Running test plugin...
TestPlugin.exe

echo.
echo Test completed! Check the console monitor window for output.
echo Press any key to exit...
pause >nul