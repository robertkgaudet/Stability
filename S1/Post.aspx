<%@ Page Title="" Language="C#" MasterPageFile="~/S1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Post.aspx.cs" Inherits="S1_Post" %>
<%@ MasterType VirtualPath="~/S1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
		.blog-article-box
		{
			margin:0px;
			padding:0px;
		}
		.panel-heading
		{
			margin:0px;	
			padding:0px;
		}
		.panel-image
		{
			margin:0px;	
			padding:0px;
		}
		.panel-body, .blog-article-box, .hpanel {
			padding:0px;
			margin:0px;
		}

		.input-group-addon{
  border:1px solid !important;
  border-color: darkgray !important;
}
		input[type="number"] {

  border:1px solid !important;
  border-color: darkgray !important;
		}
	</style>

	<script src="https://js.braintreegateway.com/web/dropin/1.25.0/js/dropin.min.js"></script>

	<script type="text/javascript">

		var purchaseEvent;

		var setPrice = async (newPrice) => {
			document.getElementById('#price-input').value = newPrice
		}

		var changeMethod = async (methodChecked) => {

			if (methodChecked == 'cc') {

				document.getElementById('#ccpaymentview').style.display = "block";
				document.getElementById('#offlinepaymentview').style.display = "none";
			}
			else {

                document.getElementById('#ccpaymentview').style.display = "none";
                document.getElementById('#offlinepaymentview').style.display = "block";
            }
        }

		$(document).ready(function () {

			changeMethod('cc');

			braintree.dropin.create({
				authorization: '<%= this.ClientToken %>',
				container: '#dropin-container'
			}, function (createErr, instance) {

					purchaseEvent = async () => {
                        console.log(document.getElementById('#price-input').value)
						if (document.getElementById('#price-input').value.length > 0) {

                            document.getElementById('ContentPlaceHolder1_hidPrice').value = document.getElementById('#price-input').value;

							instance.requestPaymentMethod().then((payload) => {

								document.getElementById('ContentPlaceHolder1_hidNonce').value = payload.nonce;

                                console.log(document.getElementById('ContentPlaceHolder1_hidNonce').value)

                                document.querySelector('#form1').submit();
                            });
                        }

					};
			});
		});

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

	<div class="content-boxed m-b-lg">
		<div class="row">
			<div class="col-lg-12">
				<div class="hpanel blog-article-box">
					<div class="panel-heading <%=panelImage%>">
						<%=panelImageSrc%>
						<h4><asp:Literal id="litTitle" runat="server"></asp:Literal></h4>
						<small></small>
						<div class="text-muted small">
							<asp:Literal ID="litHeaderSubTitle" runat="server"></asp:Literal>
							
							<div class="m-t-sm">
								<div class="m-b-lg">
									<h5>
										<asp:Literal ID="litHelpSurvivorMessage" runat="server"></asp:Literal>
									</h5>
								</div>
								<asp:HyperLink ID="hypAmazon" CssClass="btn-success btn-lg m-t-lg m-b-lg" runat="server"></asp:HyperLink>
								<div class="m-t-lg">
									<div class="fb-share-button" 
										data-href="<%=pageFriendlyURL%>" 
										data-layout="button" 
										data-size="large" 
										data-mobile-iframe="true">
									<a target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=<%=pageFriendlyURL%>&src=sdkpreparse" class="fb-xfbml-parse-ignore">Share</a>
									</div>
								</div>
								<div runat="server" class="m-t-lg" id="divArticleAdmin" visible="false">
									<asp:HyperLink CssClass="btn-info btn-sm" ID="hypAddPhoto" runat="server" Text="Insert Article Photo"></asp:HyperLink>
									<asp:HyperLink CssClass="btn-info btn-sm" ID="hypEdit" runat="server" Text="Edit"></asp:HyperLink>
									<asp:HyperLink CssClass="btn-info btn-sm" ID="hypDelete" runat="server" Text="Delete"></asp:HyperLink>
								</div>
							</div>
						</div>
					</div>
					<div class="panel-body">
						<div class="row" runat="server" id="divSurvivorInfo" visible="false">
							<div class="col-sm-7">
								<asp:Literal id="litText" runat="server"></asp:Literal>
							</div>
							<div class="col-sm-5">
								<div class="hpanel" style="padding-bottom:8px;">
									<div class="panel-heading">
										<h2>
											Donate to <asp:Literal ID="litSurvivorNameDonate" runat="server" Visible="false"></asp:Literal>
										</h2>
										<small><asp:Literal ID="litDisasterNameDonate" runat="server" Visible="false"></asp:Literal></small>
									</div>
									<div class="panel-body"  style="padding:8px;">

										<div class="row" style="padding-bottom:16px;">
											<div class="col-sm-1">

											</div>
											<div class="col-sm-10">
												<small>
													This person's information has been vetted and approved.
												</small>
											</div>
										</div>

								<div class="row">
									<div class="col-sm-12">
										<p><strong><%= successMessage %></strong></p>
									</div>
								</div>

								<div class="row">
									<div class="col-sm-12">
										<a onclick="setPrice(10)" class="btn btn-primary btn-sm" style="margin:4px;">$10</a>
										<a onclick="setPrice(25)" class="btn btn-primary btn-sm" style="margin:4px;">$25</a>
										<a onclick="setPrice(50)" class="btn btn-primary btn-sm" style="margin:4px;">$50</a>
										<a onclick="setPrice(100)" class="btn btn-primary btn-sm" style="margin:4px;">$100</a>
										<a onclick="setPrice(250)" class="btn btn-primary btn-sm" style="margin:4px;">$250</a>
										<a onclick="setPrice(500)" class="btn btn-primary btn-sm" style="margin:4px;">$500</a>
										<a onclick="setPrice(1000)" class="btn btn-primary btn-sm" style="margin:4px;">$1000</a>
									</div>
								</div>

								<div class="row">
									<div class="col-sm-7">
										<div class="input-group m-b">
											<span class="input-group-addon">$</span> 
											<input id="#price-input" type="number" class="form-control" placeholder="25" value="25" />
										</div>
									</div>
								</div>

								<div class="row">
									<div class="col-sm-12">
										<div class="input-group">
											<input type="radio" name="paymentmethod" value="creditcard" checked onchange="changeMethod('cc')" />
											<label for="creditcard">Credit/Debit Card Payment</label>
										</div>
										<div class="input-group">
											<input type="radio" name="paymentmethod" value="offline" onchange="changeMethod('offline')"/>
											<label for="offline">Mail a Check/Money Order</label>
										</div>
									</div>
								</div>

								<div class="row" id="#ccpaymentview">
									<div class="col-sm-12">

										<div id="dropin-container"></div>

										<a ID="btnBrainTreeSubmit" onclick="purchaseEvent()" class="btn btn-primary btn-lg btn-block" >Donate</a>
										<input type="hidden" name="payment-method-nonce" runat="server" id="hidNonce" />
										<input type="hidden" name="payment-price" runat="server" id="hidPrice" />
									</div>
								</div>

								<div class="row" id="#offlinepaymentview" visible="false">
									<div class="col-sm-1"></div>
									<div class="col-sm-10">
										<p>Please send your donation to this address.<br />
