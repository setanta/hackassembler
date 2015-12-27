#include "symboltable.h"

SymbolTable::SymbolTable()
{
    clear();
}

void SymbolTable::clear()
{
    m_symbolTable.clear();
    m_symbolTable["SP"]     = 0x0000;
    m_symbolTable["LCL"]    = 0x0001;
    m_symbolTable["ARG"]    = 0x0002;
    m_symbolTable["THIS"]   = 0x0003;
    m_symbolTable["THAT"]   = 0x0004;
    m_symbolTable["SCREEN"] = 0x4000;
    m_symbolTable["KBD"]    = 0x6000;

    for (uint i = 0; i < 16; i++)
        m_symbolTable["R" + QByteArray::number(i)] = i;

    m_nextMemoryPos = 16;
}

uint SymbolTable::addEntry(const QByteArray& symbol)
{
    uint address = m_nextMemoryPos;
    addEntry(symbol, address);
    m_nextMemoryPos++;
    return address;
}

void SymbolTable::addEntry(const QByteArray& symbol, uint address)
{
    m_symbolTable[symbol] = address;
}

bool SymbolTable::contains(const QByteArray& symbol) const
{
    return m_symbolTable.contains(symbol);
}

uint SymbolTable::getAddress(const QByteArray& symbol, bool& found) const
{
    found = contains(symbol);
    if (found)
        return m_symbolTable[symbol];
    return 0;
}

uint SymbolTable::getAddressWithAddEntry(const QByteArray& symbol)
{
    bool found;
    uint address = getAddress(symbol, found);
    if (found)
        return address;
    return addEntry(symbol);
}
