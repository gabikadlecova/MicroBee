using System;
using System.Collections.Generic;
using System.Text;
using MicroBee.Data.Models;

namespace MicroBee.ViewModels
{
    class AddItemViewModel
    {
	    public MicroItem Item { get; set; }
	    public byte[] ImageData { get; set; }
    }
}
