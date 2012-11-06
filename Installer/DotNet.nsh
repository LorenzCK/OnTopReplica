# .NET Installation checker

!macro HasDotNet4 OutVar
	Push "Install" ;reg key
	Push "Software\Microsoft\NET Framework Setup\NDP\v4\Client" ;v4.0 client reg node
	Call HasDotNet4Core
	Pop ${OutVar}
!macroend

!define HasDotNet4 "!insertmacro HasDotNet4"

Function HasDotNet4Core
	Pop $R0 ;reg node to check
	Pop $R1 ;reg key
	
	ReadRegDWORD $R3 HKLM $R0 $R1
	
	;MessageBox MB_OK "$R0 \ $R1 value is $R3"
	;IntOp $R8 $R3 % 1 ;logical AND with 1 (should evaluate to 1 in $R8)
	
	IntCmp $R3 1 has hasNot has ;jump if >= 1
	
	has:
	Push 1
	Goto exit
	
	hasNot:
	Push 0
	Goto exit
	
	exit:
FunctionEnd
