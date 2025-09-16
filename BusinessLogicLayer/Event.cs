using Business_Layer;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BusinessLogicLayer
{
    public class Event
    {
        public int Id { get; set; }  // Primary Key
        public string Name { get; set; }  // Event Name

    }
}
