﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using _8bitVonNeiman.Common;
using _8bitVonNeiman.Compiler.Model;
using _8bitVonNeiman.Memory.View;

namespace _8bitVonNeiman.Memory {
    public class MemoryController : IMemoryControllerInput, IMemoryFormOutput {

        private List<MemoryForm> _forms = new List<MemoryForm>();
        private Dictionary<int, ExtendedBitArray> _memory;
        private MemoryFileHandler _fileHandler = new MemoryFileHandler();

        private readonly IMemoryControllerOutput _output;

        public MemoryController(IMemoryControllerOutput output) {
            _output = output;
            _memory = new Dictionary<int, ExtendedBitArray>();

            // Установка начального адреса в ячейку вектора сброса
            SetMemory(new ExtendedBitArray(Constants.StartAddress), Constants.ResetVectorAddress);
            SetMemory(new ExtendedBitArray(), Constants.ResetVectorAddress + 1);
        }

        /// Добавляет переданную память к текущей
        public void SetMemory(Dictionary<int, ExtendedBitArray> memory) {
            memory.ToList().ForEach(x => _memory[x.Key] = x.Value);
            
            ShowMemory();
        }

        public void Open() {
            if (_forms.Count == 0) {
                var form = new MemoryForm(this);
                form.Show();
                _forms.Add(form);
                ShowMemory();
            }
        }
        
        /// Открывает форму, если она закрыта или закрывает, если открыта
        public void ChangeFormState() {
            if (_forms.Count == 0) {
                var form =  new MemoryForm(this);
                form.Show();
                _forms.Add(form);
                ShowMemory();
            }
        }

        /// Устанавливает переданный байт по переданному адресу
        public void SetMemory(ExtendedBitArray memory, int address) {
            _memory[address] = memory;
            int i = address / MemoryForm.ColumnCount;
            int j = address % MemoryForm.ColumnCount;
            _forms.ForEach(x => x.SetMemory(i, j, MemoryHex(i, j)));
        }

        /// <summary>
        /// Возвращает байт, находящийся по переданному адресу.
        /// </summary>
        /// <param name="address">Адрес, память по которому запрошивается</param>
        /// <returns>Байт, находящийся по переданному адресу</returns>
        public ExtendedBitArray GetMemory(int address) {
            if (_memory.ContainsKey(address)) {
                return _memory[address];
            } else {
                return new ExtendedBitArray();
            }
        }

        /// Возвращает массив памяти переданного сегмента. Если номер сегмента неходится вне диапазона 0..3 будет возвращен пустой массив.
        public List<ExtendedBitArray> GetMemoryFromSegment(int segment) {
            if (segment < 0 || segment > 3) {
                return new List<ExtendedBitArray>();
            }
            var memory = new List<ExtendedBitArray>(256);
            var offset = segment * 256;
            for (int i = offset; i < offset + 256; i++) {
                memory.Add(_memory.ContainsKey(i) ? _memory[i] : new ExtendedBitArray());
            }

            return memory;
        }

        /// <summary>
        /// Функция, вызывающаяся при закрытии формы. Необходима для корректной работы функции ChangeFormState()
        /// </summary>
        public void FormClosed(MemoryForm form) {
            _forms.Remove(form);
        }

        /// <summary>
        /// Функция, вызываемая при изменении пользователем текста в ячейке памяти. 
        /// Помещает новое значение ячейки в память или выводит сообщение об ощибке, если введено некорректное число.
        /// </summary>
        /// <param name="row">Строка измененной ячейки.</param>
        /// <param name="collumn">Столбец измененной ячейки.</param>
        /// <param name="s">Строка, введенная в ячейку.</param>
        public void MemoryChanged(int row, int collumn, string s) {
            int num;
            try {
                num = Convert.ToInt32(s, 16);
                if (num > 255 || num < 0) {
                    _forms.First()?.ShowMessage("Число должно быть от 0 до FF");
                    _forms.ForEach(x => x.SetMemory(row, collumn, MemoryHex(row, collumn)));
                    return;
                }
            } catch {
                _forms.First()?.ShowMessage("Введено некорректное число");
                _forms.ForEach(x => x.SetMemory(row, collumn, MemoryHex(row, collumn)));
                return;
            }
            var bitArray = new ExtendedBitArray();
            CompilerSupport.FillBitArray(null, bitArray, num, 8);
            _memory[row * MemoryForm.ColumnCount + collumn] = bitArray;
            if (s.Length == 1) {
                s = "0" + s;
            }
            s = s.ToUpper();
            _forms.ForEach(x => x.SetMemory(row, collumn, s));
        }

        /// Очищает память.
        public void ClearMemoryClicked() {
            _memory = new Dictionary<int, ExtendedBitArray>();

            // Установка начального адреса по умолчанию в ячейку вектора сброса
            SetMemory(new ExtendedBitArray(Constants.StartAddress), Constants.ResetVectorAddress);
            SetMemory(new ExtendedBitArray(0), Constants.ResetVectorAddress + 1);

            ShowMemory();
        }

        public void LoadMemoryClicked() {
            var memory = _fileHandler.LoadMemory();
            if (memory == null) {
                return;
            }
            _memory = memory;
            SetMemory(memory);
        }

        public void SaveMemoryClicked() {
            _fileHandler.Save(_memory);
        }

        public void SaveAsMemoryClicked() {
            _fileHandler.SaveAs(_memory);
        }

        public void CheckMemoryClicked() {
            var memory = _fileHandler.LoadMemory();
            var missplaced = new List<int>();
            foreach (var key in memory.Keys) {
                if (!_memory.ContainsKey(key) || memory[key].NumValue() != _memory[key].NumValue()) {
                    missplaced.Add(key);
                }
            }
            if (missplaced.Count == 0) {
                MessageBox.Show("Память соотвутствует эталонной");
            } else {
                var text = string.Join(", ", missplaced.Select( x => x.ToString("X2")).ToArray());
                MessageBox.Show("Память не соответствует в следующих позициях: " + text);
            }
        }

        public void FormButtonClicked() {
            var form = new MemoryForm(this);
            _forms.Add(form);

            int formsCount = _forms.Count;
            switch (formsCount) {
                case 2:
                    int ds = _output.DS;
                    form.ScrollToSegment(ds);
                    form.Text = "Память - Сегмент данных";
                    break;
                case 3:
                    int ss = _output.SS;
                    form.ScrollToEndOfSegment(ss);
                    form.Text = "Память - Сегмент стека";
                    break;
            }
            form.Show();

            ShowMemory();
        }

        /// Обновляет состояние формы в соответствии с текущим состоянием памяти
        private void ShowMemory() {
            for (int i = 0; i < MemoryForm.RowCount; i++)
            for (int j = 0; j < MemoryForm.ColumnCount; j++) {
                _forms.ForEach(x => x.SetMemory(i, j, MemoryHex(i, j)));
            }
        }

        /// Возвращает шестнадцатиричное представление значение ячейки памяти, находящийся по переданным индексам
        private string MemoryHex(int row, int collumn) {
            int memoryIndex = row * MemoryForm.ColumnCount + collumn;
            if (_memory.ContainsKey(memoryIndex)) {
                return _memory[memoryIndex].ToHexString();
            } else {
                return "00";
            }
        }
    }
}
