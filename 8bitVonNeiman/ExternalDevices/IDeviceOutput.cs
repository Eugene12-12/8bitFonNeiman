﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8bitVonNeiman.ExternalDevices {
    public interface IDeviceOutput {
        void DeviceFormClosed(IDeviceInput device);
        void MakeInterruption(byte irq);
        ISet<IDeviceInput> Devices { get; }
    }
}
