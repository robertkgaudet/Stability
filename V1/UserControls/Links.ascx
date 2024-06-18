<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Links.ascx.cs" Inherits="V1_UserControls_Links" %>
					
			        <style>
			            .LinkDescription { color:#365899; font-weight:normal !important;}
                        .EventLink  { color:#365899; font-weight:bold !important; text-decoration:underline;}

                       /* .drag_disabled{
                                pointer-events: none;
                            }

                        .drag_enabled{
                            pointer-events: all;
                        }*/

			        </style>
                    <div class="content">
                        <p class="m-t-lg m-b-lg">
                                                    <h2>Helpful Links For This Disaster</h2>
                            We've compiled this helpful list of links. Use it for finding local resources, agencies that can help and to learn about how best to start your recovery.
                            <br />
                            <div id="nestable-menu">
                            <button type="button" data-action="expand-all" class="btn btn-default btn-sm">Expand All</button>
                            <button type="button" data-action="collapse-all" class="btn btn-default btn-sm">Collapse All</button>

                            </div>
                        </p>
                        
                        
                        <div class="dd drag_disabled" id="nestable">
                            <ol class="dd-list dd-nodrag">
                                <asp:Repeater ID="rptCategories" runat="server" OnItemDataBound="rptCategories_ItemDataBound">
							        <ItemTemplate>
                                        <li class="dd-item dd-nodrag">
                                            <div class="dd-handle">
                                                <span class="font-bold"><asp:Literal ID="litParentCategory" runat="server"></asp:Literal></span>
                                            </div>
                                            <ol class="dd-list">
                                                <asp:Repeater ID="rptLinks" runat="server" OnItemDataBound="rptLinks_ItemDataBound">
											        <ItemTemplate>
                                                        <li class="dd-item">
                                                            <div class="dd-handle">
                                                                 <asp:HyperLink CssClass="EventLink" ID="hypLinkText" runat="server" Target="_blank"></asp:HyperLink> -
                                                                <asp:Label ID="lblDescription" runat="server" CssClass="LinkDescription"></asp:Label>
                                                                <div class="m-b-sm" runat="server" id="divDeleteLinks" visible="false">
														            <i class="fa fa-lock"></i> <asp:LinkButton CssClass="deleteLinkArticle" OnClick="lbDeleteLink_Click" ID="lbDeleteLink" runat="server" Text="Remove Link"></asp:LinkButton>
													            </div>
                                                            </div>
                                                        </li>
											        </ItemTemplate>
										        </asp:Repeater>
                                            </ol>
                                        </li>
							        </ItemTemplate>
						        </asp:Repeater>
                            </ol>
                        </div>
                    </div>
					<%--<div class="grid">
						<asp:Repeater ID="rptCategories" runat="server" OnItemDataBound="rptCategories_ItemDataBound">
							<ItemTemplate>
								<div class="hpanel horange grid-item">
									<div class="panel-heading hbuilt">
										<asp:Literal ID="litParentCategory" runat="server"></asp:Literal>
									</div>
									<div class="panel-body">
										<dl>
											<asp:Repeater ID="rptLinks" runat="server" OnItemDataBound="rptLinks_ItemDataBound">
												<ItemTemplate>
													<dt class="m-b-xs">
														<asp:HyperLink CssClass="EventLink" ID="hypLinkText" runat="server" Target="_blank"></asp:HyperLink>
													</dt>
													<dd class="m-b-sm">
														
													</dd>
													<div class="m-b-sm" runat="server" id="divDeleteLinks" visible="false">
														<i class="fa fa-lock"></i> <asp:LinkButton CssClass="deleteLinkArticle" OnClick="lbDeleteLink_Click" ID="lbDeleteLink" runat="server" Text="Remove Link"></asp:LinkButton>
													</div>
												</ItemTemplate>
											</asp:Repeater>
										</dl>
										<asp:Repeater ID="rptArticles" runat="server" OnItemDataBound="repArticles_ItemDataBound">
											<ItemTemplate>
												<h4><asp:Label ID="lblText" runat="server"></asp:Label></h4>
												<asp:Label ID="lblDescription" runat="server"></asp:Label>
												<div class="m-b-sm" runat="server" id="divDeleteArticle" visible="false">
													<i class="fa fa-lock"></i> <asp:LinkButton CssClass="deleteLinkArticle" OnClick="lbDeleteArticle_Click" ID="lbDeleteArticle" runat="server" Text="DELETE ARTICLE"></asp:LinkButton>
												</div>
											</ItemTemplate>
										</asp:Repeater>
									</div>
								</div>
							</ItemTemplate>
						</asp:Repeater>
					</div>--%>
        <script src="/Homer/vendor/nestable/jquery.nestable.js"></script>

<script>
    $(document).ready(function () {

        $('#nestable').nestable({
            group: 1, maxDepth: 0
        }).nestable('collapseAll');

        //const nestable = new Nestable('#nestable');
        //nestable.collapseAll();
        $('.nestable').nestable({ handleClass: '123' });
        //$("#nestable").nestable('collapseAll');

        $('#nestable-menu').on('click', function (e) {
            var target = $(e.target),
                action = target.data('action');
            if (action === 'expand-all') {
                $('.dd').nestable('expandAll');
            }
            if (action === 'collapse-all') {
                $('.dd').nestable('collapseAll');
            }
        });
     });
</script>