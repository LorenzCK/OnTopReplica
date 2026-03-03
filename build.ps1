#!/usr/bin/env powershell
<#
.SYNOPSIS
    OnTopReplica 项目编译脚本
    
.DESCRIPTION
    自动化编译、清理和支持工具

.PARAMETER Configuration
    编译配置：Debug 或 Release (默认 Release)
    
.PARAMETER Action
    执行的操作：Build（编译）、Clean（清理）、Rebuild（重建）
    
.PARAMETER OpenAfterBuild
    编译完成后是否打开应用
    
.EXAMPLE
    .\build.ps1 -Configuration Release -Action Build
    .\build.ps1 -Configuration Debug -OpenAfterBuild
#>

param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release",
    
    [ValidateSet("Build", "Clean", "Rebuild")]
    [string]$Action = "Build",
    
    [switch]$OpenAfterBuild,
    
    [switch]$ShowHelp
)

# 颜色定义
function Write-Success($msg) { Write-Host "✅ $msg" -ForegroundColor Green }
function Write-Error($msg) { Write-Host "❌ $msg" -ForegroundColor Red }
function Write-Warning($msg) { Write-Host "⚠️  $msg" -ForegroundColor Yellow }
function Write-Info($msg) { Write-Host "ℹ️  $msg" -ForegroundColor Cyan }

# 显示帮助
if ($ShowHelp) {
    Get-Help $PSCommandPath -Full
    exit
}

# 项目路径
$scriptPath = Split-Path -Parent $PSCommandPath
$projectPath = $scriptPath
$solutionFile = Join-Path $projectPath "src\OnTopReplica.sln"
$exePath = Join-Path $projectPath "src\OnTopReplica\bin\$Configuration\OnTopReplica.exe"

Write-Host "====================================================" -ForegroundColor Magenta
Write-Host "  OnTopReplica build helper" -ForegroundColor Magenta
Write-Host "====================================================" -ForegroundColor Magenta
Write-Info "配置: $Configuration"
Write-Info "操作: $Action"
Write-Info ""

# 检查解决方案文件
if (-not (Test-Path $solutionFile)) {
    Write-Error "找不到解决方案文件：$solutionFile"
    Write-Info "请在项目根目录运行此脚本"
    exit 1
}

# 查找 MSBuild
Write-Info "正在查找 MSBuild..."

# 如果已经在 PATH 中，直接使用
$cmd = Get-Command msbuild -ErrorAction SilentlyContinue
if ($cmd) {
    $msbuildPath = $cmd.Source
    Write-Success "MSBuild 在 PATH 中，可执行路径：$msbuildPath"
} else {
    # 预定义的一些常见安装位置
    $msbuildCandidates = @(
        "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
        "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
        "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
        "C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe",
        "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe",
        "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe",
        "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe",
        "C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe"
    )
    foreach ($candidate in $msbuildCandidates) {
        if (Test-Path $candidate) {
            $msbuildPath = $candidate
            Write-Success "找到 MSBuild: $candidate"
            break
        }
    }
}

# 如果仍然没有找到，尝试使用 vswhere
if (-not $msbuildPath) {
    $vswhere = "C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe"
    if (Test-Path $vswhere) {
        Write-Info "正在通过 vswhere 查找安装路径..."
        $vsPath = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath
        if ($vsPath) {
            $possible = Join-Path $vsPath "MSBuild\Current\Bin\MSBuild.exe"
            if (Test-Path $possible) {
                $msbuildPath = $possible
                Write-Success "通过 vswhere 找到 MSBuild: $msbuildPath"
            }
        }
    }
}

if (-not $msbuildPath) {
    Write-Error "找不到 MSBuild.exe"
    Write-Host ""
    Write-Warning "需要安装 Visual Studio 或 Visual Studio Build Tools"
    Write-Host "请访问: https://visualstudio.microsoft.com/downloads/" -ForegroundColor Cyan
    Write-Host ""
    Write-Info "或者使用以下命令安装 Build Tools:"
    Write-Host "  choco install visualstudio2022buildtools" -ForegroundColor Gray
    exit 1
}

# 执行清理（如果需要）
if ($Action -eq "Clean" -or $Action -eq "Rebuild") {
    Write-Info "清理输出文件..."
    $binPath = Join-Path $projectPath "src\OnTopReplica\bin"
    $objPath = Join-Path $projectPath "src\OnTopReplica\obj"
    
    if (Test-Path $binPath) {
        Remove-Item $binPath -Recurse -Force | Out-Null
        Write-Success "已清理 bin 文件夹"
    }
    
    if (Test-Path $objPath) {
        Remove-Item $objPath -Recurse -Force | Out-Null
        Write-Success "已清理 obj 文件夹"
    }
}

# 编译
if ($Action -eq "Build" -or $Action -eq "Rebuild") {
    Write-Host ""
    Write-Info "开始编译 $Configuration 版本..."
    Write-Info "命令: & '$msbuildPath' '$solutionFile' \"/p:Configuration=$Configuration\" \"/p:Platform=Any CPU\" /v:minimal"
    Write-Host ""
    
    & $msbuildPath $solutionFile "/p:Configuration=$Configuration" "/p:Platform=Any CPU" "/v:minimal"
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Success "编译成功！"
        
        if (Test-Path $exePath) {
            Write-Success "生成文件: $exePath"
            
            # 显示文件大小和构建时间
            $file = Get-Item $exePath
            $size = [math]::Round($file.Length / 1KB, 2)
            Write-Info "文件大小: $size KB"
            Write-Info "生成时间: $($file.LastWriteTime)"
        } else {
            Write-Warning "警告：找不到生成的 EXE 文件"
        }
        
        # 打开应用（如果指定）
        if ($OpenAfterBuild) {
            Write-Info ""
            Write-Info "启动应用..."
            & $exePath
        } else {
            Write-Info ""
            Write-Info "To run the application, execute: powershell -Command '$exePath'"
        }
    } else {
        Write-Host ""
        Write-Error "编译失败！"
        Write-Info "请检查上面的错误信息"
        exit 1
    }
}

Write-Host ""
Write-Host "════════════════════════════════════════════════════" -ForegroundColor Magenta
