<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/SecurePayment.master" autoeventwireup="true" inherits="Secure_Test, App_Web_d2x3jl3l" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--      <label for="card-number">Card Number</label>
      <div id="card-number"></div>

      <label for="cvv">CVV</label>
      <div id="cvv"></div>

      <label for="expiration-date">Expiration Date</label>
      <div id="expiration-date"></div>

      <input type="submit" value="Pay" disabled />--%>







	<asp:HiddenField id="hidNonceControl" runat="server"></asp:HiddenField>
	<div class="container center-block">
		<div class="row">
			<div class="col-xs-12 margin-2">
				<div class="donate">
					<div class="container-fluid">
						<div class="row">
							<div class="col-xs-12 col-sm-6 pull-right">
								<div class="well">
									<asp:Label ID="lblCampaignTitle" Font-Bold="true" runat="server"></asp:Label>
									<br /><br />
									<asp:Image ID="imgCampaign" CssClass="img-responsive" runat="server" />
									<div class="margin-20">
										<asp:Label CssClass="OpenNeedAmtSmall" ID="lblAmountNeeded" runat="server"></asp:Label> <br />in needs still remain open.
									</div>
									<div class="margin">
										<asp:Label ID="lblItemDescription" runat="server"></asp:Label>
									</div>
								</div>
							</div>
							<div class="col-xs-12 col-sm-6">
								<h1>Donate a <asp:Literal ID="litItemName" runat="server"></asp:Literal></h1>
								Your donation will be used to provide this item directly to <asp:Literal ID="litClientName" runat="server"></asp:Literal>
								<div id="divSuccess" runat="server" class="alert alert-success margin-top-50 text-center">
									<h1>Donation Accepted!</h1> <asp:Label id="lblSuccess" runat="server"></asp:Label>
								</div>
								<div id="divWarning" runat="server" class="alert alert-danger margin-top-50 text-center">
									<h1>Error Detected</h1> <asp:Label id="lblWarning" runat="server"></asp:Label>
								</div>
								<div id="divShare" runat="server" class="margin-top-50">
									Share your donation and encourage others to help!
									<asp:HyperLink ID="hypReturnToCampaign" runat="server"></asp:HyperLink>
								</div>
								<div id="divDonationContainer" runat="server">
									<div class="input-group margin-20">
										<label for="amount">Enter your donation.</label><br />
										<asp:TextBox runat="server" id="txtAmount" onkeypress="return isNumberKey(event)" class="form-control input-lg amount-field" name="number" placeholder="$"></asp:TextBox>
										<asp:Literal id="litCountMessage" runat="server"></asp:Literal>
									</div>
								
									<div class="input-group margin-20">
										<label for="first-name" class="input-group-addon">Firstname</label>
										<asp:TextBox runat="server" id="txtFirstName" class="form-control input-md name-field" placeholder="Firstname"></asp:TextBox>
									</div>
								
									<div class="input-group margin-20">
										<label for="last-name" class="input-group-addon">Lastname</label>
										<asp:TextBox runat="server" id="txtLastName" class="form-control input-md name-field" placeholder="Lastname"></asp:TextBox>
									</div>
								
									<div class="input-group margin-20">
										<label for="email" class="input-group-addon">Email</label>
										<asp:TextBox runat="server" id="txtEmail" class="form-control input-md email-field" placeholder="Email"></asp:TextBox>
									</div>
								
									<div class="input-group margin-20">
										<label for="phone" class="input-group-addon">Phone</label>
										<asp:TextBox runat="server" id="txtPhone" class="form-control input-md phone-field" placeholder="(xxx) xxx-xxxx"></asp:TextBox>
									</div>

									<div class="input-group margin-20">
										<label for="card-number" class="input-group-addon">Card #</label>
										<div id="card-number" class="hosted-field form-control input-md card-field" placeholder="Additional Info"></div>
									</div>
				
									<div class="input-group margin-20">
										<label for="expiration-date" class="input-group-addon">Exp Date</label>
										<div id="expiration-date" class="hosted-field form-control input-md date-field"></div>
									</div>
				
									<div class="input-group margin-20">
										<label for="postal-date" class="input-group-addon">Zip Code</label>
										<div id="postal-code" class="hosted-field form-control input-md zip-field"></div>
									</div>
				
									<div class="input-group margin-20">
										<label for="cvv" class="input-group-addon">CVV</label>
										<div id="cvv" class="hosted-field form-control input-md cvv-field"></div>
									</div>
				
									<div class="input-group col-xs-8 margin-20">
										<input type="hidden" name="paymentmethodnonce" runat="server" id="hidNonce" />
										<asp:Button ID="btnSubmit" runat="server" Text="Donate" class="btn-lg btn-success btn-block" />
										<asp:HiddenField runat="server" id="hidItemCostEach"></asp:HiddenField>
										<asp:HiddenField runat="server" id="hidItemCount"></asp:HiddenField>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>

    <script src="https://js.braintreegateway.com/web/3.48.0/js/client.min.js"></script>
    <script src="https://js.braintreegateway.com/web/3.48.0/js/hosted-fields.min.js"></script>
	<script>
		var form = document.querySelector('#form1');
		var submit = document.querySelector('input[type="submit"]');

      braintree.client.create({
		  authorization: 'sandbox_v28bwv34_bzzxdb3b496mcmg8'//eyJ2ZXJzaW9uIjoyLCJhdXRob3JpemF0aW9uRmluZ2VycHJpbnQiOiJlMjRlZTNlNmFkZDRlMGJmMGQ1ZjdmNGUzMzEzNGE5OGMwM2VhZjU5ZWM1OTUxNTllYzkyNGMxNGE0NjA5YzU2fGNyZWF0ZWRfYXQ9MjAxNy0wMy0wM1QyMToyMDo0NS41Mjg0ODkwNDErMDAwMFx1MDAyNm1lcmNoYW50X2lkPTM0OHBrOWNnZjNiZ3l3MmJcdTAwMjZwdWJsaWNfa2V5PTJuMjQ3ZHY4OWJxOXZtcHIiLCJjb25maWdVcmwiOiJodHRwczovL2FwaS5zYW5kYm94LmJyYWludHJlZWdhdGV3YXkuY29tOjQ0My9tZXJjaGFudHMvMzQ4cGs5Y2dmM2JneXcyYi9jbGllbnRfYXBpL3YxL2NvbmZpZ3VyYXRpb24iLCJjaGFsbGVuZ2VzIjpbXSwiZW52aXJvbm1lbnQiOiJzYW5kYm94IiwiY2xpZW50QXBpVXJsIjoiaHR0cHM6Ly9hcGkuc2FuZGJveC5icmFpbnRyZWVnYXRld2F5LmNvbTo0NDMvbWVyY2hhbnRzLzM0OHBrOWNnZjNiZ3l3MmIvY2xpZW50X2FwaSIsImFzc2V0c1VybCI6Imh0dHBzOi8vYXNzZXRzLmJyYWludHJlZWdhdGV3YXkuY29tIiwiYXV0aFVybCI6Imh0dHBzOi8vYXV0aC52ZW5tby5zYW5kYm94LmJyYWludHJlZWdhdGV3YXkuY29tIiwiYW5hbHl0aWNzIjp7InVybCI6Imh0dHBzOi8vY2xpZW50LWFuYWx5dGljcy5zYW5kYm94LmJyYWludHJlZWdhdGV3YXkuY29tLzM0OHBrOWNnZjNiZ3l3MmIifSwidGhyZWVEU2VjdXJlRW5hYmxlZCI6dHJ1ZSwicGF5cGFsRW5hYmxlZCI6dHJ1ZSwicGF5cGFsIjp7ImRpc3BsYXlOYW1lIjoiQWNtZSBXaWRnZXRzLCBMdGQuIChTYW5kYm94KSIsImNsaWVudElkIjpudWxsLCJwcml2YWN5VXJsIjoiaHR0cDovL2V4YW1wbGUuY29tL3BwIiwidXNlckFncmVlbWVudFVybCI6Imh0dHA6Ly9leGFtcGxlLmNvbS90b3MiLCJiYXNlVXJsIjoiaHR0cHM6Ly9hc3NldHMuYnJhaW50cmVlZ2F0ZXdheS5jb20iLCJhc3NldHNVcmwiOiJodHRwczovL2NoZWNrb3V0LnBheXBhbC5jb20iLCJkaXJlY3RCYXNlVXJsIjpudWxsLCJhbGxvd0h0dHAiOnRydWUsImVudmlyb25tZW50Tm9OZXR3b3JrIjp0cnVlLCJlbnZpcm9ubWVudCI6Im9mZmxpbmUiLCJ1bnZldHRlZE1lcmNoYW50IjpmYWxzZSwiYnJhaW50cmVlQ2xpZW50SWQiOiJtYXN0ZXJjbGllbnQzIiwiYmlsbGluZ0FncmVlbWVudHNFbmFibGVkIjp0cnVlLCJtZXJjaGFudEFjY291bnRJZCI6ImFjbWV3aWRnZXRzbHRkc2FuZGJveCIsImN1cnJlbmN5SXNvQ29kZSI6IlVTRCJ9LCJjb2luYmFzZUVuYWJsZWQiOmZhbHNlLCJtZXJjaGFudElkIjoiMzQ4cGs5Y2dmM2JneXcyYiIsInZlbm1vIjoib2ZmIn0='
		}, function (clientErr, clientInstance) {
        if (clientErr) {
          console.error(clientErr);
          return;
        }

        // This example shows Hosted Fields, but you can also use this
        // client instance to create additional components here, such as
        // PayPal or Data Collector.

        braintree.hostedFields.create({
          client: clientInstance,
          styles: {
            'input': {
              'font-size': '14px'
            },
            'input.invalid': {
              'color': 'red'
            },
            'input.valid': {
              'color': 'green'
            }
          },
          fields: {
            number: {
              selector: '#card-number',
              placeholder: '4111 1111 1111 1111'
            },
            cvv: {
              selector: '#cvv',
              placeholder: '123'
            },
            expirationDate: {
              selector: '#expiration-date',
              placeholder: '10/2019'
            }
          }
        }, function (hostedFieldsErr, hostedFieldsInstance) {
          if (hostedFieldsErr) {
            console.error(hostedFieldsErr);
            return;
          }

          submit.removeAttribute('disabled');

				submit.on('click', function () {
					    hostedFieldsInstance.tokenize(function (tokenizeErr, payload) {
					        if (tokenizeErr) {
					            // Handle error in Hosted Fields tokenization
					            return;
					        }
					        var noncestr = payload.nonce
					        alert(noncestr); // Confirm nonce is received.
                            $('#paymentmethodnonce').attr("value", noncestr); // Add nonce to form element.
                            //document.querySelector('input[id="<%=hidNonce.ClientID%>"]').value = payload.nonce;
					        form.submit();
					    });
					}, false);
        });
      });
    </script>
</asp:Content>

