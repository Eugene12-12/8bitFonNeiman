﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _8bitVonNeiman.Common;
using _8bitVonNeiman.Common.MicroLibrary;
using _8bitVonNeiman.ExternalDevices.Timer2.View;

namespace _8bitVonNeiman.ExternalDevices.Timer2 {
    public class Timer2Controller : IDeviceInput, ITimer2FormOutput {
        private static readonly double UPDATE_PERIOD_MILLIS = 100.0;

        private Timer2Form _form;
        private readonly IDeviceOutput _output;

        private int _baseAddress = 20;
        private byte _irq = 2;

        private ExtendedBitArray _tcntH = new ExtendedBitArray();
        private ExtendedBitArray _tcntL = new ExtendedBitArray();

        private ExtendedBitArray _tiorH = new ExtendedBitArray();
        private ExtendedBitArray _tiorL = new ExtendedBitArray();

        private ExtendedBitArray _tscrH = new ExtendedBitArray(0x10);
        private ExtendedBitArray _tscrL = new ExtendedBitArray();

        private ExtendedBitArray _hBuffer = new ExtendedBitArray();

        private delegate void UpdateFormDelegate();

        private UpdateFormDelegate _updateFormDelegate;

        private readonly MicroTimer _timer;
        private byte _internalCounter;

        private double _lastUpdateMillis;

        public Timer2Controller(IDeviceOutput output, int baseAddress, int irq) {
            _output = output;
            _updateFormDelegate = new UpdateFormDelegate(UpdateForm);
            _timer = new MicroTimer(1000);
            _timer.MicroTimerElapsed += new MicroTimer.MicroTimerElapsedEventHandler(OnTimerEvent);
            _baseAddress = baseAddress * 0x10;
            _irq = (byte)irq;
        }

        public override void OpenForm() {
            if (_form == null) {
                _form = new Timer2Form(this);
            }
            UpdateForm();
            _form.ShowDeviceParameters(_baseAddress, _irq);
            _form.Show();
        }

        public override void ExitThread() {
            _timer.Abort();
            _timer.Enabled = false;
        }

        /// Открывает форму, если она закрыта или закрывает, если открыта
        public void ChangeFormState() {
            if (_form == null) {
                _form = new Timer2Form(this);
                _form.Show();
                UpdateForm();
            } else {
                _form.Close();
            }
        }

        private void UpdateForm() {
            double nowMillis = DateTime.Now.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds;

            _form.ShowRegisters(_tcntH, _tcntL, _tiorH, _tiorL, _tscrH, _tscrL);
            _lastUpdateMillis = nowMillis;
        }

        public override bool HasMemory(int address) {
            return _baseAddress <= address && address <= _baseAddress + 5;
        }

        public override void SetMemory(ExtendedBitArray memory, int address) {
            int localAddress = address - _baseAddress;
            if (4 <= localAddress && localAddress <= 5 && memory[7]) {
                WriteBitCmd(memory, address);
                return;
            }

            switch (localAddress) {
                case 0:
                    _tcntL = memory;
                    _tcntH = new ExtendedBitArray(_hBuffer);
                    break;
                case 2:
                    _tiorL = memory;
                    _tiorH = new ExtendedBitArray(_hBuffer);
                    break;
                case 4:
                    _tscrL = memory;
                    var value = _hBuffer.NumValue() & 0xF1 | _tscrH.NumValue() & 0x0E;
                    _tscrH = new ExtendedBitArray(value);
                    break;
                case 1:
                case 3:
                case 5:
                    _hBuffer = new ExtendedBitArray(memory);
                    break;
            }
            ApplySettings();
        }

        public override ExtendedBitArray GetMemory(int address) {
            switch (address - _baseAddress) {
                case 0:
                    _hBuffer = new ExtendedBitArray(_tcntH);
                    return _tcntL;
                case 2:
                    _hBuffer = new ExtendedBitArray(_tiorH);
                    return _tiorL;
                case 4:
                    _hBuffer = new ExtendedBitArray(_tscrH);
                    return _tscrL;
                case 1:
                case 3:
                case 5:
                    return _hBuffer;
            }
            return new ExtendedBitArray();
        }

        public override void SetMemoryBit(bool value, int address, int bitIndex) {
            switch (address - _baseAddress) {
                case 0:
                    _tcntL[bitIndex] = value;
                    break;
                case 1:
                    _tcntH[bitIndex] = value;
                    break;
                case 2:
                    _tiorL[bitIndex] = value;
                    break;
                case 3:
                    _tiorH[bitIndex] = value;
                    break;
                case 4:
                    _tscrL[bitIndex] = value;
                    break;
                case 5:
                    _tscrH[bitIndex] = value;
                    break;
            }
            ApplySettings();
        }

        public override bool GetMemoryBit(int address, int bitIndex) {
            switch (address - _baseAddress) {
                case 0:
                    return _tcntL[bitIndex];
                case 1:
                    return _tcntH[bitIndex];
                case 2:
                    return _tiorL[bitIndex];
                case 3:
                    return _tiorH[bitIndex];
                case 4:
                    return _tscrL[bitIndex];
                case 5:
                    return _tscrH[bitIndex];
            }
            return false;
        }

