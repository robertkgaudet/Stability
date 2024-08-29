<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Child.master" ValidateRequest="false"
	AutoEventWireup="true" CodeFile="ShiftSignup.aspx.cs" Inherits="V1_NonProfitAdministration_ShiftSignup" %>
	<%@ Register Src="~/V1/UserControls/PositionNavigation.ascx" TagPrefix="uc1" TagName="PostionNavigation" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
        <link rel="stylesheet" href="/Homer/vendor/clockpicker/dist/bootstrap-clockpicker.min.css" />
        <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote.css" />
        <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote-bs3.css" />
        <link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />

        <script>
            $(document).ready(function(){
                console.log("js loaded")
            })
        </script>
    
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
			<uc1:PostionNavigation runat="server" ID="ucPostionNavigation" />
            <h1>Sign-up for shifts</h1>
    </asp:Content>