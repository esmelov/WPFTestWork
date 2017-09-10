using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Core.Entity
{
    [Table("Order")]
    public class Order : IDataErrorInfo
    {
        public Int32 Id { get; set; }        
        public Int32 UserId { get; set; }
        public String Description { get; set; }
        public String Error
        {
            get
            {
                return null;
            }
        }

        public String this[String columnName]
        {
            get
            {
                String error = null;
                switch (columnName)
                {
                    case "Description":
                        if (String.IsNullOrWhiteSpace(Description) || Description == "<Введите описание товара>")
                        {
                            error = "Поле Описание не может быть пустым!";
                        }
                        break;
                }
                return error;
            }
        }
    }
}
