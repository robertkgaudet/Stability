<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="CajunNavyFoundation.aspx.cs" Inherits="V1_CajunNavyFoundation" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<script>
		$(document).ready(function () {

			$('.bntDonate').click(function () {
				window.location.href = 'http://cajunrelief.org/donations/donate-cajun-relief-foundation/';
				return false;
			})
		});
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-sm-12 container">
					<div class="hpanel">
						
					<div class="hpanel">
						<div class="panel-heading hbuilt text-center">
							<h1>Cajun Navy Hurricane Michael Relief Fund</h1>
						</div>
						<div class="panel-body">
							<button class="btn btn-primary btn-lg bntDonate pull-left m-r-lg m-b-md">DONATE NOW</button>
							<div class="p-l-lg">
								<p>
									<h4>Summary</h4>
									Hurricane Michael caused widespread damage and power outages in communities across Florida's Panhandle. 
									Now a Tropical Storm, Michael is forecast to bring strong winds and rains to Southern Georgia and the Carolinas. 
									This fund supports preparation, relief efforts in the form of emergency supplies like food, water, and medicine as well as 
									longer-term recovery assistance to help residents recover and rebuild.
								</p>
								<p>
									<h4>Challenge</h4>
									Hurricane Michael's powerful winds and torrential rains damaged homes, businesses, hospitals, and shut down major roadways. 
									Moving towards communities in Georgia, North and South Carolina, Tropical Storm Michael has left hundreds of thousands of residents without power. 
									We are responding and will help fund any necessary relief and recovery efforts on the ground.
								</p>
								<p>
									<h4>Solution</h4>
									All donations to this fund will support Hurricane Michael preparation, recovery, and relief efforts in the United States. 
									Initially, the fund will help first responders meet immediate needs for food, fuel, clean water, hygiene products, and shelter. 
									Once initial relief work is complete, this fund will transition to support longer-term any necessary recovery efforts run by local,
									vetted organizations responding to this disaster.
								</p>
							</div>
						</div>
									
						<div class="panel-footer">
							<div class="row">
								<div class="col-sm-6">
									<asp:HyperLink CssClass="EventLink" ID="HyperLink1" runat="server"></asp:HyperLink>
								</div>
								<div class="col-sm-6">
									<asp:Literal ID="Literal1" runat="server"></asp:Literal>
								</div>
							</div>
						</div>
					</div>
						<div class="panel-body">
							<img src="Images/LOGO.png" class="pull-left m-r-lg img-responsive img-small" />
							<div class="p-l-lg">
								<h3>About the Cajun Navy Foundation</h3>
								<div class="pull-right">
								<p>
									<p style="text-align: left;">Cajun Navy Foundation &#8211; Organic, citizen-led disaster relief.</p>
									<p>Cajun Navy Foundation believes that in today’s world, there is no reason that compassionate neighbors in affected communities can&#8217;t fill the void and bring relief as efficiently and as fast as any established entity. In fact, we think this model will be faster.</p>
									<p>During a disaster, citizens are hungry to help those in need and when they&#8217;re our neighbors, local, efficient, fast, with no red tape and very little overhead, they can&#8217;t be stopped. We believe we have created a better way called &#8220;Citizen-led Relief&#8221;.</p>
									<p>The DNA of Cajun Navy can be explained in one sentence: &#8220;When you&#8217;re hungry, there is no waste.&#8221;  We&#8217;ve spent the last year creating innovative ways to help citizens bring organic relief to those who need it. Unlike established organizations, we can scale fast, adjust on the fly and meet specific needs which eliminate waste.</p>
									<p>Anywhere there are people that are hungry or in need, good citizens are hungry to help them and Cajun Navy Foundation wants to bring them together.</p>
									<p><span style="font-size: 14px;"> </span></p>
									<p><strong>Why Cajun Navy?</strong></p>
									<p>Cajun Navy believes that today&#8217;s model for disaster recovery is not making the best use of motivated, hungry, connected citizens who use technology, social media and the internet to accomplish things. We also believe that social media and technology have allowed us to rethink aspects of how long-term recoveries after disasters are funded.</p>
									<p>The Cajun Navy team is made up of professionals with years of experience delivering technology, business software, social media, logistics and marketing solutions; every day we are working to connect local citizens who would like to work together to help deliver more efficient relief.</p>
									<p><strong>The Current Model</strong></p>
									<p>Survivors want to rebuild and rebuild quickly; however, red tape, paperwork and unorganized responses from some disaster organizations slow down the recovery process. It&#8217;s not that the stalwart disaster organizations are bad; the problem is that top-down models are inefficient and require a victim to flex to its demands instead of being flexible for the victims.</p>
									<blockquote><p><em>&#8220;When you&#8217;re hungry, there is no waste.&#8221;</em></p></blockquote>
									<p>The current top down model forces organizations to scramble and raise funds in the critical first seven days of a disaster to support overhead. Unfortunately, many of these organizations leave when the fund raising opportunity is over, putting little to no direct effort into the recovery, We think the money raised during the critical seven days of a disaster needs to be used in the rebuild phase by those most interested in the community and it&#8217;s not. This leaves survivors waiting for years for the &#8220;official&#8221; help to arrive.</p>
									<p>We envision a model where initial disaster funds raised, which amounts to millions of dollars, should be available for the difficult work of long term recovery and could be better spent in the hands of local citizens who are seeking to help their neighbors.</p>
									<p>This is why we believe that organic citizen-led engagement at the local level is key; funds raised during a disaster should be used for the entire recovery of the disaster by those within the community.<span style="font-size: 14px;"> </span></p>
									<p><strong style="font-size: 14px;">Cajun Navy Foundation origins</strong></p>
									<p><span style="font-size: 14px;">Founded in September 2016 by Rob Gaudet, who was </span><a style="font-size: 14px;" href="http://video.foxbusiness.com/v/5098396839001/?#sp=show-clips">a key organizer behind the very first Cajun Navy</a>,<span style="font-size: 14px;"> Rob&#8217;s thoughtful approach to citizen-led rescues helped save thousands of lives. His method was simple; he spearheaded a Cajun Navy group whose only requirement was that citizens agree to work alongside local authorities.</span></p>
									<p>This was what separated his efforts from other citizen boaters who were frustrated at being turned back by authorities. By working within the guidelines established by authorities, Cajun Navy boaters with his group were allowed by the authorities to put their boats into the flood waters. This collaboration between authorities and citizens saved thousands of lives in the rapidly rising waters. Rob&#8217;s actions proved that citizens acting out of concern for their neighbors could be just as effective at rescue and relief as established top down organizations.</p>
									<p>&nbsp;</p>
									<p><strong style="font-size: 14px;">Where we are today</strong></p>
									<p><span style="font-size: 14px;">Having been hard at work since we were founded in 2016, we went into overdrive when hurricane Harvey made landfall and are not slowing down. We are supporting citizen-led teams in all recent disaster areas, Louisiana, Texas, Florida and Puerto Rico. These citizens are working every day to help disaster victim’s directly recover, they&#8217;re running campaigns and they&#8217;re executing supply logistics. </span></p>
									<p><span style="font-size: 14px;">See our most recent activities on our <a href="https://www.facebook.com/GoCajunNavy">Facebook page</a>.</span></p>
									<p>The Cajun Navy management team has proven they can execute and with the right partners we believe the sky is the limit for citizen-led efforts.</p>
									<p>Cajun Navy Foundation is a 501(c)3 non-profit organization- we are now at a critical stage in our growth and are actively seeking to build relationships with partners who share our vision of hungry citizen-led relief and who might interesting in helping to fund us.</p>
									<p>If you can help learn more at <a href="http://www.cajunrelief.org/">http://www.CajunRelief.org.</a></p>
									<p>If you would like to donate to further our mission, you can do so at <a href="http://www.CajunRelief.org/Donate">http://www.CajunRelief.org/Donate</a></p>
								</p>
								</div>
							</div>
						</div>
									
						<div class="panel-footer">
							<div class="row">
								<div class="col-sm-6">
									<asp:HyperLink CssClass="EventLink" ID="hypRebuildingHomesCount" runat="server"></asp:HyperLink>
								</div>
								<div class="col-sm-6">
									<asp:Literal ID="litFollowingHomesCount" runat="server"></asp:Literal>
								</div>
							</div>
						</div>
					</div>

				</div>
			</div>
		</div>
</asp:Content>