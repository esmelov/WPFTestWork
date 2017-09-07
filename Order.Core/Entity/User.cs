using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Core.Entity
{
    [Table("User")]
    public class User : IDataErrorInfo
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Adress { get; set; }
        public Boolean VIP { get; set; }        
        public IEnumerable<Order> Orders { get; set; }
        public String Error => throw new NotImplementedException();
        
        public String this[string columnName]
        {
            get
            {
                String error = String.Empty;
                switch (columnName)
                {
                    case "Name":
                        if (String.IsNullOrWhiteSpace(Name))
                        {
                            error = "Поле Имя не может быть пустым!";
                        }
                        break;

                    case "Adress":
                        if (String.IsNullOrWhiteSpace(Adress))
                        {
                            error = "Поле Адрес не может быть пустым!";
                        }
                        break;
                }
                return error;
            }
        }
    }
}
