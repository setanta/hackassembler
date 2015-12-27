#include <QTextStream>
#include "assembler.h"
#include "code.h"

bool Assembler::open(const char* sourceFile)
{
    if (!m_parser.openSourceFile(sourceFile))
        return false;

    m_hackCode.clear();
    m_symbolTable.clear();
    m_errorMessage.clear();

    return true;
}

bool Assembler::save(const char* binaryFile)
{
    QFile outputFile(binaryFile);
    if (!outputFile.open(QIODevice::WriteOnly | QIODevice::Text))
        return false;

    QTextStream out(&outputFile);
    for (const QByteArray& line : m_hackCode)
        out << line << "\n";

    return true;
}

void Assembler::run()
{
    processLabels();

    while (m_parser.hasMoreCommands()) {
        m_parser.advance();
        uint address;

        switch (m_parser.commandType()) {
        case Parser::C_COMMAND:
            m_hackCode.append("111" +
                              Code::comp(m_parser.comp()) +
                              Code::dest(m_parser.dest()) +
                              Code::jump(m_parser.jump()));
            break;

        case Parser::A_COMMAND:
            bool isNumeric;
            address = m_parser.symbol().toUInt(&isNumeric);
            if (!isNumeric)
                address = m_symbolTable.getAddressWithAddEntry(m_parser.symbol());
            m_hackCode.append(QByteArray::number(address, 2).rightJustified(16, '0'));

        default:
            break;
        }
    }
}

const QList<QByteArray>& Assembler::hackCode() const
{
    return m_hackCode;
}

bool Assembler::hasError() const
{
    return !m_errorMessage.isEmpty();
}

const QByteArray& Assembler::errorMessage() const
{
    return m_errorMessage;
}

void Assembler::processLabels()
{
    uint memoryAddress = 0;

    while (m_parser.hasMoreCommands()) {
        m_parser.advance();

        switch (m_parser.commandType()) {
        case Parser::A_COMMAND:
        case Parser::C_COMMAND:
            memoryAddress++;
            break;

        case Parser::L_COMMAND:
            m_symbolTable.addEntry(m_parser.symbol(), memoryAddress);
            break;

        default:
            break;
        }
    }
    m_parser.reset();
}
