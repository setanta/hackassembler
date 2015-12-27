#ifndef ASSEMBLER_H
#define ASSEMBLER_H

#include <QByteArray>
#include <QList>

#include "parser.h"
#include "symboltable.h"

class Assembler
{
public:
    bool open(const char* sourceFile);
    bool save(const char* binaryFile);
    void run();

    const QList<QByteArray>& hackCode() const;

    bool hasError() const;
    const QByteArray& errorMessage() const;

private:
    void processLabels();

    Parser m_parser;
    SymbolTable m_symbolTable;
    QList<QByteArray> m_hackCode;

    QByteArray m_errorMessage;
};

#endif // ASSEMBLER_H
