#ifndef CODE_H
#define CODE_H

#include <QByteArray>

namespace Code
{
    QByteArray dest(const QByteArray& mnemonic);
    QByteArray jump(const QByteArray& mnemonic);
    QByteArray comp(const QByteArray& mnemonic);
}

#endif // CODE_H
