@echo off

:: this script needs https://www.nuget.org/packages/ilmerge

:: set your target executable name here
SET APP_NAME="IBDR_BISCUITS_Processing_Tool.exe"

:: Set directory for separate files
SET ILMERGE_BUILD=DumpBinariesHere

:: the full ILMerge should be found here:
SET ILMERGE_PATH= "ILMerge.exe"
:: dir "%ILMERGE_PATH%"\ILMerge.exe

echo Removing old file
del %APP_NAME%

echo Merging %APP_NAME% ...


%ILMERGE_PATH% DumpBinariesHere\IBDR_BISCUITS_Processing_Tool.exe  ^
  /out:%APP_NAME% ^
  DumpBinariesHere\MakarovDev.ExpandCollapsePanel.dll ^
  DumpBinariesHere\Microsoft.WindowsAPICodePack.dll ^
  DumpBinariesHere\Microsoft.WindowsAPICodePack.Shell.dll ^
  DumpBinariesHere\StripConsentModel.dll ^
  "DumpBinariesHere\XML 2021K Classes.dll" ^
  DumpBinariesHere\ExcelDataReader.dll ^
  DumpBinariesHere\ICSharpCode.SharpZipLib.dll ^
  DumpBinariesHere\BCrypt.Net-Core.dll ^
  DumpBinariesHere\CsvHelper.dll ^
  DumpBinariesHere\Microsoft.Bcl.AsyncInterfaces.dll ^
  DumpBinariesHere\Microsoft.Bcl.HashCode.dll


:Done

echo Output can be found at %cd%\%APP_NAME%