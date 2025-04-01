@echo off
echo Starting Docker Engine...

:: Start Docker Desktop if it's not running
tasklist | find /i "Docker Desktop.exe" >nul
if errorlevel 1 (
    start "" "C:\Program Files\Docker\Docker\Docker Desktop.exe"
    echo Waiting for Docker to start...
    timeout /t 10 >nul
) else (
    echo Docker is already running.
)

:: Run Docker Compose
echo Starting Docker containers...
docker-compose up -d

cd API
start cmd /k "dotnet watch"

cd ../client
start cmd /k "ng serve"