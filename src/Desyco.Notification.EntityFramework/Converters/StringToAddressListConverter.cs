using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Desyco.Notification.EntityFramework.Converters
{
    public class StringToAddressListConverter : ValueConverter<NotificationAddress[], string>
    {
        private static readonly Expression<Func<NotificationAddress[], string>> Target = x => Unwrapper(x);

        private static readonly Expression<Func<string, NotificationAddress[]>> Source = x => Wrapper(x);


        public StringToAddressListConverter(ConverterMappingHints mappingHints = default)
            : base(Target, Source, mappingHints)
        {
        }

        private static string Unwrapper(NotificationAddress[] value)
        {
            var addresses = value
                .Select(s => $"{s.Address};{s.UserName};{s.DisplayName}")
                .Aggregate("", (current, next) => current + next);

            return addresses;
        }

        private static NotificationAddress[] Wrapper(string value)
        {
            var result = value
                .Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split(';'))
                .Select(v => new NotificationAddress
                {
                    Address = v[0], 
                    UserName = v[1], 
                    DisplayName = v[2]

                }).ToArray();

            return result;

        }
    }
}