        public override void UpdateUI() {
            UpdateForm();
        }

        public void FormClosed() {
            _form = null;
            _timer.Abort();
            _timer.Enabled = false;

            _output.DeviceFormClosed(this);
        }

        public void ResetButtonClicked() {
            _tcntH = new ExtendedBitArray();
            _tcntL = new ExtendedBitArray();

            _tiorH = new ExtendedBitArray();
            _tiorL = new ExtendedBitArray();

            _tscrH = new ExtendedBitArray(0x10);
            _tscrL = new ExtendedBitArray();

            _hBuffer = new ExtendedBitArray();

            _internalCounter = 0;

            ApplySettings();

            _form.Invoke(_updateFormDelegate);
        }

        public void CaptureButtonClicked() {
            SetCapture(true);

            _form.Invoke(_updateFormDelegate);
        }

        private void WriteBitCmd(ExtendedBitArray cmd, int address) {
            bool value = cmd[0];
            int bitIndex = (cmd.NumValue() & 0xF) >> 1;
            SetMemoryBit(value, address, bitIndex);
        }

        private void MakeInterruption() {
            _output.MakeInterruption(_irq);
        }

        private bool IsEnabled() {
            return _tscrL[0];
        }

        private byte GetMode() {
            bool hMode = _tscrL[2];
            bool lMode = _tscrL[1];
            return (byte)((hMode ? 2 : 0) + (lMode ? 1 : 0));
        }

        private bool IsResetOnCmp() {
            return _tscrL[3];
        }

        private bool IsOverflowIntEnabled() {
            return _tscrL[4];
        }

        private bool IsComparisonIntEnabled() {
            return _tscrL[5];
        }

        private bool IsCaptureIntEnabled() {
            return _tscrL[6];
        }

        private bool IsTSCRLLoadAllowed() {
            return _tscrL[7];
        }

        private bool Capture() {
            return _tscrH[0];
        }

        private void SetCapture(bool capture) {
            _tscrH[0] = capture;

            byte mode = GetMode();
            if (mode == 1 && capture) {
                SetCaptureFlag(true);
            }
        }

        private bool IsOverflowFlag() {
            return _tscrH[1];
        }

        private void SetOverflowFlag(bool overflow) {
            _tscrH[1] = overflow;

            if (overflow) {
                byte mode = GetMode();
                if (mode == 3) { // автозагрузка
                    _tcntH = new ExtendedBitArray(_tiorH);
                    _tcntL = new ExtendedBitArray(_tiorL);
                }
            }

            if (overflow && IsOverflowIntEnabled()) {
                MakeInterruption();
            }
        }

        private bool IsComparisonFlag() {
            return _tscrH[2];
        }

        private void SetComparisonFlag(bool comparison) {
            _tscrH[2] = comparison;

            if (comparison && IsComparisonIntEnabled()) {
                MakeInterruption();
            }
        }

        private bool IsCaptureFlag() {
            return _tscrH[3];
        }

        private void SetCaptureFlag(bool capture) {
            _tscrH[3] = capture;

            SetCapture(false);

            if (capture) {
                byte mode = GetMode();
                if (mode == 1) { // режим захвата
                    _tiorH = new ExtendedBitArray(_tcntH);
                    _tiorL = new ExtendedBitArray(_tcntL);
                }
            }

            if (capture && IsCaptureIntEnabled()) {
                MakeInterruption();
            }
        }

        private byte GetDividerMode() {
            bool div2 = _tscrH[6];
            bool div1 = _tscrH[5];
            bool div0 = _tscrH[4];
            return (byte)((div2 ? 4 : 0) + (div1 ? 2 : 0) + (div0 ? 1 : 0));
        }

        private bool IsTSCRHLoadAllowed() {
            return _tscrH[7];
        }

        private void ApplySettings() {
            byte divider = GetDividerMode();
            if (divider == 0) {
                _tscrH[4] = true;
                divider = 1;
            }
            _timer.Enabled = IsEnabled();

            byte mode = GetMode();
            if (mode == 1 && Capture()) { // режим захвата
                SetCaptureFlag(true);
            }
        }

        private void OnTimerEvent(object sender, MicroTimerEventArgs e) {
            if (++_internalCounter < GetDividerMode()) return;

            _internalCounter = 0;

            if (_tcntL.Inc()) {
                if (_tcntH.Inc()) {
                    SetOverflowFlag(true);
                }
            }
            byte mode = GetMode();
            if (mode == 2) { // сравнение
                if (_tcntL.NumValue() == _tiorL.NumValue() && _tcntH.NumValue() == _tiorH.NumValue()) {
                    SetComparisonFlag(true);

                    if (IsResetOnCmp()) {
                        _tcntH = new ExtendedBitArray();
                        _tcntL = new ExtendedBitArray();
                    }
                }
            }

            double nowMillis = DateTime.Now.ToUniversalTime().Subtract(
                    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    ).TotalMilliseconds;
            if (nowMillis - _lastUpdateMillis > UPDATE_PERIOD_MILLIS) {
                _form.Invoke(_updateFormDelegate);
                _lastUpdateMillis = nowMillis;
            }
        }
    }
}
