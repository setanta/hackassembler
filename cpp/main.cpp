#include <iostream>
#include "assembler.h"

int main(int argc, char* argv[])
{
    if (argc == 1) {
        std::cout << "Usage: " << argv[0] << " SOURCE.asm [BINARY.hack]" << std::endl << std::endl;
        return 1;
    }

    Assembler assembler;

    if (!assembler.open(argv[1])) {
        std::cerr << "Source file " << argv[1] << " does not exist." << std::endl;
        return 1;
    }

    assembler.run();

    if (assembler.hasError()) {
        std::cerr << "Problems parsing assembly file '" << argv[2] << "'." << std::endl << std::endl;
        std::cerr << assembler.errorMessage().data() << std::endl;
        return 1;
    }

    if (argc >= 3) {
        if (assembler.save(argv[2])) {
            std::cout << "Built '" << argv[1] << "' into '" << argv[2] << "'." << std::endl;
            return 0;
        } else {
            std::cerr << "Could not generate binary file '" << argv[2] << "' from '" << argv[1] << "'." << std::endl;
            return 1;
        }
    }

    for (const QByteArray& line : assembler.hackCode())
        std::cout << line.data() << std::endl;

    return 0;
}

