﻿using System;
using _8bitVonNeiman.Common;

namespace _8bitVonNeiman.Cpu {
    ///Класс: имплементирующий работу с флагами процессора
    public class FlagsController {

        public ExtendedBitArray Flags { get; private set; } = new ExtendedBitArray();
        private ExtendedBitArray _state;
        private ExtendedBitArray _arg;

        /// Аллиас регистра нуля
        public bool Z {
            get => Flags[0]; set => Flags[0] = value;
        }

        /// Аллиас регистра отрицательного числа
        public bool N {
            get => Flags[1];  set => Flags[1] = value;
        }

        /// Аллиас регистра переполнения со знаком
        public bool O {
            get => Flags[2]; set => Flags[2] = value;
        }

        /// Аллиас регистра переноса из третьего в четвертый
        public bool A {
            get => Flags[3]; set => Flags[3] = value;
        }

        /// Аллиас регистра переноса
        public bool C {
            get => Flags[4]; set => Flags[4] = value;
        }

        /// Аллиас регистра разрешения прерывания
        public bool I {
            get => Flags[5]; set => Flags[5] = value;
        }

        public void SetPreviousState(ExtendedBitArray array) {
            _state = new ExtendedBitArray(array);
        }

        public void SetArgument(ExtendedBitArray array) {
            _arg = new ExtendedBitArray(array);
        }

        public void Reset() {
            _state = null;
            _arg = null;
            Flags = new ExtendedBitArray();
        }

        public void UpdateFlags(ExtendedBitArray newState, string command, bool? overflow = null, ExtendedBitArray arg = null) {
            var mask = 0;
            switch (command.ToLower()) {
            case "add":
            case "sub":
            case "mul":
            case "div":
            case "cmp":
            case "adc":
            case "subb":
                mask = 1 + 2 + 4 + 8 + 16;
                break;
            case "and":
            case "or":
            case "xor":
            case "not":
                A = false; //сброс всех флагов
                C = false;
                Z = false;
                N = false;
                O = false;
                mask = 0 + 1;
                break;
            case "inc":
            case "dec":
            case "daa":
            case "dsa":
                    mask = 1 + 2 + 8 + 16;
                break;
            }
            FormFlags(newState, mask, overflow, command, arg);
        }

        public void FormFlags(ExtendedBitArray newState, int mask, bool? overflow, string command, ExtendedBitArray arg) {
            if ((mask & 1) != 0) {
                Z = newState.NumValue() == 0;
            }
            if ((mask & 2) != 0) {
                N = newState[Constants.WordSize - 1];
            }
            if ((mask & 4) != 0) {
                if (_state == null|| _arg == null) {
                    throw new Exception("Формирование флага без задания предыдущего состояния");
                }
                
                O = _state[Constants.WordSize - 1] == _arg[Constants.WordSize - 1] && _arg[Constants.WordSize - 1] != newState[Constants.WordSize - 1];
            }
            if ((mask & 8) != 0) {
                int oldLow = _state.NumValue() & 15;
                int argLow = (arg?.NumValue() ?? 0) & 15;
                int newLow = newState.NumValue() & 15;
                if (command == "add") {
                    A = oldLow + argLow > 15;
                } else if (command == "adc") {
                    A = oldLow + argLow + (C ? 1 : 0) > 15;
                } else if (command == "inc") {
                    A = oldLow == 15;
                } else if (command == "daa" || command == "dsa") {
                    A = oldLow != newLow;
                } else if (command == "dec") {
                    A = oldLow == 0;
                } else if (command == "cmp" || command == "sub") {
                    A = oldLow - argLow < 0;
                } else if (command == "subb") {
                    A = oldLow - argLow - (C ? 1 : 0) < 0;
                }
            }
            if ((mask & 16) != 0) {
                C = overflow.HasValue && overflow.Value;
            }
            _state = null;
            _arg = null;
        }
    }
}
