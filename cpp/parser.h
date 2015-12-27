#ifndef PARSER_H
#define PARSER_H

#include <QByteArray>
#include <QFile>
#include <QStringList>

class Parser
{
public:
    enum CommandType
    {
        UNKNOWN_COMMAND,
        A_COMMAND,       // Addressing instruction for "@Xxx" (A-instruction)
        C_COMMAND,       // Compute instruction for "dest=comp;jump" (C-instruction)
        L_COMMAND        // Pseucocommand for "(Xxx)" (L-instruction)
    };

    bool openSourceFile(const char* sourceFile);
    void reset();

    bool hasMoreCommands() const;
    void advance();

    CommandType commandType() const { return m_currentCommandType; }
    const QByteArray& symbol() const { return m_symbol; }
    const QByteArray& dest() const { return m_dest; }
    const QByteArray& comp() const { return m_comp; }
    const QByteArray& jump() const { return m_jump; }

    bool hasErrors() const { return m_errorMessages.isEmpty(); }

private:
    void clearParseData();
    QByteArray removeComment(const QByteArray& sourceLine) const;

    void processACommand(QByteArray& sourceLine);
    void processLCommand(QByteArray& sourceLine);
    void processCCommand(QByteArray sourceLine);

    bool isValidSymbol(const QByteArray& symbol) const;
    bool isValidConstant(const QByteArray& constant) const;
    bool isValidCommand(const QByteArray& command) const;
    bool isValidDestination(const QByteArray& dest) const;
    bool isValidJump(const QByteArray& jump) const;

    void addErrorMessage(QString errorMessage);

    QFile m_sourceFile;

    int m_currentLine;
    CommandType m_currentCommandType;

    QStringList m_errorMessages;

    QByteArray m_symbol;
    QByteArray m_dest;
    QByteArray m_comp;
    QByteArray m_jump;

    static const QList<QByteArray> JUMP_COMMANDS;
    static const QList<QByteArray> DEST_VALUES;
};

#endif // PARSER_H
