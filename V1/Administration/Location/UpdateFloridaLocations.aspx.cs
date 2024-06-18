using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Administration_Location_UpdateFloridaLocations : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var floridaLocations = from l in dc.Addresses
                               join lp in dc.LocationProfiles on l.AddressId equals lp.AddressId
                               where l.State == "Florida"
                               select new { l.AddressId, lp.LocationProfileId };

        foreach(var state in floridaLocations)
        {
            var doesRecordExist = from lpe in dc.LocationProfileEvents
                                  where 
                                  lpe.AddressId == state.AddressId
                                  && lpe.LocationProfileId == state.LocationProfileId
                                  && lpe.EventId == new Guid("B8E11510-082D-4B57-B393-A990BEEB896D")
                                  select lpe;

            if(doesRecordExist.Count() == 0)
            {
                //Insert the record.
                LocationProfileEvent locationProfileEvent = new LocationProfileEvent();
                locationProfileEvent.AddressId = state.AddressId;
                locationProfileEvent.LocationProfileId = state.LocationProfileId;
                locationProfileEvent.LocationProfileEventId = Guid.NewGuid();
                locationProfileEvent.EventId = new Guid("B8E11510-082D-4B57-B393-A990BEEB896D");
                dc.LocationProfileEvents.InsertOnSubmit(locationProfileEvent);
                dc.SubmitChanges();
            }
        }
    }
}