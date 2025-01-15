using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DonationModel
/// </summary>
public class DonationModel
{
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public Decimal Amount { get; set; }
    public string Address { get; set; }

}