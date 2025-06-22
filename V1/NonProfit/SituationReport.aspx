<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="SituationReport.aspx.cs" Inherits="V1_NonProfit_SituationReport" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>
<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
    .section {
      background: #fff;
      border: 1px solid #ccc;
      border-radius: 6px;
      padding: 25px;
      margin-bottom: 30px;
    }
    .label-badge {
      background: #d9edf7;
      color: #31708f;
      padding: 6px 12px;
      border-radius: 15px;
      display: inline-block;
      margin-right: 10px;
      margin-bottom: 5px;
    }
    .number-medium {
      font-size: 32px;
      font-weight: bold;
      color: #337ab7;
    }
    .sub-label {
      font-size: 13px;
      color: #777;
    }
    .btn-yellow {
      background-color: #f0ad4e;
      color: #fff;
      border: none;
    }
    .btn-yellow:hover {
      background-color: #ec971f;
    }
  </style>
	<style>
  .number-large {
    font-size: 48px;
    font-weight: bold;
    color: #333;
  }

  .number-medium {
    font-size: 32px;
    font-weight: bold;
    color: #555;
  }

  .number-small {
    font-size: 20px;
    font-weight: bold;
    color: #777;
  }

  .label-description {
    font-size: 12px;
    color: #888;
    margin-top: -8px;
  }
   .memberPhoto {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    object-fit: cover; /* Ensures the image covers the entire area */
    border: 2px solid black; /* Adds the white border */
    position: relative;
    background-color: white;
}
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
				<uc1:TeamHeader runat="server" ID="ucTeamHeader" />
  <!-- Header -->
  <div class="text-center">
    <div style="text-align: center;">
		<asp:Image runat="server" ID="imgTeamLogo" class="memberPhoto" style="width: 70px; height: auto;" />
	</div>
    <h1>Situation Report<br /><small><%=_teamName%></small></h1>
    <p><strong>Report Date:</strong> <span id="report-date"></span></p>
  </div>

  <!-- Daily Summary & Impact Story -->
  <div class="section">
    <h3>📅 Daily Overview & Impact Story</h3>
    <p><strong>Daily Summary:</strong> Today’s operations focused on flood relief in the Mississippi Delta region. Volunteer teams cleared debris from 18 properties, delivered over 90 essential supply kits, and provided wellness checks to 42 residents still without power. Rapid coordination between Stability.org partners enabled coverage across 4 rural parishes.</p>
    <p><strong>Impact Story:</strong> In Issaquena County, our team discovered an elderly couple stranded without transportation or power for four days. Volunteers navigated flooded backroads to bring supplies, charge their medical equipment using portable solar units, and coordinate with local responders to evacuate them to safety. Their gratitude was immense and a reminder of why this mission matters.</p>
    <a href="https://stability.org/donate" class="btn btn-yellow btn-lg">💛 Donate to This Deployment</a>
  </div>

  <!-- Current Deployments -->
  <div class="section">
    <h4>Current Deployments</h4>
    <div>
      <span class="label-badge">Hurricane Francine – Florida</span>
      <span class="label-badge">Flood Response – Mississippi Delta</span>
      <span class="label-badge">Wildfire Recovery – New Mexico</span>
    </div>
  </div>

  <!-- Funders Summary -->
  <div class="section" style="background-color: #fcf8e3; border-left: 5px solid #f0ad4e;">
    <h4>🔎 Funders Highlights</h4>
    <ul>
      <li>💡 93% of donations deployed within 48 hours</li>
      <li>📦 578 resources delivered to 3 counties</li>
      <li>👩‍⚕️ 12 medics and 4 logistics coordinators activated</li>
      <li>🤝 Partnered with 3 local churches and Rotary Club District 6840</li>
      <li>📍 Focused response in rural underserved areas</li>
      <li>🧠 Lessons learned: Need to pre-stage fuel and solar power</li>
    </ul>
  </div>

  <!-- Placeholder for Metrics Grid (convert Tailwind content next) -->
  <div class="row">
    <div class="col-sm-6">
      <div class="section">
        <h4>Volunteer Engagement</h4>
        <div class="number-large">248</div>
        <div class="sub-label">Volunteers Registered</div>
        <hr>
        <div class="row text-center">
          <div class="col-xs-4">
            <div class="number-small">6</div>
            <div class="sub-label">Cooks</div>
          </div>
          <div class="col-xs-4">
            <div class="number-small">12</div>
            <div class="sub-label">Drivers</div>
          </div>
          <div class="col-xs-4">
            <div class="number-small">4</div>
            <div class="sub-label">Medics</div>
          </div>
        </div>
        <hr>
        <div class="row text-center">
          <div class="col-xs-6">
            <div class="number-medium">137</div>
            <div class="sub-label">Assignments</div>
          </div>
          <div class="col-xs-6">
            <div class="number-medium">1,920</div>
            <div class="sub-label">Volunteer Hours</div>
          </div>
        </div>
        <hr>
        <div class="number-medium">31</div>
        <div class="sub-label">Deployment Histories</div>
      </div>
    </div>

    <div class="col-sm-6">
      <div class="section">
        <h4>Resource Coordination</h4>
        <div class="number-large">612</div>
        <div class="sub-label">Items Donated</div>
        <hr>
        <div class="row text-center">
          <div class="col-xs-6">
            <div class="number-small">52</div>
            <div class="sub-label">Requests Fulfilled</div>
          </div>
          <div class="col-xs-6">
            <div class="number-small">247</div>
            <div class="sub-label">Inventory Movements</div>
          </div>
        </div>
        <hr>
        <div class="row text-center">
          <div class="col-xs-4">
            <div class="number-small">11</div>
            <div class="sub-label">Chainsaws</div>
          </div>
          <div class="col-xs-4">
            <div class="number-small">6</div>
            <div class="sub-label">BBQ Pits</div>
          </div>
          <div class="col-xs-4">
            <div class="number-small">2</div>
            <div class="sub-label">Generators</div>
          </div>
        </div>
      </div>
    </div>
  </div>
	<div class="col-sm-6">
  <div class="section">
    <h4>Resource Coordination</h4>
    <div class="number-large">612</div>
    <div class="sub-label">Items Donated</div>
    <hr>
    <div class="row text-center">
      <div class="col-xs-6">
        <div class="number-medium">52</div>
        <div class="sub-label">Requests Fulfilled</div>
      </div>
      <div class="col-xs-6">
        <div class="number-medium">247</div>
        <div class="sub-label">Inventory Movements</div>
      </div>
    </div>
    <hr>
    <div class="row text-center">
      <div class="col-xs-4">
        <div class="number-small">11</div>
        <div class="sub-label">Chainsaws</div>
      </div>
      <div class="col-xs-4">
        <div class="number-small">6</div>
        <div class="sub-label">BBQ Pits</div>
      </div>
      <div class="col-xs-4">
        <div class="number-small">2</div>
        <div class="sub-label">Generators</div>
      </div>
    </div>
  </div>
</div>
	<div class="col-sm-6">
  <div class="section">
    <h4>Dashboard Reporting</h4>
    <div class="row text-center">
      <div class="col-xs-6">
        <div class="number-large">148</div>
        <div class="sub-label">Volunteers Deployed</div>
      </div>
      <div class="col-xs-6">
        <div class="number-medium">2,105</div>
        <div class="sub-label">Hours Served</div>
      </div>
      <div class="col-xs-6">
        <div class="number-medium">578</div>
        <div class="sub-label">Resources Delivered</div>
      </div>
      <div class="col-xs-6">
        <div class="number-medium">742</div>
        <div class="sub-label">Activity Records</div>
      </div>
      <div class="col-xs-12">
        <hr>
        <div class="number-medium" style="font-size:24px;">Export Ready</div>
        <div class="sub-label">Cumulative Reports</div>
      </div>
    </div>
  </div>
</div>

  <script>
	  document.getElementById('report-date').textContent = new Date().toLocaleDateString(undefined, {
		  weekday: 'long', year: 'numeric', month: 'long', day: 'numeric'
	  });
  </script>
				<uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>

