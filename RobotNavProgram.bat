@echo off
title COS30019 Introduction to Artificial Intelligence Assignment 1
set /p path=Please Enter Filepath for .txt file of the Environment:
:loop
echo:
set /p string=Please type the type of Search you wish to perform (BFS, DFS, Greedy, A*, Bidirectional.  All these can be done with or without jump):
"%~dp0RobotNavTest\RobotNavTest\bin\Debug\net6.0\RobotNavTest.exe" "%path%" "%string%" > CON
echo:
set /p again=Would you like to run another search (Y/N)?
if "%again%"=="Y" goto loop 
pause