Cajun Navy Foundation<br />
10231 The Grove Blvd Unit 37<br />
Baton Rouge, La 70836</p>
									</div>
								</div>

									<div class="row" ID="divWhoHasGiven" runat="server" Visible="true">
										<div class="col-sm-1">

										</div>
										<div class="col-sm-10">
											<h4>Who Has Given?</h4>
											<%=donationList %>
										</div>
									</div>


									</div>
								</div>
								<div class="hpanel">
									<div class="panel-body">
										<dl>
											<dt runat="server" id="dtDisasterTitle" visible="false">Disaster</dt>
											<dd><asp:HyperLink ID="hypDisaster" runat="server" visible="false"></asp:HyperLink></dd>
											<dt runat="server" id="dtRecoveryStage" visible="false" class="m-t-md">Recovery Stage</dt>
											<dd><asp:Literal ID="litRecoveryStage" runat="server" visible="false"></asp:Literal></dd>
											<dt runat="server" id="dtHousingTitle" visible="false" class="m-t-md">Current Housing Situation</dt>
											<dd><asp:Literal ID="litHousingSituation" runat="server" visible="false"></asp:Literal></dd>
											<dt runat="server" id="dtSurvivorLinkList" visible="false" class="m-t-md">Survivors I'm Helping</dt>
											<dt runat="server" id="dtQualifiersTitle" visible="false" class="m-t-md">Other Factors</dt>
											<dd><asp:Literal ID="litQualifiers" runat="server" visible="false"></asp:Literal></dd>
											<dd>
												<ul>
													<%=survivorLinkList %>
												</ul>
											</dd>
										</dl>
										<div class="m-t-md">
											<asp:HyperLink ID="hypAllArticles" CssClass="btn-success btn-sm m-t-lg m-b-lg" runat="server"></asp:HyperLink>
										</div>
										<div class="m-t-sm">
											<div class="fb-share-button" 
												data-href="<%=pageFriendlyURL%>" 
												data-layout="button" 
												data-size="large" 
												data-mobile-iframe="true">
											<a target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=<%=pageFriendlyURL%>&src=sdkpreparse" class="fb-xfbml-parse-ignore">Share</a>
											</div>
										</div>

									</div>
								</div>
							</div>
						</div>
						<asp:Literal id="litTextNoSurvivor" runat="server"></asp:Literal>
					</div>
					<div class="panel-body">
						<div class="pull-left">
							<asp:Literal id="litCategory" runat="server"></asp:Literal>
						</div>
					</div>
					<div class="panel-footer">
						<span class="pull-right">
							<i class="fa fa-comments-o"> </i> <asp:Literal id="litCommentCount" runat="server"></asp:Literal> comments
						</span>
						<i class="fa fa-eye"> </i> <asp:Literal id="litViews" runat="server"></asp:Literal> views
					</div>
					<div class="panel-footer">
						<div class="m-lg">
							<div class="social-form m-b">
								<div class="row">
									<div class="col-sm-11">
										<asp:TextBox TextMode="MultiLine" ID="txtComment" runat="server" CssClass="form-control" placeholder="Write a comment..."></asp:TextBox>
									</div>
									<div class="col-sm-1">
										<asp:Button ID="btnComment" runat="server" Text="Post" OnClick="btnComment_Click" />
										<asp:HiddenField ID="hidArticleId" runat="server" />
										<asp:HiddenField ID="hidTitle" runat="server" />
									</div>
								</div>
							</div>
							<asp:Literal ID="litComments" runat="server"></asp:Literal>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<div style="height:400px;"></div>
</asp:Content>