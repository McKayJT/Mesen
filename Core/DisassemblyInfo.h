#pragma once
#include "stdafx.h"
#include <unordered_map>
#include "CPU.h"

class LabelManager;

class DisassemblyInfo
{
public:
	static string OPName[256];
	static AddrMode OPMode[256];
	static uint8_t OPSize[256];
	static bool IsUnofficialCode[256];

private:
	uint8_t _byteCode[3];
	bool _isSubEntryPoint = false;
	bool _isSubExitPoint = false;
	uint32_t _opSize = 0;
	AddrMode _opMode;
	
public:
	DisassemblyInfo();
	DisassemblyInfo(uint8_t* opPointer, bool isSubEntryPoint);

	void SetSubEntryPoint();

	int32_t GetEffectiveAddress(State& cpuState, MemoryManager* memoryManager);
	
	void GetEffectiveAddressString(string &out, State& cpuState, MemoryManager* memoryManager, LabelManager* labelManager);
	void ToString(string &out, uint32_t memoryAddr, MemoryManager* memoryManager, LabelManager* labelManager);
	void GetByteCode(string &out);
	uint32_t GetSize();
	uint16_t GetOpAddr(uint16_t memoryAddr);

	bool IsSubEntryPoint();
	bool IsSubExitPoint();
};

