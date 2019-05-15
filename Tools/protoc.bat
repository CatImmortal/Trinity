@echo off
protoc.exe --csharp_out="../Assets/GameMain/Libraries/ET/Module/Message/" --proto_path="../Proto/" OuterMessage.proto
protoc.exe --csharp_out="../Assets/GameMain/Scripts/Hotfix/ETNetwork/Module/Message/" --proto_path="../Proto/" HotfixMessage.proto
protoc.exe --csharp_out="../Assets/GameMain/Libraries/ET/Module/FrameSync/" --proto_path="../Proto/" FrameMessage.proto
echo finish... 
pause