#ifndef SYMBOLTABLE_H
#define SYMBOLTABLE_H

#include <QByteArray>
#include <QHash>

class SymbolTable
{
public:
    SymbolTable();

    void clear();

    uint addEntry(const QByteArray& symbol);
    void addEntry(const QByteArray& symbol, uint address);

    bool contains(const QByteArray& symbol) const;

    uint getAddress(const QByteArray& symbol, bool& found) const;
    uint getAddressWithAddEntry(const QByteArray& symbol);

private:
    // Symbol -> RAM Address
    QHash<QByteArray, uint> m_symbolTable;
    uint m_nextMemoryPos;
};

#endif // SYMBOLTABLE_H
