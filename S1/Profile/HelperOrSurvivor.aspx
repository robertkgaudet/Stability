<%@ Page Title="" Language="C#" MasterPageFile="~/S1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="HelperOrSurvivor.aspx.cs" Inherits="S1_Profile_HelperOrSurvivor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
		<script type="text/javascript">

			$(document).ready(function () {
				$('.btn-survivor').click(function () {
					window.location.href = '/S1/Profile/NewBlogPost.aspx?storySubject=survivor';
					return false;
				});
				$('.btn-helper').click(function () {
					window.location.href = '/S1/Profile/NewBlogPost.aspx?storySubject=helper';
					return false;
				});
			});

		</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div class="hpanel">
		<div class="panel-body">
			<a class="small-header-action" href="">
				<div class="clip-header">
					<i class="fa fa-arrow-up"></i>
				</div>
			</a>
			<h2 class="font-light m-b-xs">
				<asp:Literal ID="litChooseSubject" runat="server"></asp:Literal>
			</h2>
			<small>
				Is this story about a Helper or Survivor?
			</small>
		</div>
	</div>
	<div class="hpanel">
		<div class="panel-body">
			<p class="m-b-lg">
				If you are a survivor or this story is about a survivor you are helping, choose survivor.
				<br />
				<button type="button" class="btn btn-primary btn-lg m-t-sm btn-survivor">Survivor Story</button>
			</p>
			<p>
				If you are a volunteer or helping survivors, choose helper.
				<br />
				<button type="button" class="btn btn-primary btn-lg m-t-sm btn-helper">Helper Story</button>
			</p>
		</div>
	</div>
</asp:Content>