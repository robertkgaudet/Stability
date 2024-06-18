<%@ Page Title="" Language="C#" MasterPageFile="~//V1/MasterPages/Basic.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Donate.aspx.cs" Inherits="CrowdRelief.Secure_Donate" %>
<%@ MasterType VirtualPath="~//V1/MasterPages/Basic.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
		.input-group-addon {
		min-width:80px;
		text-align:right;
		}

		.donate {
		  background: #F9F9F9;
		  border-radius:5px;
		  padding: 2px;
		  margin: 40px 0;
		  box-shadow: 0 0 20px 0 rgba(0, 0, 0, 0.2), 0 5px 5px 0 rgba(0, 0, 0, 0.24);
		  font-size: 15px;
		}

		.margin-20{
			margin:20px;
		}
		
		.amount-field{
			min-width:100px; max-width:100px; background-color:#F3F9E4; color:#5E7F08; text-align:center;
		}
		.email-field{
			min-width:220px;
			max-width:220px;
		}
		.phone-field{
			min-width:220px;
			max-width:220px;
		}
		.name-field{
			min-width:210px;
			max-width:210px;
		}
		.date-field{
			min-width:100px;
			max-width:100px;
		}
		.zip-field{
			min-width:100px;
			max-width:100px;
		}
		.cvv-field{
			min-width:100px;
			max-width:100px;
		}


/* show border around full height container */
.h-100 {
    border: 1px dotted #cc2222;
}
	</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<asp:HiddenField runat="server" id="hidItemCostEach"></asp:HiddenField>
	<asp:HiddenField runat="server" id="hidItemCount"></asp:HiddenField>
	<input type="hidden" name="payment-method-nonce" runat="server" id="hidNonce" />

	<div class="row">
		<div class="col-xs-12 col-xs-offset-0 col-sm-8 col-sm-offset-2 col-md-6 col-md-offset-3">
			<div class="donate">
				<div class="container-fluid">
					<div id="divShare" runat="server" class="margin-top-50">
						<h1>SURVIVOR STORY DETAILS GO HERE</h1>
					</div>
					<div id="divDonationContainer" runat="server">
						<div class="input-group">
							Choose An Amount To Donate
							<div class="row">
								<div class="col-sm-12">
									<button type="button" class="btn btn-outline btn-danger btn-lg center-block m-r-sm m-b-sm font-bold" Style="width: 130px; float:left;">$10</button>
									<button type="button" class="btn btn-outline btn-danger btn-lg center-block m-r-sm m-b-sm font-bold" Style="width: 130px; float:left;">$25</button>
									<button type="button" class="btn btn-outline btn-danger btn-lg center-block m-b-sm font-bold" Style="width: 130px; float:left;">$50</button>
								</div>
							</div>
							<div class="row">
								<div class="col-sm-12">
									<button type="button" class="btn btn-outline btn-danger btn-lg center-block m-r-sm m-b-sm font-bold" Style="width: 130px; float:left;">$75</button>
									<button type="button" class="btn btn-outline btn-danger btn-lg center-block m-r-sm m-b-sm font-bold" Style="width: 130px; float:left;">$100</button>
									<button type="button" class="btn btn-outline btn-danger btn-lg center-block m-b-sm font-bold" Style="width: 130px; float:left;">$150</button>
								</div>
							</div>
							<div class="row">
								<div class="col-sm-12">
									<div class="input-group m-b">
										<span class="input-group-addon">$</span> 
										<asp:TextBox runat="server" id="txtAmount" placeholder="Enter a different amount." Class="form-control"></asp:TextBox>
									</div>
								</div>
							</div>
						</div>
						<hr />
						<small>Enter Your Information</small>
						<div class="input-group m-b-sm">
							<label for="first-name" class="input-group-addon">Firstname</label>
							<asp:TextBox runat="server" id="txtFirstName" class="form-control input-md name-field" placeholder="Firstname"></asp:TextBox>
						</div>
								
						<div class="input-group m-b-sm">
							<label for="last-name" class="input-group-addon">Lastname</label>
							<asp:TextBox runat="server" id="txtLastName" class="form-control input-md name-field" placeholder="Lastname"></asp:TextBox>
						</div>
								
						<div class="input-group m-b-sm">
							<label for="email" class="input-group-addon">Email</label>
							<asp:TextBox runat="server" id="txtEmail" class="form-control input-md email-field" placeholder="Email"></asp:TextBox>
						</div>
						<div class="input-group m-b-sm">
							<label for="phone" class="input-group-addon">Phone</label>
							<asp:TextBox runat="server" id="txtPhone" class="form-control input-md phone-field" placeholder="(xxx) xxx-xxxx"></asp:TextBox>
						</div>
						<small><b>Payment Types Accepted</b></small><br />
						<img src="download.png" />
						<div class="input-group m-b-sm">
							<label for="card-number" class="input-group-addon">Card #</label>
							<div id="card-number" class="hosted-field form-control input-md card-field" placeholder="Additional Info"></div>
						</div>
				
						<div class="input-group m-b-sm">
							<label for="expiration-date" class="input-group-addon">Exp Date</label>
							<div id="expiration-date" class="hosted-field form-control input-md date-field"></div>
						</div>
				
						<div class="input-group m-b-sm">
							<label for="postal-date" class="input-group-addon">Zip Code</label>
							<div id="postal-code" class="hosted-field form-control input-md zip-field"></div>
						</div>
				
						<div class="input-group m-b-sm">
							<label for="cvv" class="input-group-addon">CVV</label>
							<div id="cvv" class="hosted-field form-control input-md cvv-field"></div>
						</div>

						<div>
							<asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success btn-lg m-b-sm font-bold" Style="width:100px; float:right" Text="Donate" />
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	



	
    <script src="https://js.braintreegateway.com/web/3.48.0/js/client.min.js"></script>
    <script src="https://js.braintreegateway.com/web/3.48.0/js/hosted-fields.min.js"></script>
	<script>

        $(document).ready(function () {

            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                if (charCode == 110 || charCode == 190 || charCode == 46)
                    return true;

                if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }

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
                            placeholder: '10/2020'
                        },
                        postalCode: {
                            selector: '#postal-code',
                            placeholder: '71111'
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

                            alert(noncestr); // Confirm nonce is received.
                            $('#paymentmethodnonce').attr("value", payload.nonce);
                            form.submit();
                        });
                    }, false);
                });
            });
        });
    </script>
</asp:Content>