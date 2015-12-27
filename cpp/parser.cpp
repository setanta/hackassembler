#include <iostream>
#include "parser.h"

const QList<QByteArray> Parser::JUMP_COMMANDS = {
    "JGT",
    "JEQ",
    "JGE",
    "JLT",
    "JNE",
    "JLE",
    "JMP"
};

const QList<QByteArray> Parser::DEST_VALUES = {
    "M",
    "D",
    "MD",
    "A",
    "AM",
    "AD",
    "AMD"
};

bool Parser::openSourceFile(const char* inputFile)
{
    m_sourceFile.setFileName(inputFile);
    if (!m_sourceFile.open(QIODevice::ReadOnly | QIODevice::Text))
        return false;
    reset();
    return true;
}

void Parser::reset()
{
    m_sourceFile.seek(0);
    m_currentLine = 0;
    m_errorMessages.clear();
    clearParseData();
}

void Parser::clearParseData()
{
    m_currentCommandType = Parser::UNKNOWN_COMMAND;
    m_symbol.clear();
    m_dest.clear();
    m_comp.clear();
    m_jump.clear();
}

bool Parser::hasMoreCommands() const
{
    return !m_sourceFile.atEnd();
}

void Parser::advance()
{
    QByteArray sourceLine;
    while (sourceLine.isEmpty() && !m_sourceFile.atEnd()) {
        m_currentLine++;
        sourceLine = removeComment(m_sourceFile.readLine());
    }

    clearParseData();

    if (sourceLine.startsWith('@'))
        processACommand(sourceLine);
    else if (sourceLine.startsWith(('(') && sourceLine.endsWith(')')))
        processLCommand(sourceLine);
    else
        processCCommand(sourceLine);
}

QByteArray Parser::removeComment(const QByteArray& sourceLine) const
{
    return sourceLine.mid(0, sourceLine.indexOf("//")).trimmed();
}

void Parser::processACommand(QByteArray& sourceLine)
{
    m_currentCommandType = Parser::A_COMMAND;
    QByteArray address = sourceLine.mid(1);
    if (isValidSymbol(address) || isValidConstant(address))
        m_symbol = address;
    else
        addErrorMessage(QString("Invalid symbol: ") + address);
    m_symbol = address;
}

void Parser::processLCommand(QByteArray& sourceLine)
{
    m_currentCommandType = Parser::L_COMMAND;
    QByteArray label = sourceLine.mid(1, sourceLine.length() - 2);
    if (isValidSymbol(label))
        m_symbol = label;
    else
        addErrorMessage(QString("Invalid symbol: ") + label);
}

void Parser::processCCommand(QByteArray sourceLine)
{
    // DEST=COMP;JUMP
    m_currentCommandType = Parser::C_COMMAND;

    // DEST=comp;jump
    QList<QByteArray> cmdParts = sourceLine.split('=');

    if (cmdParts.length() == 2) {
        if (!isValidDestination(cmdParts.first())) {
            addErrorMessage(QString("Invalid command: ") + sourceLine);
            return;
        }
        m_dest = cmdParts.first();
        sourceLine = cmdParts.last();
    }

    // dest=comp;JUMP
    cmdParts = sourceLine.split(';');
    if (cmdParts.length() == 2) {
        if (!isValidJump(cmdParts.last())) {
            m_dest.clear();
            addErrorMessage(QString("Invalid command: ") + sourceLine);
            return;
        }
        m_jump = cmdParts.last();
        sourceLine = cmdParts.first();
    }

    // dest=COMP;jump
    if (!isValidCommand(sourceLine)) {
        m_dest.clear();
        m_jump.clear();
        addErrorMessage(QString("Invalid command: ") + sourceLine);
        return;
    }
    m_comp = sourceLine;
}

// A user-defined symbol can be any sequence of letters, digits, underscore (_),
// dot (.), dollar sign ($), and colon (:) that does not begin with a digit.
bool Parser::isValidSymbol(const QByteArray& symbol) const
{
    static const QRegExp rx("^[a-zA-Z_.$:][\\w.$:]*$");
    return rx.exactMatch(symbol);
}

// Constants must be non-negative and are written in decimal notation.
bool Parser::isValidConstant(const QByteArray& constant) const
{
    static const QRegExp rx("^\\d+$");
    return rx.exactMatch(constant);
}

// Valid Commands:
// 0, 1, -1, D, A, M, !D, !A, !M, -D, -A, -M
// D+1, A+1, M+1, D-1, A-1, M-1, D+A, D+M, D-A, D-M
// A-D, M-D, D&A, D&M, D|A, D|M
bool Parser::isValidCommand(const QByteArray& command) const
{
    static const QRegExp rx("^(0|[-]?1|[-!]?[DAM]|[DAM][+-]1|D[+\\-\\&\\|][AM]|[AM]-D)$");
    return rx.exactMatch(command);
}

bool Parser::isValidDestination(const QByteArray& dest) const
{
    return Parser::DEST_VALUES.contains(dest);
}

bool Parser::isValidJump(const QByteArray& jump) const
{
    return Parser::JUMP_COMMANDS.contains(jump);
}

void Parser::addErrorMessage(QString errorMessage)
{
    m_errorMessages.append(QString("[%1:%2] %3").arg(m_sourceFile.fileName(), QString(m_currentLine), errorMessage));
}
