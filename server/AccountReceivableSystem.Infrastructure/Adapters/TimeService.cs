using System;
using AccountReceivableSystem.Domain.Ports;

namespace AccountReceivableSystem.Infrastructure.Adapters;

public class DateTimeService : IDateTimeService
{
    public DateTime CurrentDateTime()
    {
        return DateTime.UtcNow;
    }
}