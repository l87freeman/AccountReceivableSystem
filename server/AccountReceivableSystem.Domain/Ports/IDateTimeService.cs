using System;

namespace AccountReceivableSystem.Domain.Ports;

public interface IDateTimeService
{
    DateTime CurrentDateTime();
}